using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Text timeTxt;
    public Text endTxt;
    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;
    public int cardCount = 0;
    [Header("TimeSlider")]
    [SerializeField] Slider timeSlider;
    [SerializeField] Image sliderFill;
    [SerializeField] Image pikachuIsCute;

    float time = 0f;
    float maxValue = 0f;

    // 경고 애니메이션 트리거용 애니메이터
    [SerializeField] Animator timeAnimator;
    bool isFirstWaring;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Time.timeScale = 1;
        switch(LevelManager.Instance.selectedLevel)
        {
            case Level.MBTI:
                time = 30f;
                break;
            case Level.Reason:
                time = 35;
                break;
            case Level.Resolution:
                time = 40f;
                break;
        }
        timeSlider.maxValue = time;
    }

    // Update is called once per frame
    void Update()
    { 

        time -= Time.deltaTime;

        if(time <= 10f && !isFirstWaring)
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
                PlayerPrefs.SetInt("ClearLevel", (int)LevelManager.Instance.selectedLevel);
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
