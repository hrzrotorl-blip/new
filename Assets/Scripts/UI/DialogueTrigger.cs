using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("대화창 UI 연결")]
    public GameObject dialoguePanel;  // Panel 오브젝트
    public Text dialogueText;         // Text 컴포넌트
    [TextArea]
    public string message;            // 표시할 대사

    public float showDuration = 3f;   // 자동으로 닫히는 시간 (원하면 0으로 비활성화)

    private bool isShowing = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void OnMouseDown() // 오브젝트 클릭 시
    {
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (dialoguePanel == null || dialogueText == null)
            return;

        dialoguePanel.SetActive(true);
        dialogueText.text = message;

        if (showDuration > 0)
            Invoke(nameof(HideDialogue), showDuration);
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}
