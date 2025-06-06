using UnityEngine;
using static Define;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasePlayerController : cshPlayerController
{
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
            {
            }
            if (SceneManager.GetActiveScene().name == "Map2")
                SceneManager.LoadScene("Map1");
        }
        else
        {
            Dir = MoveDir.None;
            _animator.SetTrigger("idle");
        }
    }
}
