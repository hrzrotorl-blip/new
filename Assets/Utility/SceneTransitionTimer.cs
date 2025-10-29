using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� �ʼ��Դϴ�.

public class SceneTransitionTimer : MonoBehaviour
{
    [Header("Ÿ�̸� ����")]
    [Tooltip("�� �ð��� ������ ���� �̵��˴ϴ�. (�� ����)")]
    public float timeToWait = 60.0f; // ��: 60��

    [Header("�̵��� �� ����")]
    [Tooltip("�ð��� �� �Ǿ��� �� �̵��� ���� �̸�")]
    public string sceneToLoad;

    // ���������� ��� �ð��� �����ϴ� ����
    private float currentTime = 0.0f;

    // Update�� �� �����Ӹ��� ȣ��˴ϴ�.
    void Update()
    {
        // 1. �� �����Ӹ��� �ð��� ���մϴ�.
        currentTime += Time.deltaTime;

        // 2. ���� �ð��� ������ ��� �ð�(timeToWait)�� �ʰ��ߴ��� Ȯ���մϴ�.
        if (currentTime > timeToWait)
        {
            // 3. �ð��� �� �Ǹ�, 'sceneToLoad'�� ������ ���� �ε��մϴ�.

            // �� �̸��� ����ִ��� Ȯ��
            if (string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.LogError("SceneTransitionTimer: 'Scene To Load'�� �̵��� �� �̸��� �������� �ʾҽ��ϴ�!");
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }

            // ���� �ε�Ǹ� �� ��ũ��Ʈ�� ���߹Ƿ�, Ÿ�̸Ӹ� �ٽ� 0���� ������ �ʿ�� �����ϴ�.
            // (���� ���� �����ϴ� ����� �ʿ��ϴٸ� enabled = false; ó���� �� �� �ֽ��ϴ�)
        }
    }
}