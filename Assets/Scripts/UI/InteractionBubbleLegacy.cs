using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionBubbleLegacy : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject speechBubble; // ��ǳ�� Panel ������Ʈ
    public Text infoText;           // ���Ž� Text ������Ʈ
    public float showDuration = 3f; // ǥ�õǴ� �ð� (��)

    private bool isVisible = false;
    private Coroutine hideCoroutine;

    void Start()
    {
        // ���� �� ���� ó��
        if (speechBubble != null)
            speechBubble.SetActive(false);

        if (infoText != null)
            infoText.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (speechBubble == null || infoText == null) return;

        // �̹� �� �ִٸ� ����
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        // ǥ�� ���� ���
        isVisible = !isVisible;

        if (isVisible)
        {
            ShowUI("��ȣ�ۿ�");
        }
        else
        {
            HideUI();
        }
    }

    private void ShowUI(string message)
    {
        // ��ǳ�� ǥ��
        speechBubble.SetActive(true);

        // �ؽ�Ʈ ǥ��
        infoText.text = message;
        infoText.gameObject.SetActive(true);

        // ���� �ð� �� �ڵ� ����
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
