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
    [SerializeField] TeamMemberInfoBoard infoBoard;
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

        //timeSlider 조금 늦게 실행시키고 싶어.
        HandleTimer();

        time += Time.deltaTime;

        timeSlider.value = time;
        timeTxt.text = time.ToString("N2");

    }
    void HandleTimer()
    {
        float multiplier = LevelManager.Instance.SelectedLevel == Level.Hidden ? 3 : 1;
        // Hidden스테이지는 난이도에 차별을 두기위해서 시간을 3배 빠르게 진행시키도록 하였다.
        time -= Time.deltaTime * multiplier;

        if (time <= 10f) //10초 이하로 남았을때
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
    /// <summary>
    /// trigger가 계속 되서 좀 오류가 발생해서 bool형을 만들어서 최초 위험상태 진입시에만 Animator와 TimeSlider의 색깔 및 시간초의 색깔을 변경시켜준다.
    /// </summary>
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
        //엔드게임 함수에선 BGM을 바꿔주고 EndingBGM으로
        // timeScale을 0으로 만들어 게임을 정지시켜줌
        // endTxt의 오브젝트를 활성화 시켜서 EndText가 출력되게 하고
        //Time.Deleta으로 줄여주니까 time = 0으로 고정
        //현재 레벨이 Hidden이 아니다.
        //
        AudioManager.Instance.ChangeBGM(BGM.Ending);
        Time.timeScale = 0;
        endTxt.gameObject.SetActive(true);
        time = 0;
        if (LevelManager.Instance.SelectedLevel != Level.Hidden)
        {
            endTxt.color = Color.black;
            endTxt.text = "다시 하시조?!";
        }
        else
        {
            //MBTI 레벨로 돌아감
            LevelManager.Instance.ChangeLevel(Level.MBTI);
            endTxt.color = Color.yellow;

            //
            StartCoroutine(TextScaleTween(50, 300)); //텍스트 크기 담당.
            StartCoroutine(TextRotationTween()); //텍스트 회전 담당
            //
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
        float duration = 1f; //1초동안 실행해라
        while (true)
        {
            yield return TweenFontSize(_from, _to, duration); // TweenFontSize 코루틴이 실행이 끝날때까지 
            yield return TweenFontSize(_to, _from, duration);
        }
    }
    IEnumerator TweenFontSize(int _from, int _to, float _duration)
    {
        float timer = 0f;
        while (timer < _duration)
        {
            float percent = timer / _duration;
            endTxt.fontSize = (int)Mathf.Lerp(_from, _to, animationCurve.Evaluate(percent)); // 0~ 1
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    IEnumerator TextRotationTween()
    {
        Quaternion startRot = endTxt.rectTransform.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 90f, 0f); // 수학적인 거라서 //Euler로 짐벌락? 회전각이 서로 겹치는 현상을 짐벌락 이라고함. 근데 쿼터니언은 그걸 방지해줌 그래서 유니티는 쿼터니언
        //쿼터니언 단점이 계산이 하기가 졸라 어려워요.

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
