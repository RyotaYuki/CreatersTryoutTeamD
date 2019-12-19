//////////////////////////////
//製作者  ：田口　未来
//制作日時：2019/12/16
//更新日時：2019/12/19
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
    public int _hP = 0;
    [SerializeField]
    private float _hightVelocity = 0;        // プレイヤー追従速度
    [SerializeField]
    private float _velocity = 0;             // 敵の移動速度
    [SerializeField]
    private float _lowVelocity = 0;          // 敵の移動低下時の速度

    private GameObject _player;              // プレイヤーオブジェクト

    private NavMeshAgent _navmesh;
    private bool _pointColFlag = false;
    private Patrol _patrol;
    private List<GameObject> _pointList;
    private PLEnterCheck _pLEnterCheck;

    private bool smokeFlag = false;
    //private float smokeTime = 5;
    //private float deltime = 0;
    private bool onceFlag = true;

    private float _displaySpeed = 0.1f;     // 表示速度
    private float _disappearSpeed = 0.1f;   // 非表示速度

    private float _displayTimeSec = 1f;     // 表示時間
    private float _timeFlame;

    private float _maxScale = 0.1f;
    private float _minScale = 0;
    private List<GameObject> _speechList;
    private bool _speechPopFlag;    
    //private SpriteEffectMng _spriteEffectMng;

    //タグ名
    private string money  = "Money";
    private string wall   = "Wall";
    private string point  = "Point";
    private string smoke  = "Smoke";
    private string speech = "Speech";

    // 初期化
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _navmesh = GetComponent<NavMeshAgent>();
        _navmesh.speed = _velocity;
        _patrol = Patrol.FIRST;
        _pointList = new List<GameObject>();
        _speechList = new List<GameObject>();
        _speechPopFlag = false;
        _timeFlame = 0;
        //_spriteEffectMng = GameObject.FindGameObjectWithTag("SpriteEffectMng").GetComponent<SpriteEffectMng>();

        for (int cnt = 0; cnt < this.transform.childCount; cnt++)
        {
            if(this.transform.GetChild(cnt).gameObject.tag == speech)
            {
                _speechList.Add(this.transform.GetChild(cnt).gameObject);
            }
        }
    }

    // 更新処理
    private void Update()
    {
        if(!CheckPatrolBox())
        {
            return;
        }
        if (_pLEnterCheck.GetEnterFlag())
        {
            FollowPlayer();
        }
        else
        {
            PatrolMove();
        }
    }

    //障害物に当たったら速度を低下させる
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == money)
        {
            _navmesh.speed = _lowVelocity;
        }
        if(other.tag == point)
        {
            _pointColFlag = true;
        }
        if(other.tag == smoke)
        {
            _navmesh.speed = 0;
            _navmesh.acceleration = 10;
            smokeFlag = true;
        }

        //次どの地点を目指すか決める
        if(_pointColFlag)
        {
            //順に切り替えていく
            _patrol++;
            if (_pointList.Count - 1 < (int)_patrol)
            {
                _patrol = Patrol.FIRST;
            }

            _pointColFlag = false;
        }
    }

    //プレイヤーを追従する
    private void FollowPlayer()
    {
        //smokeが焚かれていたら動かないようにする
        if (smokeFlag)
        {
            //deltime += Time.deltaTime;
            //if (deltime > smokeTime)
            //{
            //    deltime = 0;
            //    smokeFlag = false;
            //}
        }
        else
        {
            _timeFlame += Time.deltaTime;
            //プレイヤー追従設定
            _navmesh.acceleration = 5;
            _navmesh.speed = _hightVelocity;
            _navmesh.destination = _player.transform.position;

            if(_speechList.Count == 0)
            {
                return;
            }
            //怒鳴り声のエフェクト
            if ((_displayTimeSec + _displaySpeed) > _timeFlame)
            {
                //_spriteEffectMng.Expansion(_speechList[0].gameObject, _maxScale, _displaySpeed * 60);
            }
            else
            {
                //_spriteEffectMng.Shrink(_speechList[0].gameObject, _minScale, _displaySpeed * 60);
                if ((_displayTimeSec + _displaySpeed + _disappearSpeed) < _timeFlame)
                {
                    _timeFlame = 0;
                }
            }
        }
    }

    //パトロールする
    private void PatrolMove()
    {
        _navmesh.acceleration = 8;
        _navmesh.speed = _velocity;
        _navmesh.destination = _pointList[(int)_patrol].transform.position;
    }

    //巡回範囲を取得、取得後巡回ポイントリストに追加する
    private bool CheckPatrolBox()
    {
        if (_pLEnterCheck == null)
        {
            return false;
        }
        else
        {
            if (onceFlag)
            {
                _pLEnterCheck.PointSort();
                for (int j = 0; j < _pLEnterCheck.GetPointList().Count; j++)
                {
                    _pointList.Add(_pLEnterCheck.GetPointList()[j]);
                }
                onceFlag = false;
            }
            return true;
        }
    }

    //障害物から逃れたら速度を元に戻す
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == money)
        {
            _navmesh.speed = _velocity;
        }
    }

    //巡回フィールド範囲BOXを取得
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == wall)
        {
            _pLEnterCheck = other.transform.gameObject.GetComponent<PLEnterCheck>();
        }
    }


    //HP情報取得
    public int GetHP()
    {
        return _hP;
    }

    //HP情報書き換え
    public void SetHP(int hp)
    {
        _hP = hp;
    }

    //プレイヤーを追いかけるかどうか判断するフラグの取得関数
    public bool GetFollowFlag()
    {
        return _pLEnterCheck.GetEnterFlag();
    }
}
