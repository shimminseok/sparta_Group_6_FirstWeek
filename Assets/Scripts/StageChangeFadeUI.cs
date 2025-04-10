using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageChangeFadeUI : MonoBehaviour
{
    [SerializeField] Image fadeImg;
    [SerializeField] float fadeTime;



    public void PlayFadeInOut(float _time = 0)
    {
        fadeImg.enabled = true;
        StartCoroutine(FadeAndLoadScene(_time));
    }
    public void PlayFadeIn(float _time = 0)
    {
        fadeImg.enabled = true;
        StartCoroutine(FadeIn(_time));
    }
    public void PlayerFadeOut(float _time = 0)
    {
        fadeImg.enabled = true;
        StartCoroutine(FadeOut(_time));
    }
    IEnumerator FadeAndLoadScene(float _time)
    {
        yield return StartCoroutine(FadeIn(_time));
        yield return null;
        yield return StartCoroutine(FadeOut(_time));
        fadeImg.enabled = false;
    }
    IEnumerator FadeIn(float _delayTime = 0)
    {
        yield return new WaitForSeconds(_delayTime);
        yield return StartCoroutine(Fade(0f, 1f));
        fadeImg.enabled = false;
    }
    IEnumerator FadeOut(float _delayTime = 0)
    {
        yield return new WaitForSeconds(_delayTime);
        yield return StartCoroutine(Fade(1f, 0f));
        fadeImg.enabled = false;
    }
    IEnumerator Fade(float _from, float _to)
    {
        float timer = 0;
        Color c = fadeImg.color;

        while (timer < fadeTime)
        {
            float alpha = Mathf.Lerp(_from, _to,timer / fadeTime);
            fadeImg.color = new Color(c.r, c.g, c.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImg.color = new Color(c.r,c.g,c.b,0);
    }
}
