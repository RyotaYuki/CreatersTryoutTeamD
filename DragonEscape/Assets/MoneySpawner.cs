using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    //ランダム用
    [SerializeField]
    private GameObject[] boxPlaces;
    [SerializeField]
    private bool generatemode;
    [SerializeField]
    private GameObject moneyObj;

    // Start is called before the first frame update
    void Start()
    {
        if (generatemode)
        {
            int[] spawned = new int[7];
            for(int i = 0;i < 7; i++)
            {
                int r = Random.Range(0, 20);
                while (spawned[i] != r) {

                    boxPlaces[r].SetActive(true);
                    spawned[i] = r;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
