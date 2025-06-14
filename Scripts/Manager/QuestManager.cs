using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class QuestManager : MonoBehaviour
{
    public static QuestManager _inst;
    GameObject _questText, _diamondQuest, _theftQuest;
    Slider _sliderTheft;

    public bool _acceptQuest = false, iAmTheft = false;
    public int _diamond = 0, _currentMap = 0, _currentMission = 0;
    public string _theftColor = "";
    public int[] finalStory = new int[11];

    public CanvasGroup _blackScreen;

    #region
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

    public CanvasGroup BlackScreen
    {
        get
        {
            if (_blackScreen == null)
                _blackScreen = GameObject.Find("Canvas").transform.Find("BlackScreen").GetComponent<CanvasGroup>();
            return _blackScreen;
        }
    }
    #endregion

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
            _currentMap = 0;
            QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "저 독수리들이 내가 배신자라며 소중한 다이아를\n훔쳐갔어. 누군가 되찾아준다면 좋을 텐데...";
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnYes").gameObject.GetComponent<Button>().onClick.AddListener(AcceptQuest);
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnNo").gameObject.GetComponent<Button>().onClick.AddListener(RejectQuest);

            GameObject.Find("Canvas").transform.Find("OpenBook").Find("btnYes").gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("btnYes");
                cshGameManager._inst.ButtonClick();
                SceneManager.LoadScene("Map2");
            });
            GameObject.Find("Canvas").transform.Find("OpenBook").Find("btnNo").gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("btnNo");
                cshGameManager._inst.ButtonClick();
                GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.SetActive(false);
            });
        }
        else if (SceneManager.GetActiveScene().name == "Map2")
        {
            _currentMap = 1;
            QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "저 거북이 녀석이 내 1등을 훔쳐갔어\n지금이라면 복수할 수 있을 것 같아!";
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnYes").gameObject.GetComponent<Button>().onClick.AddListener(AcceptQuest);
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnNo").gameObject.GetComponent<Button>().onClick.AddListener(RejectQuest);
            GameObject.Find("Canvas").transform.Find("TheftQuest").Find("btnClose").gameObject.GetComponent<Button>().onClick.AddListener(CloseTheft);
            GameObject.Find("Canvas").transform.Find("TheftQuest").Find("btnSubmit").gameObject.GetComponent<Button>().onClick.AddListener(SubmitTheft);
        }
        else if (SceneManager.GetActiveScene().name == "Map3")
        {
            _currentMap = 3;
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnYes").gameObject.GetComponent<Button>().onClick.AddListener(AcceptQuest);
            GameObject.Find("Canvas").transform.Find("ImgQuest").Find("btnNo").gameObject.GetComponent<Button>().onClick.AddListener(RejectQuest);
            GameObject.Find("Canvas").transform.Find("FinalStory").gameObject.GetComponent<Button>().onClick.AddListener(FinalStory);
        }
    }

    public void CheckQuestTxt()
    {
        if (!QuestText.activeSelf)
        {
            QuestText.SetActive(true);
        }
        else
        {
            QuestText.SetActive(false);
        }
    }

    public void AcceptQuest()
    {
        cshGameManager._inst.ButtonClick();

        QuestText.transform.Find("btnYes").gameObject.SetActive(false);
        QuestText.transform.Find("btnNo").gameObject.SetActive(false);

        switch (_currentMap)
        {
            case 0:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "참 착하구나! 기다리고 있을게!";
                finalStory[0] = 1;
                DiamondQuest.SetActive(true);
                break;
            case 1:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "오호! 야망이 있구나!\n저 녀석을 공격해줘!";
                finalStory[3] = 1;
                break;
            case 2:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "크크... 문 근처로 가서 문을 따줘\n난 여기서 망을 볼게.";
                finalStory[4] = 1;
                break;
            case 3:
                switch (_currentMission)
                {
                    case 1:
                        finalStory[6] = 1;
                        StartCoroutine(FadeSequence());
                        break;
                    case 2:
                        finalStory[7] = 1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "너무 무거워서 들지 못하겠는데?";
                        break;
                    case 3:
                        finalStory[8] = 1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "먹다 남은 사과라니... 이거라도 챙겨야지.";
                        cshPlayerController._inst.Cure();
                        break;
                    case 5:
                        finalStory[9] = 1;
                        StartCoroutine(FadeSequence());
                        break;
                    case 7:
                        finalStory[10] = 1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "아무래도 한 몫 챙겨야겠어.";
                        Destroy(GameObject.Find("Laptop").gameObject, 2.0f);
                        break;
                }
                break;
        }

        Debug.Log(string.Join("", finalStory));
        _acceptQuest = true;
    }

    public void RejectQuest()
    {
        //GameObject npc = GameObject.FindGameObjectWithTag("NPC").gameObject;
        //npc.GetComponent<NPCController>().enabled = true;
        _acceptQuest = false;
        cshGameManager._inst.ButtonClick();

        QuestText.transform.Find("btnYes").gameObject.SetActive(false);
        QuestText.transform.Find("btnNo").gameObject.SetActive(false);


        switch (_currentMap)
        {
            case 0:
                finalStory[0] = -1;
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "나 혼자 찾아보지 뭐...";
                break;
            case 1:
                finalStory[1] = -1;
                GameObject.Find("Turtle").GetComponent<BoxCollider2D>().enabled = false;
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "겁쟁이...";
                break;
            case 2:
                finalStory[2] = -1;
                GameObject.Find("Turtle").GetComponent<BoxCollider2D>().enabled = false;
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "겁쟁이...";
                break;
            case 3:
                switch (_currentMission)
                {
                    case 1:
                        finalStory[4] = -1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "남 집에서 잠이나 잘 생각을 하다니!";
                        break;
                    case 2:
                        finalStory[5] = -1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "도둑질을 하려니까 양심에 찔리는 걸...";
                        break;
                    case 3:
                        finalStory[6] = -1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "남의 집 냉장고를 함부로 여는 사람은 되고 싶지 않아.";
                        break;
                    case 5:
                        finalStory[7] = -1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "목욕이라니!\n머리가 정말 어떻게 됐나 봐...";
                        break;
                    case 7:
                        finalStory[8] = -1;
                        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "도둑질을 하려니까 양심에 찔리는 걸...";
                        break;
                }
                break;
        }

        Debug.Log(string.Join("", finalStory));
    }

    public void AddDiamond()
    {
        _diamond++;
        if (_acceptQuest && _diamond >= 5)
        {
            DiamondQuest.transform.Find("txtDiamond").GetComponent<TextMeshProUGUI>().fontSize = 17;
            DiamondQuest.transform.Find("txtDiamond").GetComponent<TextMeshProUGUI>().text = "박쥐에게로 돌아가기";
            MissionComplete();
        }
        else
            DiamondQuest.transform.Find("txtDiamond").GetComponent<TextMeshProUGUI>().text = _diamond + " / 5";
    }

    public void MissionComplete()
    { 
        switch (_currentMap)
        {
            case 0:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "다이아몬드를 전부 모아왔구나! 고마워! \n선물로 공격력을 증가시켜줄게!";
                StartCoroutine(WaitForMouseClick1());
                break;
            case 1:
                QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "이제 아무도 날 무시하지 못할 거야...";
                StartCoroutine(WaitForMouseClick2());
                break;
        }
        _acceptQuest = false;
    }

    public void CloseTheft()
    {
        cshGameManager._inst.ButtonClick();
        TheftQuest.SetActive(false);
        _theftColor = "";
        SliderTheft.value = 0;
    }

    public void SubmitTheft()
    {
        if (_theftColor == "redgreenblueyellow" && (SliderTheft.value < 0.75 && SliderTheft.value > 0.6))
        {
            finalStory[5] = 1;
            cshHouseController._inst.DoorUnlock();

            TheftQuest.SetActive(false);
            _acceptQuest = false;
            iAmTheft = true;

            SceneManager.LoadScene("Map3");
        }
        else
        {
            cshGameManager._inst.ButtonFailed();
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
        cshGameManager._inst.ButtonClick();
    }

    void FinalStory()
    {
        string storyLine = "";
        string storyLine2 = "";

        int aboutTurtle = 0;

        for (int i = 0; i < finalStory.Length; i++)
        {
            switch (i)
            {
                case 0: // 박쥐
                    if (finalStory[i] == 1)
                        storyLine += "당신은 낙인 찍힌 박쥐를 도와주기로 했습니다.\n";
                    else if (finalStory[i] == -1)
                        storyLine += "당신은 박쥐의 간절한 부탁을 외면했습니다.\n";
                    break;
                case 1: // 독수리 밥 줌
                    if (finalStory[i] == 1)
                        storyLine += "당신은 독수리에게 먹이를 주었습니다.\n";
                    else if (finalStory[i] == -1)
                        storyLine += "굶주린 독수리를 외면했습니다.\n";
                    break;
                case 2: // 독수리 죽임
                    if (finalStory[i] == 1)
                        storyLine += "당신은 독수리를 쓰러뜨렸습니다.\n";
                    else if (finalStory[i] == -1)
                        storyLine += "당신은 끝까지 생명을 빼앗지 않았습니다.\n";
                    break;
                case 3: // 토끼 미션 1
                    if (finalStory[i] == 1)
                        storyLine += "토끼의 거북이 복수 계획에 동참했습니다.\n";
                    else if (finalStory[i] == -1)
                        storyLine += "토끼의 복수심을 말리려 했습니다.\n";
                    break;
                case 4: // 토끼 미션 2
                    if (finalStory[i] == 1)
                        storyLine += "당신은 결국 거북이 집을 털기로 결정했습니다.\n";
                    else if (finalStory[i] == -1)
                        storyLine += "당신은 토끼의 계획에서 발을 뺐습니다.\n";
                    break;
                case 5: // 문 따기
                    if (finalStory[i] == 1)
                        storyLine += "거북이 집의 문을 조용히 따고 들어갔습니다.\n";
                    else if (finalStory[i] == -1)
                        storyLine += "문을 열려다 양심에 가책을 느끼고 멈췄습니다.\n";
                    break;
                case 6: // 잠 자기
                    if (finalStory[i] == 1)
                        aboutTurtle--;
                    else if (finalStory[i] == -1)
                        aboutTurtle++;
                    break;

                case 7: // 티비 훔치기
                    if (finalStory[i] == 1)
                        aboutTurtle--;
                    else if (finalStory[i] == -1)
                        aboutTurtle++;
                    break;

                case 8: // 냉장고 사과
                    if (finalStory[i] == 1)
                        aboutTurtle--;
                    else if (finalStory[i] == -1)
                        aboutTurtle++;
                    break;

                case 9: // 목욕
                    if (finalStory[i] == 1)
                        aboutTurtle--;
                    else if (finalStory[i] == -1)
                        aboutTurtle++;
                    break;

                case 10: // 랩탑 훔치기
                    if (finalStory[i] == 1)
                        aboutTurtle--;
                    else if (finalStory[i] == -1)
                        aboutTurtle++;
                    break;
            }
        }

        if (aboutTurtle <= -3) storyLine2 += "당신은 결국 탐욕에 굴복했습니다. 거북이의 집은 엉망이 되었습니다. 당신의 행동은 동화 속 세계에 큰 상처를 남겼습니다. 누군가의 신뢰는 한 번 무너지면 회복되기 어렵습니다.";
        else if (aboutTurtle >= 3) storyLine2 += "결국, 당신은 온갖 유혹을 떨쳐내고 거북이의 집에서 조용히 나섰습니다.\n당신의 행동은 도덕적인 본보기가 되었고, 동화 속 세상에 평화가 찾아왔습니다.";
        else storyLine2 += "당신은 일부 유혹에 흔들리기도 했지만 끝내 완전히 나쁘지도 착하지도 않은 선택을 했습니다.\n당신의 발자취는 누군가의 기억 속에 남겠지만, 그 끝이 해피엔딩인지는 아무도 모릅니다.";

        GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("txtOpenBook1").GetComponent<TextMeshProUGUI>().text = storyLine;
        GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("txtOpenBook2").GetComponent<TextMeshProUGUI>().text = storyLine2;

        GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.SetActive(true);
    }

    IEnumerator WaitForMouseClick1()
    {
        // 마우스 클릭될 때까지 기다림
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        cshGameManager._inst._arrow = Resources.Load<GameObject>("Prefabs/PowerArrow");
        cshGameManager._inst._attackPower += 20;
        DiamondQuest.SetActive(false);
        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "날씨가 참 좋네!";
        _acceptQuest = false;
    }

    IEnumerator WaitForMouseClick2()
    {
        // 마우스 클릭될 때까지 기다림
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        _currentMap = 2;
        GameObject.Find("Rabbit").transform.position = new Vector3(25, 4, 0);
        QuestText.transform.Find("btnYes").gameObject.SetActive(true);
        QuestText.transform.Find("btnNo").gameObject.SetActive(true);
        QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "이 앞 거북이 집을 털어볼까?\n복수의 끝을 달려야겠어.";
        TheftMission();
    }
    // 페이드 인 (천천히 보이게)
    public IEnumerator FadeIn()
    {
        BlackScreen.gameObject.SetActive(true);
        float time = 0f;
        while (time < 1.0f)
        {
            BlackScreen.alpha = Mathf.Lerp(0f, 1f, time / 1.0f);
            time += Time.deltaTime;
            yield return null;
        }
        BlackScreen.alpha = 1f;
    }

    // 페이드 아웃 (천천히 사라지게)
    public IEnumerator FadeOut()
    {
        float time = 0f;
        while (time < 1.0f)
        {
            BlackScreen.alpha = Mathf.Lerp(1f, 0f, time / 1.0f);
            time += Time.deltaTime;
            yield return null;
        }
        BlackScreen.alpha = 0f;
        BlackScreen.gameObject.SetActive(false);
    }

    private IEnumerator FadeSequence()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1f); // 중간에 잠깐 정지
        yield return StartCoroutine(FadeOut());
        if (_currentMission == 1)
            QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "정말 개운해!";
        else if (_currentMission == 5)
            QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "뽀득뽀득 남의 욕조만큼 기분 좋을 게 없지!";
    }
}
