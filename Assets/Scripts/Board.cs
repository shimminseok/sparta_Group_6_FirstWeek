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



    Dictionary<Level, Vector2> boardPositions = new Dictionary<Level, Vector2>
    {
        { Level.MBTI,      new Vector2( 0.3f,  0.6f) },
        { Level.Reason,    new Vector2(-2.3f, 0.6f) },
        { Level.Resolution,new Vector2(-5.0f, 0.6f) },
        { Level.Hidden,   new Vector2(-4.3f,-0.6f) }
    };
    void Start()
    {
        int cardCount = LevelManager.Instance.GetCardCount();
        int[] arr = Shuffle(cardCount);  // {0,0,1,1, ...,9,9};

        int gridX = cardCount;
        int gridY = cardCount;
        if (LevelManager.Instance.SelectedLevel == Level.Hidden)
        {
            gridX = 8;
            gridY = 8;
        }

        transform.localPosition = GetBoardPosition(LevelManager.Instance.SelectedLevel);
        StartCoroutine(SpawnCards(arr, gridX, gridY));
    }


    int[] Shuffle(int _cnt)
    {
        int[] arr = new int[_cnt * 2];
        for (int i = 0; i < _cnt; i++)
        {
            arr[i * 2] = i;
            arr[i * 2 + 1] = i;
        }
        arr = arr.OrderBy(x => Random.Range(0,arr.Last())).ToArray();
        return arr;
    }
    Vector2 GetBoardPosition(Level _level)
    {
        return boardPositions.TryGetValue(_level, out Vector2 pos) ? pos : boardPositions[Level.Hidden];
    }
    IEnumerator SpawnCards(int[] _arr, int _x, int _y)
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
