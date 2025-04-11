using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardedBtn : MonoBehaviour
{
    public void OnClickRewardBtn()
    {
        if (LevelManager.Instance.SelectedLevel == Level.Resolution)
        {
            
            LoadSceneManager.Instance.LoadScene(SceneType.EndingScene);
        }
        else
        {
            AdsInitializer.Instance.ShowAd();
        }
    }
}
