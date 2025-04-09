using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageChangeFadeUI : MonoBehaviour
{
    [SerializeField] Image fadeImg;
    [SerializeField] float fadeTime;



    public void PlayFade()
    {
        fadeImg.enabled = true;
        StartCoroutine(FadeAndLoadScene());
    }
    IEnumerator FadeAndLoadScene()
    {


        yield return StartCoroutine(Fade(0f,1f));
        yield return null;
        yield return StartCoroutine(Fade(1f,0f));
        fadeImg.enabled = false;
        SceneManager.LoadScene("SampleScene");

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
