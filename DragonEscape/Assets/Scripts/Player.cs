using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private GameObject _stopSprite;
    private GameObject _speechbubble;
    private SpriteEffectMng _spriteEffectMng;

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
    private int _money = 50;

    [SerializeField]
    private GameObject _moneyObj;
    [SerializeField]
    private GameObject _smokeObj;

    //移動受付用
    private float _moveX;
    private float _moveY;
    [SerializeField]
    private int inputmode = 0;//1が簡単操作

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
    [SerializeField]
    private Image[] _moneyUIs;
    [SerializeField]
    private Sprite[] _moneySprites;

    //リザルト用
    [SerializeField]
    private Image[] _goalUIs;
    [SerializeField]
    private Image[] _havemoneyUIs;
    [SerializeField]
    private Image[] _yourpointUIs;


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

        //Playerの子オブジェクト内で該当オブジェクトを取得
        for(int j = 0; j < transform.childCount; j++)
        {
            if(transform.GetChild(j).name == "speech bubble")
            {
                _speechbubble = transform.GetChild(j).gameObject;
            }
            if (transform.GetChild(j).name == "STOP")
            {
                _stopSprite = transform.GetChild(j).gameObject;
            }
        }
        _speechbubble.SetActive(false);
        _stopSprite.SetActive(false);
        _spriteEffectMng = GameObject.FindGameObjectWithTag("SpriteEffectMng").GetComponent<SpriteEffectMng>();
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
            KeybordInput();
            ControllerInput();
            //スピード調整
            SpeedControl();

            ItemThrow();
        }


        if (gamemode == 2)
        {
            MoneyCheck(_goalUIs, _money);
            MoneyCheck(_havemoneyUIs, _money);
            MoneyCheck(_yourpointUIs, _money);
            if (Input.GetButtonDown("money"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    //オープニング
    void OpeningAnime()
    {
        _cameraAnimator.SetBool(_aboolopening, true);
        _canvasAnimator.SetBool(_aboolopening, true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool(_aboolopening, true);
            _animePlaying = true;
        }
        for (int j = 0; j < 10; j++)
        {
            if (Input.GetKeyDown("joystick button " + j))
            {
                _animator.SetBool(_aboolopening, true);
                _animePlaying = true;
                break;
            }
        }

        if (_animePlaying)
        {
            _cameraAnimator.SetBool(_aboolopening, false);
            _canvasAnimator.SetBool(_aboolopening, false);
        }

        if (_animePlaying && _animecount >= 1.0f)
        {
            //STOP吹き出しを出す
            _speechbubble.SetActive(true);
            _stopSprite.SetActive(true);
            _spriteEffectMng.Expansion(_speechbubble, 0.05f, 0.02f * 120);
            _spriteEffectMng.Expansion(_stopSprite, 0.07f, 0.015f * 120);
        }
        if (_animePlaying && _animecount <= 2.0f)
        {
            _animecount += 1 * Time.deltaTime;
        }
        else if (_animePlaying && _animecount >= 2.0f)
        {
            _gm.SetGameMode(1);
            _animator.SetBool(_aboolopening, false);
            MoneyCheck(_moneyUIs, _money);
            Invoke("StopSpeechOff", 2);
            _stopSprite.transform.parent = null;
            _speechbubble.transform.parent = null;
            _cameraAnimator.enabled = false;
            _animePlaying = false;
            _animecount = 0;
        }
    }

    void StopSpeechOff()
    {
        _speechbubble.SetActive(false);
        _stopSprite.SetActive(false);
    }

    //ＵＩ更新
    void UIUpdate()
    {
        //UI変更処理
        _hpbar.fillAmount = (float)_hp / (float)_maxhp;
        //_moneyText.text = _money + "＄";
    }

    //入力受け取り
    void ControllerInput() {
        //float moveX = Input.GetAxis("Horizontal");
        float moveX = 0;
        float moveY = Input.GetAxis("RTrigger") * _forwordSpeed * _speed * Time.deltaTime;
        Move(moveX, moveY);
    }
    void KeybordInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical") * _forwordSpeed * _speed * Time.deltaTime;
        Move(moveX, moveY);
    }



    void Move(float moveX,float moveY)
    {
        if (inputmode == 0)
        {
            //移動入力受け取り
            //_moveY = Input.GetAxis("RTrigger") * _forwordSpeed * _speed * Time.deltaTime;
            //_moveY = Input.GetAxis("Vertical") * _forwordSpeed * _speed * Time.deltaTime;


            _force = transform.up * moveY * _forwordSpeed;
            //
            _rb.AddForce(_force);
            _movevector = _force;
            //
            float Z_Rotation = moveX * _rotateSpeed * _speed * Time.deltaTime;
            Vector3 rotatePoint = transform.position - (transform.up * (_speed / 5));
            transform.RotateAround(rotatePoint, new Vector3(0, 1, 0), Z_Rotation);
            //慣性制限
            _rb.AddForce(_moveForceMultiplier * (new Vector3(_movevector.x, 0, _movevector.z) - _rb.velocity));
            //指定キー入れてるときより慣性を制限するように
            if (Input.GetButton("shift"))
            {
                _rb.AddForce(_moveForceMultiplier * (new Vector3(_movevector.x, 0, _movevector.z) - _rb.velocity));
                //transform.Rotate(0, 0, -Z_Rotation);
                transform.RotateAround(rotatePoint, new Vector3(0, 1, 0), Z_Rotation);

            }
        }else if(inputmode == 1)
        {
            var moveDir = new Vector3(moveX, 0, moveY);
            var axis = Vector3.Cross(transform.up, moveDir);
            var angle = Vector3.Angle(transform.up, moveDir) * (axis.y < 0 ? -1 : 1);
            //
            Debug.Log(axis);
            Debug.Log(angle);
            //float n = axis * angle;

            //if (n >= 1) {
            //    transform.Rotate(0, 0, 1);
            //}
        }
    }

    //スピード更新
    void SpeedControl()
    {
        if (_maxspeed > _speed)
        {
            _speed += _addSpeedPower * Time.deltaTime;
        }
        else if (_minspeed < _speed)
        {
            _speed -= _downSpeedPower * Time.deltaTime;
        }
    }

    //物を投げる処理
    void ItemThrow()
    {
        if (Input.GetButtonDown("money"))
        {
            Instantiate(_moneyObj, transform.position, Quaternion.identity);
            _money -= 1;
            //UI変更処理
            UIUpdate();
            MoneyCheck(_moneyUIs, _money);
        }
        if (Input.GetButtonDown("smoke"))
        {
            Instantiate(_smokeObj, transform.position, Quaternion.identity);
            _money -= 1;
            //UI変更処理
            UIUpdate();
            MoneyCheck(_moneyUIs, _money);
        }
    }

    private void MoneyCheck(Image[] nums, int money)
    {
        int digit = 0;
        int m = money;
        foreach (Image numI in nums)
        {

            digit = m % 10;
            m /= 10;
            numI.sprite = _moneySprites[digit];

        }
    }

    public void AddMoney(int money)
    {
        _money += money;
        MoneyCheck(_moneyUIs, _money);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if (_speed >= 10)
            {
                _hp -= 10;
                _speed = 9;
                //UI変更処理
                UIUpdate();
                
            }
            if(_hp <= 0)
            {
                _gm.GameOver();
            }
        }
    }



}
