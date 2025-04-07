using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryBtn : MonoBehaviour
{

    public void OnClickRetryBtn()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
