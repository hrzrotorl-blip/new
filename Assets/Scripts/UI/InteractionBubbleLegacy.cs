using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionBubbleLegacy : MonoBehaviour
{
    [Header("UI 설정")]
    public GameObject speechBubble; // 말풍선 Panel 오브젝트
    public Text infoText;           // 레거시 Text 오브젝트
    public float showDuration = 3f; // 표시되는 시간 (초)

    private bool isVisible = false;
    private Coroutine hideCoroutine;

    void Start()
    {
        // 시작 시 숨김 처리
        if (speechBubble != null)
            speechBubble.SetActive(false);

        if (infoText != null)
            infoText.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (speechBubble == null || infoText == null) return;

        // 이미 떠 있다면 리셋
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        // 표시 상태 토글
        isVisible = !isVisible;

        if (isVisible)
        {
            ShowUI("상호작용");
        }
        else
        {
            HideUI();
        }
    }

    private void ShowUI(string message)
    {
        // 말풍선 표시
        speechBubble.SetActive(true);

        // 텍스트 표시
        infoText.text = message;
        infoText.gameObject.SetActive(true);

        // 일정 시간 후 자동 숨김
        hideCoroutine = StartCoroutine(HideAfterSeconds(showDuration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideUI();
    }

    private void HideUI()
    {
        if (speechBubble != null)
            speechBubble.SetActive(false);

        if (infoText != null)
            infoText.gameObject.SetActive(false);

        isVisible = false;
        hideCoroutine = null;
    }
}
