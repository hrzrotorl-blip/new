using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HintAttention : MonoBehaviour
{
    [Header("Timing")]
    public float delayBeforeStart = 5f; // 씬 입장 후 대기 시간(초)
    public bool loopUntilClicked = true; // 클릭되면 멈출지 여부

    [Header("Pulse (heartbeat)")]
    public float pulseAmplitude = 0.12f; // 원래 크기 대비 증감 (예: 0.12 => ±12%)
    public float pulseSpeed = 2.5f;     // 속도

    [Header("Shake")]
    public float shakeAmplitude = 8f;    // 픽셀 단위 흔들림 (UI는 RectTransform 기준)
    public float shakeSpeed = 12f;       // 흔드는 속도

    [Header("Optional")]
    public float idleScale = 1f;         // 기본 스케일 (대개 1)
    public bool startOnEnable = true;    // Enable되자마자 대기 시작할지

    // 내부 상태
    RectTransform rt;
    Vector3 startPos;
    Vector3 startScale;
    bool animating = false;
    float startTime;
    Button btn;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        btn = GetComponent<Button>();
        startPos = rt.anchoredPosition3D;
        startScale = rt.localScale;
        if (idleScale != 1f)
            startScale = Vector3.one * idleScale;
    }

    void OnEnable()
    {
        if (startOnEnable)
            StartAttentionSequence();
        // 버튼 클릭 시 애니메이션 중지 연결 (추가로 힌트 표시 함수도 연결해줘)
        btn.onClick.AddListener(OnButtonClicked_StopAnimation);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        animating = false;
        // 복원
        rt.anchoredPosition3D = startPos;
        rt.localScale = startScale;
        btn.onClick.RemoveListener(OnButtonClicked_StopAnimation);
    }

    // 외부에서 바로 시작시키고 싶을 때도 호출 가능
    public void StartAttentionSequence()
    {
        StopAllCoroutines();
        StartCoroutine(WaitThenAnimate());
    }

    IEnumerator WaitThenAnimate()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        animating = true;
        startTime = Time.time;
        // 계속 반복 (멈춤 조건은 클릭 시)
        while (animating)
        {
            float t = (Time.time - startTime);

            // pulse: 사인으로 1 +/- amplitude
            float pulse = 1f + Mathf.Sin(t * pulseSpeed) * pulseAmplitude;
            rt.localScale = startScale * pulse;

            // shake: 사인/코사인으로 좌우/상하 변위
            float x = Mathf.Sin(t * shakeSpeed) * shakeAmplitude;
            float y = Mathf.Cos(t * (shakeSpeed * 0.9f)) * (shakeAmplitude * 0.6f);
            rt.anchoredPosition3D = startPos + new Vector3(x, y, 0f);

            yield return null;
        }
        // 멈출 때 원상복구
        rt.anchoredPosition3D = startPos;
        rt.localScale = startScale;
    }

    // 버튼 클릭 시 호출되어 애니메이션 중지
    public void OnButtonClicked_StopAnimation()
    {
        if (!loopUntilClicked) return;
        animating = false;
        StopAllCoroutines();
        rt.anchoredPosition3D = startPos;
        rt.localScale = startScale;
    }

    // 만약 다른 코드에서 강제로 멈추고 싶다면 이 함수를 호출
    public void ForceStop()
    {
        animating = false;
        StopAllCoroutines();
        rt.anchoredPosition3D = startPos;
        rt.localScale = startScale;
    }
}
