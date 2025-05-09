using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCredit : MonoBehaviour
{
    [SerializeField] float spd;
    void Start()
    {
        AudioManager.Instance.ChangeBGM(BGM.Ending);
        Time.timeScale = 1f;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.up * Time.deltaTime * spd);
        if (transform.position.y > 2500)
        {
            Invoke(nameof(DestroyEndingCredit), 3f);
        }
    }



    void DestroyEndingCredit()
    {
        LoadSceneManager.Instance.LoadScene(SceneType.StartScene);
    }
}
