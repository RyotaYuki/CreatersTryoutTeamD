using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMMSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject gmm;
    int count = 0;
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
        if (other.tag == "Money")
        {
            if (count <= 5)
            {
                
                GameObject tempgmm = Instantiate(gmm, transform.position,transform.rotation);
                tempgmm.GetComponent<GiveMoneyman>().SetTObj(other.gameObject);
                count++;
            }
        }
    }
}
