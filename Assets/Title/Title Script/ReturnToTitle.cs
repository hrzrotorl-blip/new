using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    // ���� Ÿ��Ʋ �� �̸� (�� �̸� ��Ȯ�� �Է�!)
    public string titleSceneName = "MainTitle";
    private static ReturnToTitle instance;

    void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϸ� �ڽ��� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // ó�� ������� �ν��Ͻ���� ����
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // ESC Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ���� Ÿ��Ʋ�� ��ȯ
            SceneManager.LoadScene(titleSceneName);
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
