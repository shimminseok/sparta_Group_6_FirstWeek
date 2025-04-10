using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.Burst.Intrinsics;
using NUnit.Framework;
public class Board : MonoBehaviour
{
    public Transform cards;
    public GameObject card;

    public GameObject board;

    List<Card> cardList = new List<Card>(); 
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
        board.transform.localPosition = new Vector3(boardPosX, 0.6f, 0f);

        StartCoroutine(CoCardSpread(arr, level));
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

    //카드를 뿌리는 코루틴 함수
    IEnumerator CoCardSpread(int[] _arr, int _level)
    {
        for (int i = 0; i < _arr.Length; i++)
        {
            GameObject go = Instantiate(card, cards);

            float x = (i % _level) * 1.8f;
            float y = (i / _level) * 2.8f;
            go.GetComponent<Card>().Setting(_arr[i], new Vector2(x, y));

            yield return new WaitForSeconds(0f);
        }

        //모든 카드가 자기 자리에 위차할 경우, 애니메이션 실행
        foreach(var card in cardList)
        {
            card.anim.enabled = true;
        }
    }
}