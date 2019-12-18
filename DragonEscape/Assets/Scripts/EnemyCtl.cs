//////////////////////////////
//製作者  ：田口　未来
//制作日時：2019/12/16
//更新日時：2019/12/17
/////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtl : MonoBehaviour
{
    private enum Patrol
    {
        FIRST,
        SECOND,
        THIRD,
        END
    }


    [SerializeField]
    public int HP = 0;
    [SerializeField]
    private float _hightVelocity = 0;        // プレイヤー追従速度
    [SerializeField]
    private float _velocity = 0;             // 敵の移動速度
    [SerializeField]
    private float _lowVelocity = 0;          // 敵の移動低下時の速度

    private GameObject _player = null;       // プレイヤーオブジェクト
    private NavMeshAgent _navmesh;
    private bool collisionFlag = false;

    private List<GameObject> pointList = new List<GameObject>();
    private Patrol patrol = Patrol.FIRST;
    private GameObject checkPL = null;
    private PLEnterCheck _pLEnterCheck;
    private bool smokeFlag = false;
    //private float smokeTime = 5;
    //private float deltime = 0;
    private bool onceFlag = true;

    // 初期化
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _navmesh = GetComponent<NavMeshAgent>();
        _navmesh.speed = _velocity;
    }

    // 更新処理
    private void Update()
    {
        if (_pLEnterCheck == null)
        {
            return;
        }
        else
        {
            if (onceFlag)
            {
                _pLEnterCheck.PointSort();
                for (int j = 0; j < _pLEnterCheck.GetPointList().Count; j++)
                {
                    pointList.Add(_pLEnterCheck.GetPointList()[j]);
                }
                onceFlag = false;
            }
        }

        if (_pLEnterCheck.GetEnterFlag())
        {
            if (!smokeFlag)
            {
                //プレイヤー追従設定
                _navmesh.acceleration = 5;
                _navmesh.speed = _hightVelocity;
                _navmesh.destination = _player.transform.position;
            }
            else
            {
                //deltime += Time.deltaTime;
                //if (deltime > smokeTime)
                //{
                //    deltime = 0;
                //    smokeFlag = false;
                //}
            }
        }
        else
        {
            _navmesh.acceleration = 8;
            _navmesh.speed = _velocity;
            _navmesh.destination = pointList[(int)patrol].transform.position;
        }
    }

    //障害物に当たったら速度を低下させる
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Money")
        {
            _navmesh.speed = _lowVelocity;
        }
        if(other.tag == "Point")
        {
            collisionFlag = true;
        }
        if(other.tag == "Smoke")
        {
            _navmesh.speed = 0;
            _navmesh.acceleration = 10;
            smokeFlag = true;
        }

        //次どの地点を目指すか決める
        if(collisionFlag)
        {
            //順に切り替えていく
            if (patrol == Patrol.FIRST)
            {
                patrol = Patrol.SECOND;
            }
            else if (patrol == Patrol.SECOND)
            {
                patrol = Patrol.THIRD;
            }
            else if(patrol == Patrol.THIRD)
            {
                if (pointList.Count > 3)
                {
                    patrol = Patrol.END;
                }
                else
                {
                    patrol = Patrol.FIRST;
                }
            }
            else if (patrol == Patrol.END)
            {
                patrol = Patrol.FIRST;
            }
            collisionFlag = false;
        }
    }

    //障害物から逃れたら速度を元に戻す
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Money")
        {
            _navmesh.speed = _velocity;
        }
    }

    //巡回フィールド範囲BOXを取得
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Wall")
        {
            checkPL = other.transform.gameObject;
            _pLEnterCheck = checkPL.GetComponent<PLEnterCheck>();
        }
    }

    //HP情報取得
    public int GetHP()
    {
        return HP;
    }

    //HP情報書き換え
    public void SetHP(int hp)
    {
        HP = hp;
    }

    //プレイヤーを追いかけるかどうか判断するフラグの取得関数
    public bool GetFollowFlag()
    {
        return _pLEnterCheck.GetEnterFlag();
    }
}
