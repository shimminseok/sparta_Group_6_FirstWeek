using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    public static TitleSceneManager Instance;

    [SerializeField] GameObject selectedOpenUIObj;
    [SerializeField] List<UILevelSlot> levelSlotLists = new List<UILevelSlot>();
    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        AudioManager.Instance?.ChangeBGM(BGM.Main);

        if (LevelManager.Instance.isFirstStart)
            OpenSelectedLevelUI();
    }

    private void Start()
    {


        OpenLevelUpSlot();
    }



    public void OpenLevelUpSlot()
    {
        for (int i = 0; i <= (int)LevelManager.Instance.SelectedLevel; i++)
        {
            levelSlotLists[i].OpenCard();
        }
    }
    public void OpenSelectedLevelUI()
    {
        selectedOpenUIObj.SetActive(true);
    }
}
