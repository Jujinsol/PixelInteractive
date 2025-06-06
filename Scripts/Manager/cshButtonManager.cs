using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cshButtonManager : MonoBehaviour
{
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name == "Opening")
        {
            GameObject.Find("Canvas").transform.Find("btnStart").gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                cshGameManager._inst.ButtonClick();
                SceneManager.LoadScene("Map1");
            });
        }
        else if (SceneManager.GetActiveScene().name == "Map1")
        {
            GameObject.Find("Canvas").transform.Find("GameOverBackground").Find("btnRetry").gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                cshGameManager._inst.ButtonClick();
                SceneManager.LoadScene("Opening");
            });
        }
        else if (SceneManager.GetActiveScene().name == "Map2")
        {
            GameObject.Find("Canvas").transform.Find("GameOverBackground").Find("btnRetry").gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                cshGameManager._inst.ButtonClick();
                SceneManager.LoadScene("Opening");
            });
        }
    }
}
