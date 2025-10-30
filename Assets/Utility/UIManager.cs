using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 레거시 Text 컴포넌트를 위해 추가

public class UIManager : MonoBehaviour
{
    public Text infoText; // TextMeshProUGUI 대신 Text 사용
    private Coroutine hideCoroutine;

    void Start()
    {
        if (infoText != null)
            infoText.gameObject.SetActive(false); // 시작할 때 텍스트 숨김
    }

    // 메시지를 보여주는 함수
    public void ShowMessage(string message, float duration = 3f)
    {
        if (infoText == null) return;

        // 이미 메시지가 떠있다면, 이전 숨김 코루틴 중지
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        infoText.text = message;
        infoText.gameObject.SetActive(true);

        // duration초 후에 메시지를 숨기는 코루틴 시작
        hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
    }

    // 메시지를 즉시 숨기는 함수
    public void HideMessage()
    {
        if (infoText != null)
            infoText.gameObject.SetActive(false);
    }

    // 지정된 시간 후에 텍스트를 숨기는 코루틴
    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        infoText.gameObject.SetActive(false);
        hideCoroutine = null;
    }
}