using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshGameManager : MonoBehaviour
{
    public static cshGameManager _inst;
    public GameObject _player;
    private GameObject _eagle;
    public GameObject _arrow;
    public int _attackPower = 10;

    AudioSource _audioSource;
    AudioClip _audioClip;

    Dictionary<string, Vector3> _eagleSpawnPositions = new Dictionary<string, Vector3>();

    private void Awake()
    {
        if (_inst != this && _inst != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _inst = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        _arrow = Resources.Load<GameObject>("Prefabs/Arrow");
        _audioSource = GetComponent<AudioSource>();
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
            _player = GameObject.Find("Player").gameObject;
            _eagle = Resources.Load<GameObject>("Prefabs/Eagle");
            _player.transform.position = new Vector3(0, 3, 0);

            _eagleSpawnPositions.Add("Eagle1", new Vector3(21, 5.5f, 0));
            _eagleSpawnPositions.Add("Eagle2", new Vector3(50, -0.5f, 0));
            _eagleSpawnPositions.Add("Eagle3", new Vector3(37, -0.5f, 0));
            _eagleSpawnPositions.Add("Eagle4", new Vector3(67, 3.5f, 0));
            _eagleSpawnPositions.Add("Eagle5", new Vector3(133, 17.5f, 0));
            _eagleSpawnPositions.Add("Eagle6", new Vector3(139, -2.5f, 0));

            foreach (var entry in _eagleSpawnPositions)
            {
                StartCoroutine(SpawnMonsterAfterDelay(0f, entry.Value, entry.Key));
            }
        }
        else if (SceneManager.GetActiveScene().name == "Map2" && !QuestManager._inst.iAmTheft)
        {
            _player = GameObject.Find("Player").gameObject;
            _player.transform.position = GameObject.Find("Portal").transform.position;
        }
        else if (SceneManager.GetActiveScene().name == "Map2" && QuestManager._inst.iAmTheft)
        {
            _player = GameObject.Find("Player").gameObject;
            _player.transform.position = new Vector3(21, 4, 0);
        }
        else if (SceneManager.GetActiveScene().name == "Map3")
        {
            _player = GameObject.Find("littlePlayer").gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (_player == null) return;
        Camera.main.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);
    }

    public IEnumerator SpawnMonsterAfterDelay(float delay, Vector3 position, string name = "Eagle")
    {
        yield return new WaitForSeconds(delay);
        GameObject go = Instantiate(_eagle, position, Quaternion.identity);
        go.name = name;
    }

    public Vector3 GetSpawnPositionByName(string eagleName)
    {
        if (_eagleSpawnPositions.ContainsKey(eagleName))
            return _eagleSpawnPositions[eagleName];
        else
            return Vector3.zero;
    }

    public void ButtonClick()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/Click");
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }

    public void ButtonFailed()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/ClickFail");
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }
}