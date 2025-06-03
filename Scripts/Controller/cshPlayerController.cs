using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using static UnityEditor.Progress;

public class cshPlayerController : MonoBehaviour
{
    public static cshPlayerController _inst;

    float shootCoolTime, carrotCoolTime, _speed = 7.0f, _jumpPower = 10.0f;
    bool _isGround, _isJump = false, _canGoNext = false;
    public int _hp = 100;

    public GameObject _BowCoolTime,_CarrotCoolTime, _playerHPBar;
    Slider _BowCoolTimeSilder, _CarrotCoolTimeSlider, _playerHPSlider;
    public GameObject _monster;
    public Transform _spawnPoint;

    GameObject gameOverUI;
    Animator _animator;
    AudioSource _audioSource;
    AudioClip _audioClip;

    FinalDir _finalDir;
    MoveDir _dir;

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

        Init();
    }

    void FixedUpdate()
    {
        shootCoolTime += Time.deltaTime;
        carrotCoolTime += Time.deltaTime;
        _BowCoolTimeSilder.value = Mathf.Clamp01(1f - shootCoolTime);
        _CarrotCoolTimeSlider.value = Mathf.Clamp01(1 - (carrotCoolTime / 2.5f));
        Moving();
    }

    protected void Init()
    {
        Vector3 pos = new Vector3(0, 3, 0);
        transform.position = pos;
        Dir = MoveDir.Right;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            _isGround = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            QuestManager._inst.CheckQuestTxt();
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
            _canGoNext = true;
        }
        else if (collision.gameObject.tag == "Door" && QuestManager._inst._acceptQuest)
        {
            QuestManager._inst.TheftQuest.SetActive(true);
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
            _canGoNext = false;
        }
        else if (collision.gameObject.tag == "Door")
        {
            QuestManager._inst.TheftQuest.SetActive(false);
            QuestManager._inst._theftColor = "";
            QuestManager._inst.SliderTheft.value = 0;
        }
    }

    protected virtual void Moving()
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
        else if (Input.GetKey(KeyCode.W) && _isGround && !_isJump)
        {
            _audioClip = Resources.Load<AudioClip>("Sound/Jump");
            _audioSource.clip = _audioClip;
            _audioSource.Play();

            Dir = MoveDir.Up;
            _animator.SetTrigger("jump");
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

            _isJump = true;
            _isGround = false;
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
            if (SceneManager.GetActiveScene().name == "Map1")
                SceneManager.LoadScene("Map2");
            if (SceneManager.GetActiveScene().name == "Map2")
                SceneManager.LoadScene("Map1");
        }
        else
        {
            Dir = MoveDir.None;
            _animator.SetTrigger("idle");
        }
    }

    void Shoot()
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

        _hp -= 50;
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

    void Cure()
    {
        _hp += 10;
        if (_hp >= 100) _hp = 100;
        _playerHPSlider.value = _hp;
    }
}