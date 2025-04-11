using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSlot : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] Button levelBtn;
    [SerializeField] GameObject lockObject;

    [SerializeField] bool isOpenStage;
    void Start()
    {
        levelBtn.onClick.RemoveAllListeners();
        levelBtn.onClick.AddListener(() => OnClickLevelBtn());

    }
    public void OnClickLevelBtn()
    {
        if (isOpenStage)
        {
            LevelManager.Instance.ChangeLevel(level);
            LevelManager.Instance.OnClickLevel();
        }
    }
    public void OpenCard()
    {
        isOpenStage = true;
        lockObject?.SetActive(!isOpenStage);
    }
}
