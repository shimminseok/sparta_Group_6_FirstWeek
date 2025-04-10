using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;
public class Board : MonoBehaviour
{
    public Transform cards;
    public GameObject card;


    List<Card> cardList = new List<Card>(); 
    void Start()
    {
        int level = LevelManager.Instance.GetCardCount();

        int[] arr = CreateCard(level);  // {0,0,1,1, ...,9,9};

        arr = arr.OrderBy(x => Random.Range(0, arr.Last())).ToArray();


        int x = level;
        int y = level;
        float boardPosX = 0;
        float boardPosY = 0.6f;
        if (LevelManager.Instance.SelectedLevel == Level.MBTI)
        {
            boardPosX = 0.3f;
        }
        else if (LevelManager.Instance.SelectedLevel == Level.Reason)
        {
            boardPosX = -2.3f;
        }
        else if (LevelManager.Instance.SelectedLevel == Level.Resolution)
        {
            boardPosX = -5f;
        }
        else
        {
            boardPosX = -4.3f;
            boardPosY = -0.6f;
            x = 8;
            y = 8;
        }
        StartCoroutine(CoCardSpread(arr, level));
        transform.localPosition = new Vector2(boardPosX, boardPosY);
    }


    int[] CreateCard(int _cnt)
    {
        int[] arr = new int[_cnt * 2];
        for (int i = 0; i < _cnt; i++)
        {
            arr[i * 2] = i;
            arr[i * 2 + 1] = i;
        }

        return arr;
    }
    IEnumerator CoCardSpread(int[] _arr, int _level)
    {
        for (int i = 0; i < _arr.Length; i++)
        {
            GameObject go = Instantiate(card, cards);

            float x = (i % _level) * 1.8f;
            float y = (i / _level) * 2.8f;

            Card cardScript = go.GetComponent<Card>();
            cardScript.Setting(_arr[i], new Vector2(x,y));
            cardList.Add(cardScript);

            yield return new WaitForSeconds(0f);
        }

        foreach (var card in cardList)
        {
            card.anim.enabled = true;
        }
    }
}
