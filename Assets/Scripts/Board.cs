using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using Unity.Burst.Intrinsics;
public class Board : MonoBehaviour
{
    public Transform cards;
    public GameObject card;

    public GameObject board;

    void Awake()
    {
        
    }
    void Start()
    {
        int level = LevelManager.Instance.GetCardCount();

        int[] arr = CreateCard(LevelManager.Instance.GetCardCount());  // {0,0,1,1, ...,9,9};

        arr = arr.OrderBy(x => Random.Range(0, arr.Last())).ToArray();


        //level에 따른 board의 position.x 변경
        float boardPosX = 0;
        if (LevelManager.Instance.selectedLevel == Level.MBTI)
        {
            boardPosX = 0.3f;
        }
        else if (LevelManager.Instance.selectedLevel == Level.Reason)
        {
            boardPosX = -2.3f;
        }
        else if (LevelManager.Instance.selectedLevel == Level.Resolution)
        {
            boardPosX = -5f;
        }
        board.transform.localPosition = new Vector3(boardPosX, 0.6f, 0f);

        StartCoroutine(Card(arr, level));

        GameManager.Instance.cardCount = arr.Length;
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

    IEnumerator Card(int[] _arr, int _level)
    {
        for (int i = 0; i < _arr.Length; i++)
        {
            GameObject go = Instantiate(card, cards);
            go.transform.localPosition = new Vector2(6.4f, 2.4f);
            float x = (i % _level) * 1.8f;
            float y = (i / _level) * 2.8f;
            go.GetComponent<Card>().Setting(_arr[i], new Vector2(x, y));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
