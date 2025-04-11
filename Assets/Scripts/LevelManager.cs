using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Level
{
    MBTI,
    Reason,
    Resolution,
    Hidden
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Level SelectedLevel /*{ get; private set; }*/ = Level.MBTI;
    public Level PrevLevel { get; private set; }
    int[] CardCountArray = new int[4] { 3, 6, 9, 12 };

    public bool isFirstStart;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        isFirstStart = true;
    }

    public void OnClickLevel()
    {
<<<<<<< HEAD
        LoadSceneManager.Instance.LoadScene(SceneType.InGameScene);

=======
        ChangeLevel((Level)_level);
        SceneManager.LoadScene("SampleScene");
>>>>>>> parent of 63a16ba (style : 코드 수정 및 클래스 이름 변경)
    }
    public int GetCardCount()
    {
        SelectedLevel = (Level)((int)SelectedLevel % CardCountArray.Length);
        return CardCountArray[(int)SelectedLevel];
    }

    public void LevelUp()
    {
        SelectedLevel += 1;
        if (SelectedLevel >= Level.Hidden)
        {
            SelectedLevel = Level.Resolution;
        }
    }

    public void ChangeLevel(Level _level)
    {
        if (_level != Level.Hidden)
            PrevLevel = SelectedLevel;

        SelectedLevel = _level;

    }


}
