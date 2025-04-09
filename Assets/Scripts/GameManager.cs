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
    float time = 0f;
    float gameTimer;
    bool isGameover;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        AudioManager.Instance.ChangeBGM(BGM.InGame);
        Time.timeScale = 1;
        switch (LevelManager.Instance.selectedLevel)
        {
            case Level.MBTI:
                gameTimer = 30f;
                break;
            case Level.Reason:
                gameTimer = 35;
                break;
            case Level.Resolution:
                gameTimer = 40f;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        //레벨에 따른 타임아웃 시간 변경
        if (time < gameTimer)
        {
            if (LevelManager.Instance.selectedLevel == Level.MBTI)
            {
                time = 30f;
            }
            else if (LevelManager.Instance.selectedLevel == Level.Resolution)
            {
                time = 35f;
            }
            else if (LevelManager.Instance.selectedLevel == Level.Resolution)
            {
                time = 40f;
            }
        }
        else
            EndGame();
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
            AudioManager.Instance.PlaySFX(SFX.UnMatch);
        }
        firstCard = null;
        secondCard = null;
    }
    void EndGame()
    {
        endTxt.text = "Game Over";
        endTxt.gameObject.SetActive(true);
        Time.timeScale = 0;
        AudioManager.Instance.ChangeBGM(BGM.Ending);
    }

    private void OnApplicationQuit()
    {
    }
}
