using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
public class Board : MonoBehaviour
{
    public Transform cards;
    public GameObject card;

    void Awake()
    {
        
    }
    void Start()
    {
        int[] arr = CreateCard(LevelManager.Instance.GetCardCount());  // {0,0,1,1, ...,7,7};

        arr = arr.OrderBy(x => Random.Range(0, arr.Last())).ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            GameObject go = Instantiate(card, cards);
            
            // 4 * 4

            float x = (i % 9) * 1.4f;
            float y = (i / 9) * 1.4f ;
            go.transform.localPosition = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);

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
