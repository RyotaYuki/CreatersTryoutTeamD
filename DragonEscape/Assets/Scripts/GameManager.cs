using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    private float _gamespeed { get; set; }//ゲームの進行スピード
    private float _elapsedtime { get; set; }//ゲームが始まってから経った時間
    private int _gamemode { get; set; }//現在のゲームモード 0がタイトル　1がプレイ中　2がリザルト
    [SerializeField]
    private GameObject _ClearUI;

    [SerializeField]
    private bool _generateMode = false;
    [SerializeField]
    private GameObject[] _mapPaterns;

    //キャンバスアニメーター
    [SerializeField]
    private Animator _canvasAnimator;

    [SerializeField]
    private Image _goalpicture;
    [SerializeField]
    private Sprite[] _goalpicturelist;

    private void Start()
    {
        if (_generateMode)
        {

            int r = Random.Range(0, 3);
            foreach (GameObject mPatern in _mapPaterns)
            {
                mPatern.SetActive(false);
            }
            _mapPaterns[r].SetActive(true);
            _goalpicture.sprite = _goalpicturelist[r];
        }
    }

    private void Update()
    {
        if(_gamemode == 3)
        {
            if (Input.GetButtonDown("smoke"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            for (int j = 0; j < 10; j++)
            {
                if (Input.GetKeyDown("joystick button " + j))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                }
            }

        }
    }

    public int GetGameMode()
    {
        return _gamemode;
    }
    public void SetGameMode(int gamemode)
    {
        _gamemode = gamemode;
    }

    public void GameClear()
    {
        _gamemode = 2;
        _ClearUI.SetActive(true);


    }
    public void GameOver()
    {
        _gamemode = 3;
        _canvasAnimator.SetBool("gameover", true);

    }
}
