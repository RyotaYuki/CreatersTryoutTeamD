using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// オーディオ情報を管理します。
/// </summary>
public class AudioManager : MonoSingleton<AudioManager>
{
    /// <summary>
    /// 使用するオーディオミキサー。
    /// </summary>
    public AudioMixer Mixer;

    /// <summary>
    /// 使用するオーディオグループ。
    /// </summary>
    public AudioMixerGroup MixerBgmGroup;
    public AudioMixerGroup MixerSeGroup;
    public AudioMixerGroup MixerVoiceGroup;

    /// <summary>
    /// 管理しているオーディオ情報
    /// </summary>
    public List<AudioPlayer> AudioPlayers { get; private set; }

    /// <summary>
    /// 管理しているBgm情報を取得します。
    /// </summary>
    public List<AudioPlayer> AudioBgmPlayers
    {
        get { return AudioPlayers.FindAll(ap => ap.Source.outputAudioMixerGroup == MixerBgmGroup); }
    }

    /// <summary>
    /// 管理しているSe情報を取得します。
    /// </summary>
    public List<AudioPlayer> AudioSePlayers
    {
        get { return AudioPlayers.FindAll(ap => ap.Source.outputAudioMixerGroup == MixerSeGroup); }
    }

    /// <summary>
    /// 管理しているVoice情報を取得します。
    /// </summary>
    public List<AudioPlayer> AudioVoicePlayers
    {
        get { return AudioPlayers.FindAll(ap => ap.Source.outputAudioMixerGroup == MixerVoiceGroup); }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AudioManager()
    {
        AudioPlayers = new List<AudioPlayer>();
    }

    /// <summary>
    /// プレイヤーの登録を行います。
    /// </summary>
    /// <param name="player"></param>
    public void RegistPlayer(AudioPlayer player)
    {
        AudioPlayers.Add(player);
    }

    /// <summary>
    /// プレイヤーの登録解除を行います。
    /// </summary>
    /// <param name="player"></param>
    public void UnregistPlayer(AudioPlayer player)
    {
        AudioPlayers.Remove(player);
    }
}