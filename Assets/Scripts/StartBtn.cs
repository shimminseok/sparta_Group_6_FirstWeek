using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] GameObject selectLevelObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStartBtn()
    {
        selectLevelObj.SetActive(true);
    }
}
