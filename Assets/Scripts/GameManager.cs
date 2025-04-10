using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] int hiddenConditionCnt;
    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;

    bool isFirstWaring;
    int feiledMatchCnt = 0;
    float time = 0f;
    public int CardCount { get; private set; } = 0;
    public bool IsGameOver { get; private set; }

    [Header("TimeSlider")]
    [SerializeField] Slider timeSlider;
    [SerializeField] Image sliderFill;
    [SerializeField] Image pikachuIsCute;


    // 경고 애니메이션 트리거용 애니메이터

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
            case Level.Resolution:
            case Level.Hidden:
                time = 40f;
                break;
        }
        CardCount = LevelManager.Instance.GetCardCount() * 2;

        EnterStage(LevelManager.Instance.SelectedLevel);
        timeSlider.maxValue = time;
    }

    // Update is called once per frame
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
        timeSlider.value = time;
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
                Invoke(nameof(ClearGame), 1.5f);
            }
            AudioManager.Instance.PlaySFX(SFX.Match);
        }
        else
        {
            AudioManager.Instance.PlaySFX(SFX.UnMatch);
            feiledMatchCnt++;
            firstCard.CloseCard();
            secondCard.CloseCard();
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
        }
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
        LevelManager.Instance.OnClickLevel((int)_level);
    }

    IEnumerator TextScaleTween(int _from, int _to)
    {
        yield return null;
        float timer = 0;
        float lerpduration = 1f;
        while (timer < lerpduration)
        {
            float percent = timer / lerpduration;

            yield return null;
        }
    }

    IEnumerator TextRotationTween()
    {

        float timer = 0;
        float lerpduration = 0.5f;
        while (timer < lerpduration)
        {
            float percent = timer / lerpduration;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }


        yield return null;
    }
}