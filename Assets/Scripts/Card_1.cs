using System.Collections.Generic;
using UnityEngine;

public class Card_1 : MonoBehaviour
{
    public SpriteRenderer currentSprite;
    public List<Sprite> infmSprites;

    Sprite originSprite;
    // Start is called before the first frame update
    void Start()
    {
        originSprite = currentSprite.sprite;
    } 

    public void ChangeInfmSprite()
    {
        Debug.Log(LevelManager.Instance.SelectedLevel);
        currentSprite.sprite = infmSprites[(int)LevelManager.Instance.SelectedLevel];
    }
    public void ChangeOriginSprite()
    {
        currentSprite.sprite = originSprite;
    }

    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
    }

}
