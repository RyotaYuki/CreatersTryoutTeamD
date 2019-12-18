﻿//////////////////////////////
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
    private float _hightVelocity = 0;
    [SerializeField]
    private float _velocity = 0;             // 敵の移動速度
    [SerializeField]
    private float _lowVelocity = 0;          // 敵の移動低下時の速度
    private GameObject _player = null;       // プレイヤーオブジェクト
    private NavMeshAgent _navmesh;
    private bool followFlag = false;
    private bool collisionFlag = false;
    private Rigidbody _rb;
    [SerializeField]
    private GameObject[] points = null;             // 巡回時の目標地点
    private List<GameObject> pointList = new List<GameObject>();
    private Patrol patrol = Patrol.FIRST;
    [SerializeField]
    private GameObject checkPL = null;
    private PLEnterCheck _pLEnterCheck;
    private bool smokeFlag = false;
    //private float smokeTime = 5;
    //private float deltime = 0;

    // 初期化
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _pLEnterCheck = checkPL.GetComponent<PLEnterCheck>();
        _navmesh = GetComponent<NavMeshAgent>();
        _navmesh.speed = _velocity;
        _rb = this.transform.GetComponent<Rigidbody>();
        for(int j= 0; j < points.Length; j++)
        {
            pointList.Add(points[j]);
        }
    }

    // 更新処理
    private void Update()
    {
        if (_pLEnterCheck.GetEnterFlag())
        {
            followFlag = true;
        }
        else
        {
            followFlag = false;
        }

        if (followFlag)
        {
            if (!smokeFlag)
            {
                _navmesh.acceleration = 5;
                _navmesh.speed = _hightVelocity;
                // 位置の更新
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
            if (patrol == Patrol.FIRST)
            {
                _navmesh.destination = pointList[(int)Patrol.FIRST].transform.position;
            }
            if (patrol == Patrol.SECOND)
            {
                _navmesh.destination = pointList[(int)Patrol.SECOND].transform.position;
            }
            if (patrol == Patrol.THIRD)
            {
                _navmesh.destination = pointList[(int)Patrol.THIRD].transform.position;
            }
            if (patrol == Patrol.END)
            {
                _navmesh.destination = pointList[(int)Patrol.END].transform.position;
            }
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
                if (points.Length > 3)
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
        return followFlag;
    }
}
