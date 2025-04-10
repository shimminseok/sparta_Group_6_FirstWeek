using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] GameObject selectLevelObj;

    public void OnClickStartBtn()
    {
        selectLevelObj.SetActive(true);
    }
}
