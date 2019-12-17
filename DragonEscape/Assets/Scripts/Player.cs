using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //ステータス
    public int hp;
    private float _speed = 1.0f;
    public float minspeed = 1.0f;
    public float maxspeed = 3.0f;

    public GameObject moneyObj;

    //移動受付用
    private float _moveX;
    private float _moveY;

    //カメラ操作用
    [SerializeField]
    private GameObject _camera;

    //速度処理用
    private Vector3 _force;

    private Rigidbody _rb;
    private Vector3 movevector;
    public float moveForceMultiplier; // 移動速度の入力に対する追従度


    // Start is called before the first frame update
    void Start()
    {
        _force = transform.position;
        _rb = GetComponent<Rigidbody>();
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

        if (Input.GetKeyDown("f"))
        {
            Instantiate(moneyObj, transform.position,Quaternion.identity);
        }
    }

    void Move()
    {

        _moveX = Input.GetAxis("Horizontal") * 10 *  _speed * Time.deltaTime;
        _moveY = Input.GetAxis("Vertical") * 10 * _speed * Time.deltaTime;
        _force = transform.up * _moveY * 10;

        _rb.AddForce(_force);
        movevector = _force;

            //transform.position += transform.up * speed * Time.deltaTime;
            //transform.position += transform.right * speed * Time.deltaTime;

            //float X_Rotation = Input.GetAxis("Mouse X");
            //float Y_Rotation = Input.GetAxis("Mouse Y");
            float Z_Rotation = Input.GetAxis("Horizontal");
        //transform.Rotate(0, X_Rotation, 0);
        //transform.Rotate(-Y_Rotation, 0, 0);
        _camera.transform.Rotate(0, 0, -Z_Rotation);
        _rb.AddForce(moveForceMultiplier * (new Vector3(movevector.x,0,movevector.z) - _rb.velocity));
    }



}
