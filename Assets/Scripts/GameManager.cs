using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Text timeTxt;
    [SerializeField] StageChangeFadeUI fadeUI;
    [SerializeField] AnimationCurve animationCurve;
    public Text endTxt;

    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;

    public int cardCount = 0;
    public int hiddenConditionCnt = 1;
    public int feiledMatchCnt = 0;

    float time = 0f;

    // 경고 애니메이션 트리거용 애니메이터
    [SerializeField] Animator timeAnimator;
    bool isFirstWaring;
    public bool isGameOver;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Time.timeScale = 1;
        switch (LevelManager.Instance.selectedLevel)
        {
            case Level.MBTI:
                time = 30f;
                break;
            case Level.Reason:
                time = 35;
                break;
            case Level.Resolution:
            case Level.Hidden:
                time = 20f;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

        time -= Time.deltaTime * (LevelManager.Instance.selectedLevel == Level.Hidden ? 3 : 1);

        if (time <= 10f && !isFirstWaring)
        {
            timeTxt.color = Color.red;
            isFirstWaring = true;
            timeAnimator.gameObject.SetActive(true);
        }

        if (time <= 0 && !isGameOver)
        {
            isGameOver = true;
            EndGame();
        }
        timeTxt.text = time.ToString("N2");
    }

    public void isMatched()
    {

        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            cardCount -= 2;

            if (cardCount == 0)
            {

                Invoke(nameof(ClearGame), 1.5f);
            }
            AudioManager.Instance.PlaySFX(SFX.Match);
            feiledMatchCnt = 0;
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            feiledMatchCnt++;
            if (feiledMatchCnt == hiddenConditionCnt && LevelManager.Instance.selectedLevel != Level.Hidden)
            {
                //히든스테이지 입장
                StartCoroutine(EnterHiddenStage());
            }
        }

        firstCard = null;
        secondCard = null;
    }

    void EndGame()
    {
        if (LevelManager.Instance.selectedLevel != Level.Hidden)
        {
            endTxt.color = Color.black;
            endTxt.text = "Game Over";
        }
        else
        {
            LevelManager.Instance.selectedLevel = Level.MBTI;
            StartCoroutine(TextScaleTween(50, 300));
            StartCoroutine(TextRotationTween());
            endTxt.color = Color.yellow;
            endTxt.text = "못깨겠조?!?!?!?\n킹받조?!?!?!";
        }
        endTxt.gameObject.SetActive(true);
        time = 0;
        Time.timeScale = 0;
    }

    void ClearGame()
    {
        endTxt.gameObject.SetActive(true);
        endTxt.fontSize = 100;
        endTxt.text = "Next Level";
        Time.timeScale = 0;
        PlayerPrefs.SetInt("ClearLevel", (int)LevelManager.Instance.selectedLevel);
    }

    IEnumerator EnterHiddenStage()
    {
        fadeUI.PlayFade();
        yield return new WaitForSeconds(2f);
        LevelManager.Instance.OnClickLevel((int)Level.Hidden);
    }

    IEnumerator TextScaleTween(int _from, int _to)
    {
        endTxt.fontSize = _from;
        while (true)
        {
            float timer = 0;
            float lerpduration = 1f;
            while (timer < lerpduration)
            {
                float percent = timer / lerpduration;
                endTxt.fontSize = (int)Mathf.Lerp(_from, _to, animationCurve.Evaluate(percent));
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            timer = 0;
            lerpduration = 1f;
            while (timer < lerpduration)
            {
                float percent = timer / lerpduration;
                endTxt.fontSize = (int)Mathf.Lerp(_to, _from, percent);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator TextRotationTween()
    {

        Quaternion startRot = endTxt.rectTransform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 90f, 0f);
        while (true)
        {
            float timer = 0;
            float lerpduration = 0.5f;
            while (timer < lerpduration)
            {
                float percent = timer / lerpduration;
                endTxt.rectTransform.rotation = Quaternion.Lerp(startRot, endRot, percent);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            timer = 0;
            lerpduration = 0.5f;
            while (timer < lerpduration)
            {
                float percent = timer / lerpduration;
                endTxt.rectTransform.rotation = Quaternion.Lerp(endRot, startRot, percent);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }


            yield return null;
        }

    }
}
