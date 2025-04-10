using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
        StartCoroutine(FadeChangBGM((int)_index));
    }

    public void PlaySFX(SFX _index)
    {
        sfxPlayer.PlayOneShot(sfxClips[(int)_index]);
    }

    public void ChangeVolume(float _vol)
    {
        bgmPlayer.volume = _vol;
    }


    IEnumerator FadeChangBGM(int _index)
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
