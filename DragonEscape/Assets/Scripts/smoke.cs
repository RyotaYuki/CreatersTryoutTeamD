using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke : MonoBehaviour
{
    private float _count = 0;
    [SerializeField]
    private float _destroytime = 3;
   
    void Update()
    {
        if(_destroytime >= _count)
        {
            _count += 1 * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetCount()
    {
        return _count;
    }
}
