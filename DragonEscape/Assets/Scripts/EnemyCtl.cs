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
    [SerializeField]
    public int HP = 0;
    [SerializeField]
    private float velocity = 0;             //敵の移動速度
    [SerializeField]
    private float lowVelocity = 0;          //敵の移動低下時の速度
    private GameObject player = null;       //プレイヤーオブジェクト
    private NavMeshAgent navmesh = null;

    //初期化
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navmesh = GetComponent<NavMeshAgent>();
        navmesh.speed = velocity;
    }

    //更新処理
    private void Update()
    {
        if (navmesh != null)
        {
            navmesh.destination = player.transform.position;
        }
    }

    //障害物に当たったら速度を低下させる
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "money")
        {
            navmesh.speed = lowVelocity;
        }
    }
    //障害物から逃れたら速度を元に戻す
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "money")
        {
            navmesh.speed = velocity;
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
}
