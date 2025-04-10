using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Rendering;

public class Board_1 : MonoBehaviour
{
    public List<Transform> cardRoot = new List<Transform>();
    public List<Card_1> cards = new List<Card_1>();

    bool isAllFlip;
    private void Start()
    {
        SettingPos();
    }
    public void SettingPos()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].gameObject.SetActive(true);
            Vector2 targetPos = (i == 0) ? cardRoot[1].localPosition : (i == 3) ? cardRoot[2].localPosition : cardRoot[i].localPosition;

            StartCoroutine(MoveCardRoutine(i, cards[i].transform.localPosition, targetPos));
        }

    }

    private IEnumerator MoveCardRoutine(int _index, Vector2 _from, Vector2 _to)
    {
        yield return StartCoroutine(MoveLerp(cards[_index].transform, _from, _to));


        if (_index == 0 || _index == 3)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            int midIndex = (_index == 0) ? 1 : 2;
            yield return StartCoroutine(MoveLerp(cards[_index].transform,
                                                 cardRoot[midIndex].localPosition,
                                                 cardRoot[_index].localPosition));


            yield return new WaitForSecondsRealtime(0.5f);
            for (int i = 0; i < cards.Count; i++)
            {
                StartCoroutine(RotateLerp(i));
            }
        }
    }

    private IEnumerator MoveLerp(Transform _target, Vector2 _from, Vector2 _to)
    {
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            _target.localPosition = Vector2.Lerp(_from, _to, timer / duration);
            yield return null;
        }

        _target.localPosition = _to; // 최종 보정
    }

    IEnumerator RotateLerp(int _index)
    {
        float duration = 0.15f;

        for (int i = 0; i < 3 + (_index * 2); i++)
        {
            Quaternion from = cards[_index].transform.localRotation;
            Quaternion toHalf = from * Quaternion.Euler(0, 90f, 0);

            float timer = 0;
            while (timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                cards[_index].transform.localRotation = Quaternion.Slerp(from, toHalf, timer / duration);
                yield return null;
            }

            if (i % 2 == 0)
                cards[_index].ChangeCardSprite();
            else
                cards[_index].ChangeOriginSprite();

            Quaternion toFull = toHalf * Quaternion.Euler(0, 90f, 0);
            Quaternion fromHalf = cards[_index].transform.localRotation;
            timer = 0;
            while(timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                cards[_index].transform.localRotation = Quaternion.Slerp(fromHalf, toFull, timer / duration);
                yield return null;
            }
        }
    }
}
