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

    float previousTime = 0f;
    float time = 0f;
    float gameTimer;

    // ��� �ִϸ��̼� Ʈ���ſ� �ִϸ�����
    [SerializeField] Animator timeAnimator;

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


        // ���� �ð��� 10�� ���Ϸ� ó�� ������ �� ��� �ִϸ��̼� ����
        if (previousTime < gameTimer - 10f && time >= gameTimer - 10f)
        {
            timeAnimator.SetTrigger("Warning");
        }

        if (time >= gameTimer)
        {
            EndGame();
        }
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
        time = gameTimer;
        Time.timeScale = 0;
    }
}
