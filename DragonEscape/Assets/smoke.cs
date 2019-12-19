using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke : MonoBehaviour
{
    private float _count = 0;
    [SerializeField]
    private float _destroytime = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
}
