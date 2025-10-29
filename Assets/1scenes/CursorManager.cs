using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Tooltip("�� �� �̸��鿡���� Ŀ���� ���̰� �մϴ� (��: MenuScene)")]
    public string[] menuSceneNames = new string[] { "MenuScene" };

    [Tooltip("�⺻ ���� (true = ���̰�, false = ����)")]
    public bool defaultCursorVisible = false;

    void Awake()
    {
        // �̱��� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // ���� ���� ���� �ʱ� ����
        ApplyCursorForScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        // �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyCursorForScene(scene.name);
    }

    void ApplyCursorForScene(string sceneName)
    {
        bool isMenu = menuSceneNames.Contains(sceneName);
        SetCursorVisible(isMenu ? true : defaultCursorVisible);
    }

    public void SetCursorVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // �� ���� ��ȯ�� �Բ� Ŀ�����¸� ���� �����ϰ� ���� �� ��� ����
    public void LoadSceneAndSetCursor(string sceneName, bool cursorVisible)
    {
        SetCursorVisible(cursorVisible);
        SceneManager.LoadScene(sceneName);
    }
}