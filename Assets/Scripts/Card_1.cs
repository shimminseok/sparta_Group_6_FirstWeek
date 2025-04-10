using System.Collections.Generic;
using UnityEngine;

public class Card_1 : MonoBehaviour
{
    public SpriteRenderer currentSprite;
    public List<Sprite> infmSprites;

    Sprite originSprite;
    void Start()
    {
        originSprite = currentSprite.sprite;
    } 

    public void ChangeInfmSprite()
    {
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
