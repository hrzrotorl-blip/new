using System.Collections;
using System.Collections.Generic;
// SceneTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [Tooltip("��ȯ�� �� �̸� (Build Settings�� �߰��� �̸�)")]
    public string sceneName;

    [Tooltip("�±װ� 'Player'�� ������Ʈ�� ��ȯ�� Ʈ�����մϴ�.")]
    public string requiredTag = "Player";

    // �ɼ�: Ʈ���� �ߺ� ����
    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.CompareTag(requiredTag))
        {
            triggered = true;
            // ��� �� ��ȯ
            SceneManager.LoadScene(sceneName);
        }
    }
}

