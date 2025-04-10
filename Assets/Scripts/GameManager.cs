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
    [SerializeField] Board_1 infoBoard;
    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;

    [Header("TimeSlider")]
    [SerializeField] Slider timeSlider;
    [SerializeField] Image sliderFill;
    [SerializeField] Image pikachuIsCute;

    [SerializeField] int hiddenConditionCnt;




    bool isFirstWarning;
    int feiledMatchCnt = 0;
    float time = 0f;



    public int CardCount { get; private set; } = 0;
    public bool IsGameOver { get; private set; }

    bool isGameOver;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        fadeUI.PlayerFadeOut(0.5f);
        AudioManager.Instance.ChangeBGM(LevelManager.Instance.SelectedLevel == Level.Hidden ? BGM.Hidden : BGM.InGame);
        SetInitialTime(LevelManager.Instance.SelectedLevel);

        CardCount = LevelManager.Instance.GetCardCount() * 2;
        timeSlider.maxValue = time;
        Time.timeScale = 1;

    }
    void SetInitialTime(Level _level)
    {
        switch (_level)
        {
            case Level.MBTI:
                time = 30f;
                break;
            case Level.Reason:
                time = 35f;
                break;
            case Level.Resolution:
            case Level.Hidden:
                time = 40f;
                break;
        }
    }
    void Update()
    {

        HandleTimer();
        timeSlider.value = time;
        timeTxt.text = time.ToString("N2");
    }
    void HandleTimer()
    {
        float multiplier = LevelManager.Instance.SelectedLevel == Level.Hidden ? 3 : 1;
        time -= Time.deltaTime * multiplier;

        if (time <= 10f)
        {
            if (!isFirstWarning)
            {
                TriggerLowTimeWarning();
            }
            else if (time <= 0 && !IsGameOver)
            {
                IsGameOver = true;
                EndGame();
            }
        }
    }
    void TriggerLowTimeWarning()
    {
        isFirstWarning = true;
        timeTxt.color = Color.red;
        timeAnimator.gameObject.SetActive(true);

        sliderFill.color = new Color(225 / 255f, 0, 0, 1f);
        pikachuIsCute.color = new Color(1f, 130 / 255f, 130 / 255f, 1f);
        AudioManager.Instance.ChangeBGM(BGM.Warning);
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
            feiledMatchCnt = 0;
        }
        else
        {
            AudioManager.Instance.PlaySFX(SFX.UnMatch);
            firstCard.CloseCard();
            secondCard.CloseCard();
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
        AudioManager.Instance.ChangeBGM(BGM.Ending);
        Time.timeScale = 0;
        endTxt.gameObject.SetActive(true);
        time = 0;
        if (LevelManager.Instance.SelectedLevel != Level.Hidden)
        {
            endTxt.color = Color.black;
        }
        else
        {
            LevelManager.Instance.ChangeLevel(Level.MBTI);
            endTxt.color = Color.yellow;
            StartCoroutine(TextScaleTween(50, 300));
            StartCoroutine(TextRotationTween());
            endTxt.text = "못깨겠조?!?!?\n킹받조?!?!?!";
        }
    }
    void ClearGame()
    {
        infoBoard.SettingPos();
        Time.timeScale = 0;
    }

    public IEnumerator EnterStage(Level _level)
    {
        fadeUI.PlayFadeIn();
        yield return new WaitForSeconds(2f);
        LevelManager.Instance.ChangeLevel(_level);
        if (_level != Level.Hidden)
        {
            AdsInitializer.Instance.ShowAd();
        }
        else
        {
            LevelManager.Instance.OnClickLevel((int)_level);
        }
    }
    IEnumerator TextScaleTween(int _from, int _to)
    {
        float duration = 1f;
        while (true)
        {
            yield return TweenFontSize(_from, _to, duration);
            yield return TweenFontSize(_to, _from, duration);
        }
    }
    IEnumerator TweenFontSize(int _from, int _to, float _duration)
    {
        float timer = 0f;
        while (timer < _duration)
        {
            float percent = timer / _duration;
            endTxt.fontSize = (int)Mathf.Lerp(_from, _to, animationCurve.Evaluate(percent));
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    IEnumerator TextRotationTween()
    {
        Quaternion startRot = endTxt.rectTransform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 90f, 0f);

        float duration = 0.5f;

        while (true)
        {
            yield return RotateTween(startRot, endRot, duration);
            yield return RotateTween(endRot, startRot, duration);
        }
    }
    IEnumerator RotateTween(Quaternion _from, Quaternion _to, float _duration)
    {
        float timer = 0f;
        while (timer < _duration)
        {
            float percent = timer / _duration;
            endTxt.rectTransform.rotation = Quaternion.Lerp(_from, _to, percent);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
