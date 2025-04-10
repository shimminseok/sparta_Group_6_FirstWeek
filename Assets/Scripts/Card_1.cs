using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Card_1 : MonoBehaviour
{
    public SpriteRenderer currentSprite;
    public List<Sprite> cardSprites;

    Sprite originSprite;
    void Start()
    {
        originSprite = currentSprite.sprite;
    }

    public void ChangeCardSprite()
    {
        currentSprite.sprite = cardSprites[(int)LevelManager.Instance.SelectedLevel];
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
