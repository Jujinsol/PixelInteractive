using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static Define;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EagleController : cshMonsterController
{
    float _speed = 4.0f;
    GameObject item;
    int rand;

    public override float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    protected override FinalDir FinalDir
    {
        get => base.FinalDir;
        set => base.FinalDir = value;
    }

    protected override void Init()
    {
        _speed = 4.0f;
        base.Init();
        StartCoroutine("FromRight", 1.5f);
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override IEnumerator FromRight(float time)
    {
        return base.FromRight(time);
    }

    public override void Hurt()
    {
        _hp -= cshGameManager._inst._attackPower;
        _sliderHpBar.value = _hp;

        _animator.SetBool("walk", false);
        _animator.SetBool("hurt", true);

        base.Hurt();
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetBool("die", true);
        rand = Random.Range(0, 2);
        //if (rand == 1) item = Resources.Load<GameObject>("Prefabs/Diamond");
        //else item = Resources.Load<GameObject>("Prefabs/Cherry");
        item = Resources.Load<GameObject>("Prefabs/Diamond");
        GameObject go = Instantiate(item, new Vector3(transform.position.x, transform.position.y-1, 0), new Quaternion(0, 0, 0, 0));
        Destroy(gameObject, 0.8f);
        Destroy(GameObject.Find(gameObject.name + "_hpBar"), 1.0f);

        Vector3 respawnPos = cshGameManager._inst.GetSpawnPositionByName(gameObject.name);
        cshGameManager._inst.StartCoroutine(cshGameManager._inst.SpawnMonsterAfterDelay(5f, respawnPos, gameObject.name));

        cshPlayerController._inst._monster = null;
    }
}