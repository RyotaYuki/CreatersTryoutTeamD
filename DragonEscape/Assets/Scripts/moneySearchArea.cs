using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moneySearchArea : MonoBehaviour
{
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
        if(other.tag == "Money")
        {
            transform.GetComponentInParent<GiveMoneyman>().SetTObj(other.transform.gameObject);
        }
    }
}
