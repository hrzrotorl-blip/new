using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseFreeController : MonoBehaviour
{
    [Tooltip("�� ��ũ��Ʈ�� �۵��� �� �̸�")]
    public string targetSceneName = "MainScene2";

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // ������ ���� ��쿡�� ����
        if (currentScene == targetSceneName)
        {
            // ���콺 ��� ���� + Ŀ�� ǥ��
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Ȥ�� �ٸ� ������ �Ͻ����� ���·� �Ѿ�Դٸ� ����ȭ
            Time.timeScale = 1f;
        }
    }
}
