using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float hoverScale = 1.03f;
    public float hoverTiltZ = 2f;
    public float animTime = 0.12f;

    Vector3 baseScale;
    Quaternion baseRot;
    bool pressed;

    void Awake()
    {
        baseScale = transform.localScale;
        baseRot = transform.localRotation;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        StopAllCoroutines();
        StartCoroutine(To(baseScale * hoverScale, Quaternion.Euler(0, 0, hoverTiltZ)));
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (pressed) return;
        StopAllCoroutines();
        StartCoroutine(To(baseScale, baseRot));
    }

    public void OnPointerDown(PointerEventData e)
    {
        pressed = true;
        StopAllCoroutines();
        StartCoroutine(To(baseScale * 0.98f, Quaternion.Euler(0, 0, -hoverTiltZ * 0.6f)));
    }

    public void OnPointerUp(PointerEventData e)
    {
        pressed = false;
        StopAllCoroutines();
        StartCoroutine(To(baseScale * hoverScale, Quaternion.Euler(0, 0, hoverTiltZ)));
    }

    System.Collections.IEnumerator To(Vector3 s, Quaternion r)
    {
        float t = 0f;
        Vector3 s0 = transform.localScale;
        Quaternion r0 = transform.localRotation;
        while (t < animTime)
        {
            t += Time.unscaledDeltaTime;
            float k = t / animTime;
            k = k * k * (3f - 2f * k); // smoothstep
            transform.localScale = Vector3.Lerp(s0, s, k);
            transform.localRotation = Quaternion.Slerp(r0, r, k);
            yield return null;
        }
        transform.localScale = s;
        transform.localRotation = r;
    }
}
