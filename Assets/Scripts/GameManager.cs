using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] Text timeTxt;
    [SerializeField] Text endTxt;
    [SerializeField] AnimationCurve animationCurve;

    [Header("Componenet")]
    [SerializeField] Animator timeAnimator;
    [SerializeField] StageChangeFadeUI fadeUI;
    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;


    [SerializeField] int hiddenConditionCnt;




    bool isFirstWaring;
    int feiledMatchCnt = 0;
    float time = 0f;



    public int CardCount { get; private set; } = 0;
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Time.timeScale = 1;
        AudioManager.Instance.ChangeBGM(LevelManager.Instance.SelectedLevel == Level.Hidden ? BGM.Hidden : BGM.InGame);
        switch (LevelManager.Instance.SelectedLevel)
        {
            case Level.MBTI:
                time = 30f;
                break;
            case Level.Reason:
                time = 35;
                break;
            case Level.Resolution:
            case Level.Hidden:
                time = 40f;
                break;
        }


        CardCount = LevelManager.Instance.GetCardCount() * 2;
        EnterStage(LevelManager.Instance.SelectedLevel);

    }
    void Update()
    {

        time -= Time.deltaTime * (LevelManager.Instance.SelectedLevel == Level.Hidden ? 3 : 1);


        if (time <= 10)
        {
            if (!isFirstWaring)
            {
                timeTxt.color = Color.red;
                isFirstWaring = true;
                timeAnimator.gameObject.SetActive(true);
            }
            else if (time <= 0 && !IsGameOver)
            {
                IsGameOver = true;
                EndGame();
            }
        }
        timeTxt.text = time.ToString("N2");
    }

    public void isMatched()
    {

        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            CardCount -= 2;

            if (CardCount == 0)
            {
                endTxt.gameObject.SetActive(true);
                Time.timeScale = 0;
                PlayerPrefs.SetInt("ClearLevel", (int)LevelManager.Instance.SelectedLevel);
                Invoke(nameof(ClearGame), 1.5f);
            }
            AudioManager.Instance.PlaySFX(SFX.Match);
            feiledMatchCnt = 0;
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            AudioManager.Instance.PlaySFX(SFX.UnMatch);
            feiledMatchCnt++;
            if (feiledMatchCnt == hiddenConditionCnt && LevelManager.Instance.SelectedLevel != Level.Hidden)
            {
                StartCoroutine(EnterStage(Level.Hidden));

            }
        }

        firstCard = null;
        secondCard = null;
    }

    void EndGame()
    {
        if (LevelManager.Instance.SelectedLevel != Level.Hidden)
        {
            endTxt.color = Color.black;
            endTxt.text = "Game Over";
            AdsInitializer.Instance.ShowAd();
        }
        else
        {
            LevelManager.Instance.ChangeLevel(Level.MBTI);
            StartCoroutine(TextScaleTween(50, 300));
            StartCoroutine(TextRotationTween());
            endTxt.color = Color.yellow;
            endTxt.text = "��������?!?!?!?\nŷ����?!?!?!";
        }
        endTxt.gameObject.SetActive(true);
        time = 0;
        Time.timeScale = 0;
    }

    void ClearGame()
    {
        LevelManager.Instance.LevelUp();
        AdsInitializer.Instance.ShowAd();
        Time.timeScale = 0;
        PlayerPrefs.SetInt("ClearLevel", (int)LevelManager.Instance.SelectedLevel);
    }

    public IEnumerator EnterStage(Level _level)
    {
        fadeUI.PlayFade();
        yield return new WaitForSeconds(2f);
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
