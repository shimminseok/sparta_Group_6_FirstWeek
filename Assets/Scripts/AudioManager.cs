using UnityEngine;
using System.Collections.Generic;
using System;


public enum BGM
{
    Main,       //StartScene BGM
    InGame,    //InGame BGM
}
public enum SFX
{
    Flip,       //카드 뒤집을때 Sound
    Match,   //카드 매치시 Sound
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] List<AudioClip> bgmClips;
    [SerializeField] List<AudioClip> sfxClips;

    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource sfxPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);
    }
    public void ChangeBGM(BGM _index)
    {
        bgmPlayer.clip = bgmClips[(int)_index];
    }

    public void PlaySFX(SFX _index)
    {
        sfxPlayer.PlayOneShot(sfxClips[(int)_index]);
    }

    public void ChangeVolume(float _vol)
    {
        bgmPlayer.volume = _vol;
    }
}
