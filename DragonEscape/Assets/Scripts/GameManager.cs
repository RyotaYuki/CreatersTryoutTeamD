using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        _gamemode = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
