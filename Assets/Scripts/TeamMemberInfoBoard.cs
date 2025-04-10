using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TeamMemberInfoBoard : MonoBehaviour
{
    public List<Transform> infmRoot = new List<Transform>();
    public List<TeamMemberInfoCard> infms = new List<TeamMemberInfoCard>();

    [SerializeField] GameObject nextTextObj;
 
    public void SettingPos()
    {
        AudioManager.Instance.ChangeBGM(BGM.Info);
        for (int i = 0; i < infms.Count; i++)
        {
            infms[i].gameObject.SetActive(true);

            Vector2 targetPos = (i == 0) ? infmRoot[1].localPosition
                              : (i == 3) ? infmRoot[2].localPosition
                              : infmRoot[i].localPosition;

            StartCoroutine(MoveInfmRoutine(i, infms[i].transform.localPosition, targetPos));
        }
    }

    private IEnumerator MoveInfmRoutine(int index, Vector2 from, Vector2 to)
    {
        yield return MoveLerp(infms[index].transform, from, to);

        if (index == 0 || index == 3)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            int midIndex = (index == 0) ? 1 : 2;
            yield return MoveLerp(infms[index].transform, infmRoot[midIndex].localPosition, infmRoot[index].localPosition);

            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var card in infms)
                StartCoroutine(RotateLerp(card));
        }
    }

    private IEnumerator MoveLerp(Transform target, Vector2 from, Vector2 to)
    {
        AudioManager.Instance.PlaySFX(SFX.Info_CardFlip);
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            target.localPosition = Vector2.Lerp(from, to, timer / duration);
            yield return null;
        }

        target.localPosition = to;
    }

    private IEnumerator RotateLerp(TeamMemberInfoCard card)
    {
        float duration = 0.15f;
        int loopCount = 5 + (infms.IndexOf(card) * 2);  // 원래 로직 유지

        for (int i = 0; i < loopCount; i++)
        {
            yield return RotateHalf(card, duration, true);
            if (i % 2 == 0) card.ChangeInfmSprite();
            else card.ChangeOriginSprite();
            yield return RotateHalf(card, duration, false);
        }
    }

    private IEnumerator RotateHalf(TeamMemberInfoCard card, float duration, bool forward)
    {
        Quaternion start = card.transform.localRotation;
        Quaternion end = start * Quaternion.Euler(0f, forward ? 90f : -90f, 0f);

        float timer = 0;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            card.transform.localRotation = Quaternion.Slerp(start, end, timer / duration);
            yield return null;
        }
        nextTextObj.SetActive(true);
    }
}

