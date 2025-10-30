using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // ���Ž� Text ������Ʈ�� ���� �߰�

public class UIManager : MonoBehaviour
{
    public Text infoText; // TextMeshProUGUI ��� Text ���
    private Coroutine hideCoroutine;

    void Start()
    {
        if (infoText != null)
            infoText.gameObject.SetActive(false); // ������ �� �ؽ�Ʈ ����
    }

    // �޽����� �����ִ� �Լ�
    public void ShowMessage(string message, float duration = 3f)
    {
        if (infoText == null) return;

        // �̹� �޽����� ���ִٸ�, ���� ���� �ڷ�ƾ ����
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        infoText.text = message;
        infoText.gameObject.SetActive(true);

        // duration�� �Ŀ� �޽����� ����� �ڷ�ƾ ����
        hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
    }

    // �޽����� ��� ����� �Լ�
    public void HideMessage()
    {
        if (infoText != null)
            infoText.gameObject.SetActive(false);
    }

    // ������ �ð� �Ŀ� �ؽ�Ʈ�� ����� �ڷ�ƾ
    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        infoText.gameObject.SetActive(false);
        hideCoroutine = null;
    }
}