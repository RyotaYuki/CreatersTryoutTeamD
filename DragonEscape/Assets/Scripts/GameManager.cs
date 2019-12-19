using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private float _gamespeed { get; set; }//ゲームの進行スピード
    private float _elapsedtime { get; set; }//ゲームが始まってから経った時間
    private int _gamemode { get; set; }//現在のゲームモード 0がタイトル　1がプレイ中　2がリザルト

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

    }
    public void GameOver()
    {
        _gamemode = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
