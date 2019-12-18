using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //ステータス
    public int hp = 100;
    public int maxhp = 100;
    [SerializeField]
    private float _speed = 1.0f;
    public float minspeed = 1.0f;
    public float maxspeed = 3.0f;
    private int _money = 10000000;

    public GameObject moneyObj;

    //移動受付用
    private float _moveX;
    private float _moveY;

    //カメラ操作用
    [SerializeField]
    private GameObject _camera;

    //速度処理用
    private Vector3 _force;

    [SerializeField]
    private float forwordSpeed = 10; //速度係数
    [SerializeField]
    private float rotateSpeed = 5;//回転係数


    private Rigidbody _rb;
    private Vector3 movevector;
    public float moveForceMultiplier; // 移動速度の入力に対する追従度

    //HPバー用
    public Image hpbar;
    public Text moneyText;

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
                _speed += 10.0f * Time.deltaTime;
            }
        }

        else if(minspeed < _speed)
        {
            _speed -= 30.0f * Time.deltaTime;
        }

        if (Input.GetKeyDown("f"))
        {
            Instantiate(moneyObj, transform.position,Quaternion.identity);
            _money -= 10000;
        }

        //UI変更処理
        hpbar.fillAmount = (float)hp/(float)maxhp;
        moneyText.text = _money + "＄";

    }

    void Move()
    {

        _moveY = Input.GetAxis("Vertical") * forwordSpeed * _speed * Time.deltaTime;
        _force = transform.up * _moveY * 10;

        _rb.AddForce(_force);
        movevector = _force;
        
        float Z_Rotation = Input.GetAxis("Horizontal") * rotateSpeed;

        transform.Rotate(0, 0, -Z_Rotation);

        Z_Rotation = Input.GetAxis("QEHor");

        transform.Rotate(0, 0, -Z_Rotation);


        _rb.AddForce(moveForceMultiplier * (new Vector3(movevector.x,0,movevector.z) - _rb.velocity));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _rb.AddForce(moveForceMultiplier * (new Vector3(movevector.x, 0, movevector.z) - _rb.velocity));
            transform.Rotate(0, 0, -Z_Rotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if (_speed >= 10)
            {
                hp -= 1;
                _speed = 9;
            }
        }
    }



}
