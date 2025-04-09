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
    // 이전 시간 저장용 변수
    float previousTime = 0f;
    float gameTimer;

    // 경고 애니메이션 트리거용 애니메이터
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
        // 이전 시간 초기화
        previousTime = 0f; 
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        // 제한 시간 계산을 위한 변수 설정
        float timeLimit = 30f;

        if (LevelManager.Instance.selectedLevel == Level.Reason)
            timeLimit = 35f;
        else if (LevelManager.Instance.selectedLevel == Level.Resolution)
            timeLimit = 40f;

        // 남은 시간이 10초 이하로 처음 진입할 때 경고 애니메이션 실행
        if (previousTime < timeLimit - 10f && time >= timeLimit - 10f)
        {
            timeAnimator.SetTrigger("Warning");
        }

        //레벨에 따른 타임아웃 시간 변경
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

        

        //다른 프레임 비교를 위한 이전 시간 저장
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
