using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
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

        for (int i = 0; i < arr.Length; i++)
        {
            GameObject go = Instantiate(card, cards);
            
            float x = (i % level) * 1.8f;
            float y = (i / level) * 2.8f;
            go.transform.localPosition = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);

            //level에 따른 board의 position.x 변경
            float boardPosX = 0;
            if(level == 9)
            {
                boardPosX = -5.0f;
            }
            else if(level == 6)
            {
                boardPosX = -2.3f;
            }
            else if(level == 3)
            {
                boardPosX = 0.3f;
            }

            board.transform.localPosition = new Vector3(boardPosX, 0.6f, 0f);

        }
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
}
