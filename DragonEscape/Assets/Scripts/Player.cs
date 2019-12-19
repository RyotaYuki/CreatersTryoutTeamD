using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //ゲームマネージャー
    private GameManager _gm;
    //アニメーター
    [SerializeField]
    private Animator _animator;
    //アニメカウント
    private float _animecount =0;
    private bool _animePlaying;
    //オープニングアニメーターのトリガー
    [SerializeField]
    private string _aboolopening = "opening";

    //ステータス
    [SerializeField]
    private int _hp = 100;
    [SerializeField]
    private int _maxhp = 100;
    [SerializeField]
    private float _speed = 1.0f;
    [SerializeField]
    private float _minspeed = 1.0f;
    [SerializeField]
    private float _maxspeed = 3.0f;
    private int _money = 10000000;

    [SerializeField]
    private GameObject _moneyObj;
    [SerializeField]
    private GameObject _smokeObj;

    //移動受付用
    private float _moveX;
    private float _moveY;

    //カメラ操作用
    [SerializeField]
    private GameObject _camera;
    private Animator _cameraAnimator;
    [SerializeField]
    private GameObject _canvas;//キャンバス
    private Animator _canvasAnimator;

    
    //速度処理用
    private Vector3 _force;
    [SerializeField]
    private float _forwordSpeed = 7; //速度係数
    [SerializeField]
    private float _rotateSpeed = 2.5f;//回転係数
    private float _addSpeedPower = 10;//速度上昇係数
    private float _downSpeedPower = 30;//速度減少係数

    private Rigidbody _rb;
    private Vector3 _movevector;
    [SerializeField]
    private float _moveForceMultiplier; // 移動速度の入力に対する追従度

    //HPバー用
    [SerializeField]
    private Image _hpbar;
    [SerializeField]
    private Text _moneyText;

    

    // Start is called before the first frame update
    void Start()
    {
        _force = transform.position;
        _rb = GetComponent<Rigidbody>();
        _gm = GameManager.Instance;
        if (!_hpbar) {
            _hpbar = GameObject.Find("hpbar").GetComponent<Image>();
        }
        //_animator = GetComponent<Animator>();
        if (!_camera)
        {
            _camera = GameObject.Find("Main Camera");
        }

        if (_camera)
        {
            _cameraAnimator = GameObject.Find("CameraController").GetComponent<Animator>();
        }

        if (!_canvasAnimator)
        {
            _canvasAnimator = _canvas.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームマネージャーから現在のゲームの状態を取得
        int gamemode = 0;


        //ゲームマネージャーがいなかった時はgamemodeをステージプレイに設定します。
        if (_gm)
        {
            gamemode = _gm.GetGameMode();
        }
        else
        {
            gamemode = 1;
        }


        if(gamemode == 0)
        {
            OpeningAnime();
        }

        if (gamemode == 1)
        {

            //移動処理
            Move();
            //スピード調整
            SpeedControl();

            ItemThrow();
        }
        


    }
    void OpeningAnime()
    {
        _cameraAnimator.SetBool(_aboolopening, true);
        _canvasAnimator.SetBool(_aboolopening, true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool(_aboolopening, true);
            _animePlaying = true;
        }

        if (_animePlaying)
        {
            _cameraAnimator.SetBool(_aboolopening, false);
            _canvasAnimator.SetBool(_aboolopening, false);
        }

        if (_animePlaying && _animecount <= 2.0f)
        {
            _animecount += 1 * Time.deltaTime;
        }
        else if (_animePlaying && _animecount >= 2.0f)
        {

            _gm.SetGameMode(1);
            _animator.SetBool(_aboolopening, false);
            _cameraAnimator.enabled = false;
            _animePlaying = false;
            _animecount = 0;
        }
    }
    //ＵＩ更新
    void UIUpdate()
    {
        //UI変更処理
        _hpbar.fillAmount = (float)_hp / (float)_maxhp;
        //_moneyText.text = _money + "＄";
    }

    void Move()
    {
        //移動入力受け取り
        _moveY = Input.GetAxis("Vertical") * _forwordSpeed * _speed * Time.deltaTime;
        _force = transform.up * _moveY * _forwordSpeed;

        //
        _rb.AddForce(_force);
        _movevector = _force;
        //
        float Z_Rotation = Input.GetAxis("Horizontal") * _rotateSpeed * _speed * Time.deltaTime;
        Vector3 rotatePoint = transform.position - (transform.up * (_speed / 5));
        transform.RotateAround(rotatePoint,new Vector3(0,1,0),Z_Rotation);


        //慣性制限
        _rb.AddForce(_moveForceMultiplier * (new Vector3(_movevector.x,0,_movevector.z) - _rb.velocity));

        //指定キー入れてるときより慣性を制限するように
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _rb.AddForce(_moveForceMultiplier * (new Vector3(_movevector.x, 0, _movevector.z) - _rb.velocity));
            //transform.Rotate(0, 0, -Z_Rotation);
            transform.RotateAround(rotatePoint, new Vector3(0, 1, 0), Z_Rotation);

        }
    }

    //スピード更新
    void SpeedControl()
    {

        if (Input.GetKey("w"))
        {
            if (_maxspeed > _speed)
            {
                _speed += _addSpeedPower * Time.deltaTime;
            }
        }

        else if (_minspeed < _speed)
        {
            _speed -= _downSpeedPower * Time.deltaTime;
        }
    }

    //物を投げる処理
    void ItemThrow()
    {
        if (Input.GetKeyDown("f"))
        {
            Instantiate(_moneyObj, transform.position, Quaternion.identity);
            _money -= 10000;
            //UI変更処理
            UIUpdate();
        }
        if (Input.GetKeyDown("r"))
        {
            Instantiate(_smokeObj, transform.position, Quaternion.identity);
            _money -= 10000;
            //UI変更処理
            UIUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if (_speed >= 10)
            {
                _hp -= 1;
                _speed = 9;
                //UI変更処理
                UIUpdate();
            }
        }
    }



}
