using UnityEngine;

public class cshHouseController : MonoBehaviour
{
    public static cshHouseController _inst;
    AudioSource _audioSource;
    AudioClip _audioClip;

    private void Start()
    {
        _inst = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void DoorUnlock()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/Item");
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}
