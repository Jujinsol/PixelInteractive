using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 4.0f;
    public GameObject _monster;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Turtle")
        {
            _monster = collision.gameObject;
            _monster.GetComponent<cshDinoController>().Hurt();
        }
        else if (collision.gameObject.tag == "Monster")
        {
            _monster = collision.gameObject;
            _monster.GetComponent<EagleController>().Hurt();
            Destroy(gameObject);
        }
    }
}
