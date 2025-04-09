using UnityEngine;
using System.Collections.Generic;


public enum BGM
{
    Main,       //StartScene BGM
    InGame,    //InGame BGM
    Hidden,     //Hidden BGM
    Ending,     //Ending BGM
}
public enum SFX
{
    Flip,       //ī�� �������� Sound
    Match,   //ī�� ��ġ�� Sound
    UnMatch, //ī�� ��ġ �ҹ߽� Sound
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("BGM")]
    [SerializeField] List<AudioClip> bgmClips;
    [Header("ȿ����")]
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
        bgmPlayer.Play();
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
