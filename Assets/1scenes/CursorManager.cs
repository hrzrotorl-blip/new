using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Tooltip("이 씬 이름들에서는 커서를 보이게 합니다 (예: MenuScene)")]
    public string[] menuSceneNames = new string[] { "MenuScene" };

    [Tooltip("기본 동작 (true = 보이게, false = 숨김)")]
    public bool defaultCursorVisible = false;

    void Awake()
    {
        // 싱글턴 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // 현재 씬에 맞춰 초기 설정
        ApplyCursorForScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        // 이벤트 해제
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

    // 씬 강제 전환과 함께 커서상태를 직접 지정하고 싶을 때 사용 가능
    public void LoadSceneAndSetCursor(string sceneName, bool cursorVisible)
    {
        SetCursorVisible(cursorVisible);
        SceneManager.LoadScene(sceneName);
    }
}