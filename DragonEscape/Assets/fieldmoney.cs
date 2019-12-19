using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldmoney : MonoBehaviour
{

    [SerializeField]
    private GameObject crashEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Instantiate(crashEffect, transform.position, Quaternion.identity);
            other.GetComponent<Player>().AddMoney(10);
            Destroy(gameObject);
        }
    }
}
