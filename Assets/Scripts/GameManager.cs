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

        if (time <= 10f && !isFirstWaring)
        {
            timeTxt.color = Color.red;
            isFirstWaring = true;

            sliderFill.color = new Color(225/255f, 0/255f, 0/255f, 1f);
            pikachuIsCute.color = new Color(255 / 255f, 130 / 255f, 130 / 255f, 1f);

            //timeAnimator.SetTrigger("Warning");
        }

        if (time <= 0)
        {
            EndGame();
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

            cardCount -= 2;

            if (cardCount == 0)
            {
                endTxt.gameObject.SetActive(true);
                Time.timeScale = 0;
                PlayerPrefs.SetInt("ClearLevel", (int)LevelManager.Instance.SelectedLevel);
            }
            AudioManager.Instance.PlaySFX(SFX.Match);
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;
    }

    void EndGame()
    {
        endTxt.text = "Game Over";
        endTxt.gameObject.SetActive(true);
        time = 0;
        Time.timeScale = 0;
    }


}
