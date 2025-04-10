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
        AdsInitializer.Instance.ShowAd();
    }
}
