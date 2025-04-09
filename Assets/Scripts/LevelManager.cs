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

    
    public List<UILevelSlot> levelUPSlots = new List<UILevelSlot>();
    int[] CardCountArray = new int[4] { 3, 6, 9, 12};

    
    public Level selectedLevel = Level.MBTI;


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

        //selectedLevel = (Level)PlayerPrefs.GetInt("ClearLevel",0);

        for (int i = 0; i <= (int)selectedLevel; i++)
        {
            levelUPSlots[i]?.OpenCard();
        }
    }
    public void OnClickLevel(int _level)
    {
        selectedLevel = (Level)_level;
        SceneManager.LoadScene("SampleScene");
    }
    public int GetCardCount()
    {
        selectedLevel = (Level)((int)selectedLevel % CardCountArray.Length);
        return CardCountArray[(int)selectedLevel];
    }

    public void LevelUp()
    {
        selectedLevel += 1;
    }
}
