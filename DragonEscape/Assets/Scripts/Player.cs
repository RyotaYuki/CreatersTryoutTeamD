using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //ステータス
    public int hp;
    public float _speed = 1.0f;
    public float minspeed = 1.0f;
    public float maxspeed = 3.0f;

    //移動受付用
    private float _moveX;
    private float _moveY;

    //カメラ操作用
    [SerializeField]
    private GameObject _camera;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey("w"))
        {
            if(maxspeed > _speed)
            {
                _speed += 1.0f * Time.deltaTime;
            }
        }

        else if(minspeed < _speed)
        {
            _speed -= 1.0f * Time.deltaTime;
        }
    }

    void Move()
    {
        _moveX = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        _moveY = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        transform.position += transform.up * _moveY;

        //transform.position += transform.up * speed * Time.deltaTime;
        //transform.position += transform.right * speed * Time.deltaTime;

        //float X_Rotation = Input.GetAxis("Mouse X");
        //float Y_Rotation = Input.GetAxis("Mouse Y");
        float Z_Rotation = Input.GetAxis("Horizontal");
        //transform.Rotate(0, X_Rotation, 0);
        //transform.Rotate(-Y_Rotation, 0, 0);
        _camera.transform.Rotate(0, 0, -Z_Rotation);

    }



}
