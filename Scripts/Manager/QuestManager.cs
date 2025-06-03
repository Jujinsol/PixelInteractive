using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class QuestManager : MonoBehaviour
{
    public static QuestManager _inst;
    public GameObject _questText, _diamondQuest, _theftQuest;
    public Slider _sliderTheft;
    public bool _acceptQuest = false;
    public int _diamond = 0, _currentMission = 0;
    public string _theftColor = "";

    public GameObject QuestText
    {
        get
        {
            if (_questText == null)
                _questText = GameObject.Find("Canvas").transform.Find("ImgQuest").gameObject;
            return _questText;
        }
    }

    public GameObject DiamondQuest
    {
        get
        {
            if (_diamondQuest == null)
                _diamondQuest = GameObject.Find("Canvas").transform.Find("DiamondQuest").gameObject;
            return _diamondQuest;
        }
    }

    public GameObject TheftQuest
    {
        get
        {
            if (_theftQuest == null)
                _theftQuest = GameObject.Find("Canvas").transform.Find("TheftQuest").gameObject;
            return _theftQuest;
        }
    }

    public Slider SliderTheft
    {
        get
        {
            if (_sliderTheft == null)
                _sliderTheft = GameObject.Find("Canvas").transform.Find("TheftQuest").Find("sliderTheft").GetComponent<Slider>();
            return _sliderTheft;
        }
    }

    void Start()
    {
        _inst = this;
    }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name == "Map1")
        {
            _currentMission = 0;
            QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "산책을 하다가 엄마의 다이아몬드를 잃어버렸어... \n찾아줄 수 있어?";
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnYes").gameObject.GetComponent<Button>().onClick.AddListener(AcceptQuest);
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnNo").gameObject.GetComponent<Button>().onClick.AddListener(RejectQuest);
        }
        else if (SceneManager.GetActiveScene().name == "Map2")
        {
            _currentMission = 1;
            QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "저 분홍색 녀석이 내 보물을 훔쳐갔어\n지금이라면 복수할 수 있을 것 같아. 같이 할래?";
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnYes").gameObject.GetComponent<Button>().onClick.AddListener(AcceptQuest);
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnNo").gameObject.GetComponent<Button>().onClick.AddListener(RejectQuest);
            GameObject.Find("Canvas").transform.Find("TheftQuest").Find("btnClose").gameObject.GetComponent<Button>().onClick.AddListener(CloseTheft);
            GameObject.Find("Canvas").transform.Find("TheftQuest").Find("btnSubmit").gameObject.GetComponent<Button>().onClick.AddListener(SubmitTheft);
        }
    }

    public void CheckQuestTxt()
    {
        if (!QuestText.activeSelf)
            QuestText.SetActive(true);
        else
            QuestText.SetActive(false);
    }

    public void AcceptQuest()
    {
        QuestText.transform.Find("btnYes").gameObject.SetActive(false);
        QuestText.transform.Find("btnNo").gameObject.SetActive(false);

        switch (_currentMission)
        {
            case 0:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "참 착하구나! 기다리고 있을게!";
                DiamondQuest.SetActive(true);
                break;
            case 1:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "오호! 야망이 있구나!\n저 녀석을 공격해줘!";
                break;
            case 2:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "크크... 문 근처로 가서 문을 따줘\n난 여기서 망을 볼게.";
                break;
        }

        _acceptQuest = true;
    }

    public void RejectQuest()
    {
        //GameObject npc = GameObject.FindGameObjectWithTag("NPC").gameObject;
        //npc.GetComponent<NPCController>().enabled = true;
        _acceptQuest = false;

        _questText.transform.Find("btnYes").gameObject.SetActive(false);
        _questText.transform.Find("btnNo").gameObject.SetActive(false);


        switch (_currentMission)
        {
            case 0:
                _questText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "도움을 주기 싫은 모양이네...";
                cshGameManager._inst._badthings += 5;
                break;
            case 1:
            case 2:
                GameObject.Find("Dino").GetComponent<BoxCollider2D>().enabled = false;
                _questText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "겁쟁이...";
                cshGameManager._inst._badthings -= 5;
                break;
        }

        Debug.Log("The bad things I’ve done : " + cshGameManager._inst._badthings);
    }

    public void AddDiamond()
    {
        _diamond++;
        if (_acceptQuest && _diamond >= 1)
        {
            _diamondQuest.transform.Find("txtDiamond").GetComponent<TextMeshProUGUI>().fontSize = 17;
            _diamondQuest.transform.Find("txtDiamond").GetComponent<TextMeshProUGUI>().text = "NPC에게로 돌아가기";
            MissionComplete();
        }
        else
            _diamondQuest.transform.Find("txtDiamond").GetComponent<TextMeshProUGUI>().text = _diamond + " / 5";
    }

    public void MissionComplete()
    { 
        switch (_currentMission)
        {
            case 0:
                _questText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "다이아몬드를 전부 모아왔구나! 고마워! \n선물로 공격력을 증가시켜줄게!";
                StartCoroutine(WaitForMouseClick1());
                break;
            case 1:
                _questText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "이제 아무도 날 무시하지 못할 거야...";
                StartCoroutine(WaitForMouseClick2());
                break;
        }
    }

    public void CloseTheft()
    {
        TheftQuest.SetActive(false);
        Debug.Log(_theftColor);
        _theftColor = "";
        SliderTheft.value = 0;
    }

    public void SubmitTheft()
    {
        if (_theftColor == "redgreenblueyellow" && (SliderTheft.value < 0.75 && SliderTheft.value > 0.6))
        {
            Debug.Log("OK");
        }
        else
        {
            Debug.Log("NO");
        }
    }

    public void TheftMission()
    {
        Button[] btns = TheftQuest.transform.Find("Colors").GetComponentsInChildren<Button>();

        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => SpeakName(btn.name));
        }
    }

    void SpeakName(string buttonName)
    {
        _theftColor += buttonName;
    }

    IEnumerator WaitForMouseClick1()
    {
        // 마우스 클릭될 때까지 기다림
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        cshPlayerController._inst._arrow = Resources.Load<GameObject>("Prefabs/PowerArrow");
        cshPlayerController._inst._attackPower += 10;
        _diamondQuest.SetActive(false);
        _questText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "날씨가 참 좋네!";
        _acceptQuest = false;
    }

    IEnumerator WaitForMouseClick2()
    {
        // 마우스 클릭될 때까지 기다림
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        _currentMission = 2;
        GameObject.Find("Frog").transform.position = new Vector3(24, 3.7f, 0);
        _questText.transform.Find("btnYes").gameObject.SetActive(true);
        _questText.transform.Find("btnNo").gameObject.SetActive(true);
        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "이 앞 빈 집을 털어볼까?\n큰 돈을 벌 수 있을지도 몰라.";
        TheftMission();
    }
}
