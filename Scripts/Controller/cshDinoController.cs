using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class cshDinoController : MonoBehaviour
{
    public int _hp;
    protected Animator _animator;
    AudioSource _audioSource;
    AudioClip _audioClip;

    public int Hp
    {
        get => _hp;
        private set => _hp = Mathf.Clamp(value, 0, _hp);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hp = 100;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public virtual void Hurt()
    {
        //_hp -= cshPlayerController._inst._attackPower;
        _hp -= 100;

        if (_hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/Die");
        _audioSource.clip = _audioClip;
        _audioSource.Play();

        _animator.SetTrigger("die");
        Destroy(gameObject, 0.8f);

        QuestManager._inst.MissionComplete();
    }
}
