using System.Collections;
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
    public int hiddenConditionCnt = 99;
    public int feiledMatchCnt = 0;

    float time = 0f;

    // 경고 애니메이션 트리거용 애니메이터
    [SerializeField] Animator timeAnimator;
    bool isFirstWaring;
    bool isGameOver;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Time.timeScale = 1;
        AudioManager.Instance.ChangeBGM(BGM.InGame);
        switch (LevelManager.Instance.selectedLevel)
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

    }

    // Update is called once per frame
    void Update()
    { 

        time -= Time.deltaTime * (LevelManager.Instance.selectedLevel == Level.Hidden ? 2 : 1);

        if(time <= 10f && !isFirstWaring)
        {
            timeTxt.color = Color.red;
            isFirstWaring = true;
            timeAnimator.gameObject.SetActive(true);
            AudioManager.Instance.ChangeBGM(BGM.Warning);
        }

        if (time <= 0 && !isGameOver)
        {
            isGameOver = true;
            EndGame();
        }
        timeTxt.text = time.ToString("N2");
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

                Invoke(nameof(ClearGame), 1f);
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
            if (feiledMatchCnt == hiddenConditionCnt && LevelManager.Instance.selectedLevel != Level.Hidden)
            {
                //히든스테이지 입장
                StartCoroutine(EnterHiddenStage());
            }
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
        AudioManager.Instance.ChangeBGM(BGM.Ending);
    }

    void ClearGame()
    {
        endTxt.gameObject.SetActive(true);
        Time.timeScale = 0;
        PlayerPrefs.SetInt("ClearLevel", (int)LevelManager.Instance.selectedLevel);
    }

    IEnumerator EnterHiddenStage()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("히든 스테이지 입장");
        LevelManager.Instance.OnClickLevel((int)Level.Hidden);
    }
}
