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
    private Animator _cameraAnimator;
    [SerializeField]
    private GameObject _canvas;//キャンバス
    [SerializeField]
    private GameObject[] _UIs;//
    
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
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //_animator = GetComponent<Animator>();
        if (!_camera)
        {
            _camera = GameObject.Find("Main Camera");
        }

        if (_camera)
        {
            _cameraAnimator = _camera.GetComponent<Animator>();
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
            //UI変更処理
            UIUpdate();
            ItemThrow();
        }
        


    }
    void OpeningAnime()
    {

        _cameraAnimator.SetBool("opening", true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("opening", true);
            _animePlaying = true;
        }

        if (_animePlaying)
        {
            _cameraAnimator.SetBool("opening", false);
        }

        if (_animePlaying && _animecount <= 2.2f)
        {
            _animecount += 1 * Time.deltaTime;
        }
        else if (_animePlaying && _animecount >= 2.2f)
        {

            _gm.SetGameMode(1);
            _animator.SetBool("opening", false);
            _cameraAnimator.enabled = false;
            _animePlaying = false;
            _animecount = 0;
        }
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

    //ＵＩ更新
    void UIUpdate()
    {
        //UI変更処理
        hpbar.fillAmount = (float)hp / (float)maxhp;
        moneyText.text = _money + "＄";
    }

    void UIFeedIn()
    {
        foreach(GameObject ui in _UIs)
        {
            Color c = Color.black;
            if (ui.GetComponent<Image>())
            {
                Image image = ui.GetComponent<Image>();
                c = image.color;
                image.color = new Color(c.r,c.g,c.b,c.a + 0.1f);
            }
            if (ui.GetComponent<Text>())
            {
                Text text = ui.GetComponent<Text>();
                c = text.color;
                text.color = new Color(c.r, c.g, c.b, c.a + 0.1f);
            }
        }
    }

    void UIFeedOut()
    {

    }

    //スピード更新
    void SpeedControl()
    {

        if (Input.GetKey("w"))
        {
            if (maxspeed > _speed)
            {
                _speed += 10.0f * Time.deltaTime;
            }
        }

        else if (minspeed < _speed)
        {
            _speed -= 30.0f * Time.deltaTime;
        }
    }

    //物を投げる処理
    void ItemThrow()
    {
        if (Input.GetKeyDown("f"))
        {
            Instantiate(moneyObj, transform.position, Quaternion.identity);
            _money -= 10000;
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
