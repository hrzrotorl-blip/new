using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonShine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform shine;      // 자식 "Shine"
    public float duration = 0.6f;    // 지나가는 시간
    public float cooldown = 0.8f;    // 다음 번까지 대기
    public float angleZ = 25f;       // Shine 회전
    public float extraWidth = 100f;  // 좌우 오프스크린 여유

    Coroutine run;

    void Reset()
    {
        // 자동으로 찾아주기 (없으면 드래그로 연결)
        if (shine == null)
            shine = transform.Find("Shine") as RectTransform;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (shine == null) return;
        if (run == null) run = StartCoroutine(Loop());
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (run != null) { StopCoroutine(run); run = null; }
        if (shine != null) shine.gameObject.SetActive(false);
    }

    IEnumerator Loop()
    {
        var rt = (RectTransform)transform;
        shine.gameObject.SetActive(true);
        shine.localEulerAngles = new Vector3(0, 0, angleZ);

        while (true)
        {
            float width = rt.rect.width + shine.rect.width + extraWidth;
            Vector2 start = new Vector2(-width * 0.5f, 0f);
            Vector2 end = new Vector2(width * 0.5f, 0f);

            float t = 0f;
            shine.anchoredPosition = start;

            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float k = t / duration;
                shine.anchoredPosition = Vector2.Lerp(start, end, k);
                yield return null;
            }
            shine.anchoredPosition = end;

            yield return new WaitForSecondsRealtime(cooldown);
        }
    }
}
