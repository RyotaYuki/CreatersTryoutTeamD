using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMoneyman : MonoBehaviour
{

    private GameObject _targetObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetObj)
        {
            Vector3 distance = _targetObj.transform.position - transform.position;
            Vector3 vec = distance.normalized;
            transform.position += vec * 10 * Time.deltaTime;
        }



    }

    public void SetTObj(GameObject target)
    {
        _targetObj = target;
    }
}
