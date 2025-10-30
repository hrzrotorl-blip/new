using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject messagePanel;   // Panel 오브젝트
    public Text messageText;          // Panel 안의 Text (Legacy Text)

    void Start()
    {
        // 게임 시작 시 Panel을 숨김
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    // DropSlot에서 호출
    public void ShowMessage(string message)
    {
        if (messagePanel == null || messageText == null)
        {
            Debug.LogWarning("UIManager: Panel 또는 Text가 연결되지 않았습니다!");
            return;
        }

        messagePanel.SetActive(true);
        messageText.text = message;
    }

    // 닫기용 (선택사항)
    public void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
