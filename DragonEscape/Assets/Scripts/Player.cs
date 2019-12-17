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

    //移動受付用
    private float _moveX;
    private float _moveY;

    //カメラ操作用
    [SerializeField]
    private GameObject _camera;

    //速度処理用
    private Vector3 _force;

    private Rigidbody2D _rb2d;
    private Vector3 movevector;
    public float moveForceMultiplier; // 移動速度の入力に対する追従度


    // Start is called before the first frame update
    void Start()
    {
        _force = transform.position;
        //_rb2d = GetComponent<Rigidbody2D>();
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

        _moveX = Input.GetAxis("Horizontal") * 10 *  _speed * Time.deltaTime;
        _moveY = Input.GetAxis("Vertical") * 10 * _speed * Time.deltaTime;
        _force = transform.up * _moveY * 10;

        //_rb2d.AddForce(_force);
        movevector = _force;

            //transform.position += transform.up * speed * Time.deltaTime;
            //transform.position += transform.right * speed * Time.deltaTime;

            //float X_Rotation = Input.GetAxis("Mouse X");
            //float Y_Rotation = Input.GetAxis("Mouse Y");
            float Z_Rotation = Input.GetAxis("Horizontal");
        //transform.Rotate(0, X_Rotation, 0);
        //transform.Rotate(-Y_Rotation, 0, 0);
        _camera.transform.Rotate(0, 0, -Z_Rotation);
        //_rb2d.AddForce(moveForceMultiplier * (new Vector2(movevector.x,movevector.y) - _rb2d.velocity));
    }



}
