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

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        int level = LevelManager.Instance.GetCardCount();

        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        //레벨에 따른 타임아웃 시간 변경
        if(level == 3 && time >= 30f)
        {
            time = 30f;
            EndGame();
        }
        else if(level == 6 && time >= 35f)
        {
            time = 35f;
            EndGame();
        }
        else if(level == 9 && time >= 40f)
        {
            time = 40f;
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
