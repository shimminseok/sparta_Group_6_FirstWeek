using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public SpriteRenderer frontImg;
    public Animator anim;
    public GameObject front;
    public GameObject back;
    public void Setting(int _num)
    {
        idx = _num;
        frontImg.sprite = Resources.Load<Sprite>($"mon{idx}");  //스프라이트 이름 rtan -> mon 으로 변경했습니다.
    }

    public void OpenCard()
    {
        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        AudioManager.Instance.PlaySFX(SFX.Flip);
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
        Invoke("DestoryCardInvoke", 1.0f);
    }

    void DestoryCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 1.0f);
        
    }
    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }
}
