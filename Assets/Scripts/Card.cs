using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public SpriteRenderer frontImg;
    public Animator anim;
    public GameObject front;
    public GameObject back;

    public float endTime = 0.1f;

    public float destroyTime = 1f;

    [SerializeField] Vector2 OpenCardOffset = new Vector2();
    [SerializeField] AnimationCurve animationCurve;

    bool isClick;
    public void Setting(int _num, Vector2 _endPos)
    {
        idx = _num;
        frontImg.sprite = Resources.Load<Sprite>($"mon{idx}");  //스프라이트 이름 rtan -> mon 으로 변경했습니다.
        StartCoroutine(MoveCard(transform.localPosition, _endPos));
    }

    public void OpenCard()
    {
        if (isClick)
            return;

        isClick = true;
        AudioManager.Instance.PlaySFX(SFX.Flip);
        StartCoroutine(CardOpenEffect());
        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.isMatched();
        }
    }


    public void DestroyCard()
    {
        anim.SetTrigger("isTrigger");
        StartCoroutine(DestoryCard(destroyTime));
    }
    IEnumerator DestoryCard(float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        StartCoroutine(CloseCard(1f));
        
    }
    IEnumerator CloseCard(float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        front.SetActive(false);
        back.SetActive(true);
    }


    IEnumerator CardOpenEffect()
    {
        Vector2 start = transform.localPosition;
        Vector2 endPos = start + OpenCardOffset;
        yield return MoveCard(start, endPos);

        front.SetActive(true);
        back.SetActive(false);

        yield return MoveCard(endPos, start);

        isClick = false;
    }
    IEnumerator MoveCard(Vector2 _from, Vector2 _to)
    {
        float time = 0f;

        while (time < endTime)
        {
            float percent = time / endTime;
            transform.localPosition = Vector2.Lerp(_from, _to, animationCurve.Evaluate(percent));
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _to;
    }
}
