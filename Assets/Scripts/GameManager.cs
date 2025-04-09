using UnityEngine;
using UnityEngine.Experimental.Rendering;
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
    // ���� �ð� ����� ����
    float previousTime = 0f;
    float gameTimer;

    // ��� �ִϸ��̼� Ʈ���ſ� �ִϸ�����
    [SerializeField] Animator timeAnimator; 

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    void Start()
    {
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
        // ���� �ð� �ʱ�ȭ
        previousTime = 0f; 
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        // ���� �ð� ����� ���� ���� ����
        float timeLimit = 30f;

        if (LevelManager.Instance.selectedLevel == Level.Reason)
            timeLimit = 35f;
        else if (LevelManager.Instance.selectedLevel == Level.Resolution)
            timeLimit = 40f;

        // ���� �ð��� 10�� ���Ϸ� ó�� ������ �� ��� �ִϸ��̼� ����
        if (previousTime < timeLimit - 10f && time >= timeLimit - 10f)
        {
            timeAnimator.SetTrigger("Warning");
        }

        //������ ���� Ÿ�Ӿƿ� �ð� ����
        if(time < gameTimer)
        {
            if (LevelManager.Instance.selectedLevel == Level.MBTI && time >= 30f)
            {
                time = 30f;
            }
            else if (LevelManager.Instance.selectedLevel == Level.Reason && time >= 35f)
            {
                time = 35f;
            }
            else if (LevelManager.Instance.selectedLevel == Level.Resolution && time >= 40f)
            {
                time = 40f;
            }
            else
                EndGame();
        }

        

        //�ٸ� ������ �񱳸� ���� ���� �ð� ����
        previousTime = time;
    }

    public void isMatched()
    {

        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            cardCount -= 2;

            if(cardCount == 0)
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
        Time.timeScale = 0;
    }

    private void OnApplicationQuit()
    {
    }
}
