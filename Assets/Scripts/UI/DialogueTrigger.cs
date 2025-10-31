using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("��ȭâ UI ����")]
    public GameObject dialoguePanel;  // Panel ������Ʈ
    public Text dialogueText;         // Text ������Ʈ
    [TextArea]
    public string message;            // ǥ���� ���

    public float showDuration = 3f;   // �ڵ����� ������ �ð� (���ϸ� 0���� ��Ȱ��ȭ)

    private bool isShowing = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void OnMouseDown() // ������Ʈ Ŭ�� ��
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
