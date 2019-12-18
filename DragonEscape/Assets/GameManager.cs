using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //
    private float _gamespeed { get; set; }//ゲームの進行スピード
    private float _elapsedtime { get; set; }//ゲームが始まってから経った時間
    private int _gamemode { get; set; }//現在のゲームモード 0がタイトル　1がプレイ中　2がリザルト



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetGameMode()
    {
        return _gamemode;
    }
    public void SetGameMode(int gamemode)
    {
        _gamemode = gamemode;
    }
}
