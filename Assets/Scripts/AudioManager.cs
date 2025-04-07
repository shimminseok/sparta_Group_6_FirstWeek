using UnityEngine;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] List<AudioClip> bgmClips;
    [SerializeField] List<AudioClip> sfxClips;

    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource sfxPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(int _index)
    {
        bgmPlayer.clip = bgmClips[_index];
        sfxPlayer.Play();
    }

    public void PlaySFX(int _index)
    {
        sfxPlayer.PlayOneShot(sfxClips[_index]);
    }
}
