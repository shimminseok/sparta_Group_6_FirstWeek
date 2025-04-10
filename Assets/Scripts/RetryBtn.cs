using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryBtn : MonoBehaviour
{

    public void OnClickRetryBtn()
    {
        LoadSceneManager.Instance.LoadScene(SceneType.InGameScene);

    }
}
