using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using static UnityEditor.Progress;
using TMPro;

public class cshPlayerController : MonoBehaviour
{
    public static cshPlayerController _inst;

    protected float shootCoolTime, carrotCoolTime, _speed = 7.0f, _jumpPower = 10.0f;
    protected bool _isGround, _isJump = false, _canGoNext = false;
    public int _hp = 100;

    public GameObject _BowCoolTime,_CarrotCoolTime, _playerHPBar;
    Slider _BowCoolTimeSilder, _CarrotCoolTimeSlider, _playerHPSlider;
    public GameObject _monster;
    public Transform _spawnPoint;

    GameObject gameOverUI;
    protected Animator _animator;
    protected AudioSource _audioSource;
    protected AudioClip _audioClip;

    protected FinalDir _finalDir;
    protected MoveDir _dir;

    public MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
                return;

            switch (value)
            {
                case MoveDir.Left:
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    _finalDir = FinalDir.Left;
                    break;
                case MoveDir.Right:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    _finalDir = FinalDir.Right;
                    break;
                //case MoveDir.Up:
                //    _animator.Play("JUMP");
                //    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //    break;
                case MoveDir.None:
                    if (_finalDir == FinalDir.Left)
                    {
                        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    else if (_finalDir == FinalDir.Right)
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    break;
            }
            _dir = value;
        }
    }

    void Start()
    {
        //DontDestroyOnLoad(this);

        _inst = this;
        Dir = MoveDir.Right;
        shootCoolTime = 0.0f;
        carrotCoolTime = 0.0f;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        gameOverUI = GameObject.Find("Canvas").transform.Find("GameOverBackground").gameObject;
        gameOverUI.SetActive(false);

        _BowCoolTime = GameObject.Find("Canvas").transform.Find("BowCoolTime").gameObject;
        _BowCoolTimeSilder = _BowCoolTime.GetComponent<Slider>();
        _BowCoolTimeSilder.maxValue = 1f;
        _BowCoolTimeSilder.value = 0;

        _CarrotCoolTime = GameObject.Find("Canvas").transform.Find("CarrotCoolTime").gameObject;
        _CarrotCoolTimeSlider = _CarrotCoolTime.GetComponent<Slider>();
        _CarrotCoolTimeSlider.maxValue = 1f;
        _CarrotCoolTimeSlider.value = 0;

        _playerHPBar = GameObject.Find("Canvas").transform.Find("PlayerHPBarBackground").gameObject;
        _playerHPSlider = _playerHPBar.GetComponent<Slider>();
        _playerHPSlider.maxValue = 100;
        _playerHPSlider.value = 100;
    }

    void FixedUpdate()
    {
        shootCoolTime += Time.deltaTime;
        carrotCoolTime += Time.deltaTime;
        _BowCoolTimeSilder.value = Mathf.Clamp01(1f - shootCoolTime);
        _CarrotCoolTimeSlider.value = Mathf.Clamp01(1 - (carrotCoolTime / 2.5f));
        Moving();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            _isGround = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "NPC")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.name == "Rabbit")
        {
            QuestManager._inst.QuestText.SetActive(true);
            QuestManager._inst.QuestText.gameObject.transform.Find("btnYes").gameObject.SetActive(true);
            QuestManager._inst.QuestText.gameObject.transform.Find("btnNo").gameObject.SetActive(true);
        }
        else if (collision.gameObject.tag == "Diamond")
        {
            _audioClip = Resources.Load<AudioClip>("Sound/Item");
            _audioSource.clip = _audioClip;
            _audioSource.Play();

            QuestManager._inst.AddDiamond();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Cherry")
        {
            _audioClip = Resources.Load<AudioClip>("Sound/Item");
            _audioSource.clip = _audioClip;
            _audioSource.Play();

            Cure();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Portal")
        {
            collision.GetComponent<SpriteOutline>().enabled = true;
            if (SceneManager.GetActiveScene().name == "Map1" && GameObject.Find("Canvas")?.transform.Find("OpenBook")?.gameObject != null)
            {
                GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("txtOpenBook1").GetComponent<TextMeshProUGUI>().text = "토끼와 거북이의 열렬한\n달리기 대결 이후, \n토끼는 자신을 두고\n홀로 완주한 이기적인\n거북에게 크게 분노했다.";
                GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("txtOpenBook2").GetComponent<TextMeshProUGUI>().text = "옆 동네로 가서 이들의\n이야기를 자세히 들어볼까?";

                GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("btnYes").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("btnNo").gameObject.SetActive(true);
            }
            _canGoNext = true;
        }
        else if (collision.gameObject.tag == "Door" && QuestManager._inst._acceptQuest)
        {
            QuestManager._inst.TheftQuest.SetActive(true);
        }
        else if (collision.gameObject.name == "Sign")
        {
            GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("txtOpenBook1").GetComponent<TextMeshProUGUI>().text = "<마을 공지>\n비겁한 박쥐는 새들을\n배신한 배신자입니다.\n박쥐 때문에 우리는\n굶고 있습니다.";
            GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("txtOpenBook2").GetComponent<TextMeshProUGUI>().text = "누구도 박쥐에게\n먹을 것을 주지 마세요.\n우리는 당근까지 씹어먹고 있다고요!";
            GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("btnYes").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.transform.Find("btnNo").gameObject.SetActive(false);
        }
        else if (collision.gameObject.name == "Turtle")
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "흥! 저 토끼는 패배를 인정하지 않는군!";
            QuestManager._inst.QuestText.SetActive(true);
            QuestManager._inst.QuestText.gameObject.transform.Find("btnYes").gameObject.SetActive(false);
            QuestManager._inst.QuestText.gameObject.transform.Find("btnNo").gameObject.SetActive(false);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            QuestManager._inst.CheckQuestTxt();
        }
        else if (collision.gameObject.tag == "Portal")
        {
            collision.GetComponent<SpriteOutline>().enabled = false;
            if (SceneManager.GetActiveScene().name == "Map1" && GameObject.Find("Canvas")?.transform.Find("OpenBook")?.gameObject != null)
                GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.SetActive(false);
            _canGoNext = false;
        }
        else if (collision.gameObject.tag == "Door")
        {
            QuestManager._inst.TheftQuest.SetActive(false);
            QuestManager._inst._theftColor = "";
            QuestManager._inst.SliderTheft.value = 0;
        }
        else if (collision.gameObject.name == "Sign")
        {
            Debug.Log("Sign OUT");
            GameObject.Find("Canvas").transform.Find("OpenBook").gameObject.SetActive(false);
        }
        else if (collision.gameObject.name == "Turtle")
        {
            QuestManager._inst.QuestText.transform.Find("txtQuest").GetComponent<TextMeshProUGUI>().text = "저 거북이 녀석이 내 1등을 훔쳐갔어\n지금이라면 복수할 수 있을 것 같아!";
            QuestManager._inst.QuestText.SetActive(false);
        }
    }

    protected virtual void Moving() { }

    protected void Shoot()
    {
        _animator.SetTrigger("attack");
        GameObject newArrow;
        if (_finalDir == FinalDir.Right)
            newArrow = Instantiate(cshGameManager._inst._arrow, _spawnPoint.position, new UnityEngine.Quaternion(0, 0, 0, 0));
        else
        {
            newArrow = Instantiate(cshGameManager._inst._arrow, _spawnPoint.position, new UnityEngine.Quaternion(0, 0, 0, 0));
            newArrow.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);            
        }
        newArrow.GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x,0,0) * 1000.0f);
    }

    public void Hurt()
    {
        _animator.SetTrigger("hurt");

        _hp -= 30;
        if (_hp <= 0) Die();
        _playerHPSlider.value = _hp;
    }

    void Die()
    {
        Debug.Log("Player Die");

        _audioClip = Resources.Load<AudioClip>("Sound/Death");
        _audioSource.clip = _audioClip;
        _audioSource.Play();

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Cure()
    {
        _hp += 10;
        if (_hp >= 100) _hp = 100;
        _playerHPSlider.value = _hp;
    }
}