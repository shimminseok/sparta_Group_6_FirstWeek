using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCredit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.ChangeBGM(BGM.Ending);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up;
        if (transform.position.y > 2500)
        {
            Destroy(gameObject);
        }
    }
}
