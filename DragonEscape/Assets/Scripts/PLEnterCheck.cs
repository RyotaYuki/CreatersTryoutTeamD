using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLEnterCheck : MonoBehaviour
{
    private bool enterFlag = false;         //プレイヤーが巡回エリア内に入ったかどうかのフラグ

    private int targetMax = 4;              //ターゲット数の上限
    private int addPointCnt;
    private List<GameObject> pointList;     //巡回時のターゲットポイントリスト

    private void Start()
    {
        addPointCnt = 0;
        pointList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enterFlag = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enterFlag = false;
        }
    }

    public void PointSort()
    {
        //データを入れる
        for(int child = 0; child < transform.childCount; child++)
        {
            pointList.Add(transform.GetChild(child).gameObject);
        }

        //バブルソート(昇順)でリスト内のデータを並び替える
        int cnt = 0;
        for(int j = 0; j < pointList.Count; j++)
        {
            for (int k = 1; k < pointList.Count - j; k++)
            {
                cnt++;
                if(int.Parse(pointList[k].name) < int.Parse(pointList[k - 1].name))
                {
                    var tmp = pointList[k];
                    pointList[k] = pointList[k - 1];
                    pointList[k - 1] = tmp;
                }
            }
        }
    }

    public bool GetEnterFlag()
    {
        return enterFlag;
    }

    public List<GameObject> GetPointList()
    {
        return pointList;
    }
}