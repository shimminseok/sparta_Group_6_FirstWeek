using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum BGM
{
    Main,       //StartScene BGM
    InGame,    //InGame BGM
    Hidden,     //Hidden BGM
    Ending,     //Ending BGM
    Warning,      //Warning BGM
}
public enum SFX
{
    Flip,       //카드 뒤집을때 Sound
    Match,   //카드 매치시 Sound
    UnMatch, //카드 매치 불발시 Sound
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("BGM")]
    [SerializeField] List<AudioClip> bgmClips;
    [Header("효과음")]
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
        StartCoroutine(FadeChangeBGM((int)_index));
    }

    public void PlaySFX(SFX _index)
    {
        sfxPlayer.PlayOneShot(sfxClips[(int)_index]);
    }

    public void ChangeVolume(float _vol)
    {
        bgmPlayer.volume = _vol;
    }


    IEnumerator FadeChangeBGM(int _index)
    {
        float timer = 0f;
        float startVolume = bgmPlayer.volume;

        float fadeDuration = 0.5f;

        while (timer < fadeDuration)
        {
            ChangeVolume(Mathf.Lerp(startVolume, 0f, timer / fadeDuration));
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        bgmPlayer.Stop();
        bgmPlayer.clip = bgmClips[_index];
        bgmPlayer.Play();

        timer = 0f;

        while(timer < fadeDuration)
        {
            ChangeVolume(Mathf.Lerp(0, startVolume, timer / fadeDuration));
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        bgmPlayer.volume = startVolume;
    }
}
