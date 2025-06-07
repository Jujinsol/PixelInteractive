using UnityEngine;
using static Define;
using UnityEngine.SceneManagement;
using TMPro;

public class LittlePlayerController : cshPlayerController
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bookcase")
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "여기 사는 거북이는 똑똑할까?";
            QuestManager._inst._currentMission = 0;
            QuestManager._inst.CheckQuestTxt();

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(false);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(false);
        }
        else if (collision.gameObject.name == "Bed" && QuestManager._inst.finalStory[4] == 0)
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "거북이의 침대라니! 자고 갈까?";
            QuestManager._inst._currentMission = 1;
            QuestManager._inst.CheckQuestTxt();

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(true);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(true);
        }
        else if (collision.gameObject.name == "TV" && QuestManager._inst.finalStory[5] == 0)
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "이 티비를 가져가면 지갑이 두둑해지겠어!";
            QuestManager._inst._currentMission = 2;
            QuestManager._inst.CheckQuestTxt();

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(true);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(true);
        }
        else if (collision.gameObject.name == "Fridge" && QuestManager._inst.finalStory[6] == 0)
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "흠... 배가 고프군. 냉장고를 열어 음식을 확인해볼까?";
            QuestManager._inst._currentMission = 3;
            QuestManager._inst.CheckQuestTxt();

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(true);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(true);
        }
        else if (collision.gameObject.name == "Sink")
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "거울 속 내 모습은 완전히 도둑이네...\n좀 부끄러운걸...";
            QuestManager._inst._currentMission = 4;
            QuestManager._inst.CheckQuestTxt();

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(false);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(false);

            GameObject.Find("Canvas").transform.Find("FinalStory").gameObject.SetActive(true);
        }
        else if (collision.gameObject.name == "Beth" && QuestManager._inst.finalStory[7] == 0)
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "목욕이라도 하고 갈까?";
            QuestManager._inst._currentMission = 5;
            QuestManager._inst.CheckQuestTxt();

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(true);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(true);
        }
        else if (collision.gameObject.name == "Toilet")
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "변기까지 깔끔하군!";
            QuestManager._inst._currentMission = 6;

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(false);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(false);
        }
        else if (collision.gameObject.name == "Laptop" && QuestManager._inst.finalStory[8] == 0)
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "값비싸보이는 노트북이다. 가져갈까?";
            QuestManager._inst._currentMission = 7;
            QuestManager._inst.CheckQuestTxt();

            QuestManager._inst.QuestText.transform.Find("btnYes").gameObject.SetActive(true);
            QuestManager._inst.QuestText.transform.Find("btnNo").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bookcase")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.name == "Bed")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.name == "TV")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.name == "Fridge")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.name == "Sink")
        {
            QuestManager._inst.CheckQuestTxt();

            GameObject.Find("Canvas").transform.Find("FinalStory").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.SetActive(false);
        }
        else if (collision.gameObject.name == "Beth")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.name == "Toilet")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.name == "Laptop")
        {
            QuestManager._inst.CheckQuestTxt();
        }
    }

    protected override void Moving()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
            _animator.SetTrigger("walk");
            transform.position += Vector3.left * Time.deltaTime * _speed;

            _isJump = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
            _animator.SetTrigger("walk");
            transform.position += Vector3.right * Time.deltaTime * _speed;

            _isJump = false;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
            _animator.SetTrigger("walk");
            transform.position += Vector3.up * Time.deltaTime * _speed;

            _isJump = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
            _animator.SetTrigger("walk");
            transform.position += Vector3.down * Time.deltaTime * _speed;

            _isJump = false;
        }
        else if (!_canGoNext && Input.GetKey(KeyCode.Space) && shootCoolTime > 1f)
        {
            _audioClip = Resources.Load<AudioClip>("Sound/Attack");
            _audioSource.clip = _audioClip;
            _audioSource.Play();

            Shoot();
            _isJump = false;
            shootCoolTime = 0.0f;
        }
        else if (Input.GetKey(KeyCode.E) && carrotCoolTime > 2.5f)
        {
            GameObject item = Resources.Load<GameObject>("Prefabs/Carrot");
            Instantiate(item, new Vector3(transform.position.x, transform.position.y - 0.3f, 0), new Quaternion(0, 0, 0, 0));

            carrotCoolTime = 0.0f;
        }
        else if (_canGoNext && Input.GetKey(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "Map3")
                SceneManager.LoadScene("Map2");
        }
        else
        {
            Dir = MoveDir.None;
            _animator.SetTrigger("idle");
        }
    }
}
