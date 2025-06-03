using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class cshMonsterController : MonoBehaviour
{
    public virtual float Speed { get; set; }
    protected int _changeDir;
    public int _hp;
    bool _isDead = false;

    protected Animator _animator;
    FinalDir _finalDir;

    public GameObject prfHpBar;
    public GameObject canvas;
    RectTransform hpBar;
    public Slider _sliderHpBar;
    public float height = 1.5f;

    protected virtual FinalDir FinalDir
    {
        get { return _finalDir; }
        set
        {
            if (_finalDir == value)
                return;

            switch (value)
            {
                case FinalDir.Left:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                case FinalDir.Right:
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    break;
                case FinalDir.None:
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
            }
            _finalDir = value;
        }
    }

    public int Hp
    {
        get => _hp;
        private set => _hp = Mathf.Clamp(value, 0, _hp);
    }

    void Start()
    {
        Init();

        canvas = GameObject.Find("Canvas");
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        hpBar.name = gameObject.name + "_hpBar";
        _sliderHpBar = hpBar.GetComponent<Slider>();

        _hp = 100;
        _sliderHpBar.maxValue = 100;
        _sliderHpBar.value = 100;
    }

    void FixedUpdate()
    {
        Move();
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        hpBar.position = _hpBarPos;
    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
        FinalDir = FinalDir.Left;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Attack();
        }
    }

    public virtual void Attack()
    {
        _animator.SetBool("attack", true);

        cshPlayerController._inst.Hurt();
    }

    public virtual void Hurt()
    {
        if (_hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        cshGameManager._inst._badthings++;
        Debug.Log("The bad things I’ve done : " + cshGameManager._inst._badthings);
        FinalDir = FinalDir.None;
        _isDead = true;
    }

    protected virtual void Move()
    {
        _animator.SetBool("walk", false);
        _animator.SetBool("hurt", false);
        _animator.SetBool("attack", false);

        if (!_isDead)
        {
            if (_changeDir == 0)
            {
                _animator.SetBool("walk", true);
                transform.position += Vector3.right * Speed * Time.deltaTime;
                FinalDir = FinalDir.Right;
            }
            else if (_changeDir == 1)
            {
                _animator.SetBool("walk", true);
                transform.position += Vector3.left * Speed * Time.deltaTime;
                FinalDir = FinalDir.Left;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    protected virtual IEnumerator FromRight(float time)
    {
        _changeDir = 1;
        yield return new WaitForSeconds(time);
        _changeDir = 0;
        yield return new WaitForSeconds(time);
        StartCoroutine("FromRight", time);
    }

    protected virtual IEnumerator FromLeft(float time)
    {
        _changeDir = 0;
        yield return new WaitForSeconds(time);
        _changeDir = 1;
        yield return new WaitForSeconds(time);
        StartCoroutine("FromLeft", time);
    }
}