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

    void Start()
    {
        int level = LevelManager.Instance.GetCardCount();

        int[] arr = CreateCard(level);  // {0,0,1,1, ...,9,9};

        arr = arr.OrderBy(x => Random.Range(0, arr.Last())).ToArray();


        //level에 따른 board의 position.x 변경
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
        transform.localPosition = new Vector2(boardPosX, boardPosY);
        StartCoroutine(Card(arr, x, y));
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

    IEnumerator Card(int[] _arr, int _x, int _y)
    {
        for (int i = 0; i < _arr.Length; i++)
        {
            GameObject go = Instantiate(card, cards);
            go.transform.localPosition = new Vector2(6.4f, 2.4f);
            float x = (i % _x) * 1.8f;
            float y = (i / _y) * 2.8f;
            go.GetComponent<Card>().Setting(_arr[i], new Vector2(x, y));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
