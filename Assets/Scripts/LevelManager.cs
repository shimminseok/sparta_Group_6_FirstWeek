using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] List<UILevelSlot> levelUPSlots = new List<UILevelSlot>();

    public Level SelectedLevel { get; private set; } = Level.MBTI;

    int[] CardCountArray = new int[4] { 3, 6, 9, 12 };
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
        for (int i = 0; i <= (int)SelectedLevel; i++)
        {
            levelUPSlots[i]?.OpenCard();
        }
    }
    public void OnClickLevel(int _level)
    {
        ChangeLevel((Level)_level);
        SceneManager.LoadScene("SampleScene");
    }
    public int GetCardCount()
    {
        SelectedLevel = (Level)((int)SelectedLevel % CardCountArray.Length);
        return CardCountArray[(int)SelectedLevel];
    }

    public void LevelUp()
    {
        SelectedLevel += 1;
    }

    public void ChangeLevel(Level _level)
    {
        SelectedLevel = _level;

    }
}
