using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Transform cards;
    public GameObject card;
    public int vertical = 0;

    void Start()
    {
        int[] arr = CreateCard(vertical);  // 0~8 카드 두 개씩

        // 랜덤하게 섞기
        arr = arr.OrderBy(x => Random.Range(0f, 1f)).ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            GameObject go = Instantiate(card, cards);

            float x = (i % vertical) * 1.6f;    // 가로 9칸
            float y = -(i / vertical) * 2.6f;   // 세로 2줄 (0 또는 1)

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

