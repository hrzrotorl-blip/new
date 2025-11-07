using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 전체의 일시정지 메뉴를 관리하는 싱글톤(Singleton) 스크립트입니다.
/// [수정] 씬에 'CursorManager2'가 있는지 여부를 감지하여
/// 1인칭 씬(커서 잠금)과 퍼즐 씬(커서 보임)에 자동으로 대응합니다.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    // [기존] 싱글톤 인스턴스
    public static PauseMenu instance;

    // [기존] UI 및 씬 설정
    [Header("UI 설정")]
    [Tooltip("활성화/비활성화할 일시정지 메뉴 패널(GameObject)")]
    public GameObject pauseMenuUI;

    [Header("씬 설정")]
    [Tooltip("기능을 비활성화할 타이틀 씬(메인 메뉴)의 이름")]
    public string titleSceneName = "TitleScene";

    // [기존] 현재 일시정지 상태 추적
    private bool isPaused = false;

    // [기존] Awake() 메서드 - 싱글톤 로직
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // [기존] OnEnable() - 씬 로드 이벤트 등록
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // [기존] OnDisable() - 씬 로드 이벤트 해제
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // [기존] Start() 메서드 (커서 로직 제거됨)
    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    // [기존] Update() 메서드 (타이틀 씬 감지)
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == titleSceneName)
        {
            return; // 타이틀 씬이면 ESC 입력 무시
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // [수정 요청 2] OnSceneLoaded() 메서드 수정
    /// <summary>
    /// 씬이 로드될 때마다 호출되어, 씬에 맞게 UI와 커서 상태를 재설정합니다.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == titleSceneName)
        {
            // "타이틀 씬"이 로드된 경우 (커서 활성화)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }
        }
        else
        {
            // "타이틀 씬"이 아닌 "다른 씬"(게임 씬)이 로드된 경우
            // [수정] Resume()을 직접 호출하는 대신, 
            // Resume()의 핵심 로직을 여기에 복제/적용합니다.

            // 1. 시간 정상화 및 상태 초기화
            Time.timeScale = 1f;
            isPaused = false;

            // 2. 씬을 검사하여 커서 상태 결정
            CheckSceneAndApplyCursorState();
        }
    }


    // [수정 요청 1] public void Resume() 메서드 수정
    /// <summary>
    /// 게임을 재개합니다. (UI의 '계속하기' 버튼 OnClick() 이벤트에 연결)
    /// </summary>
    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // UI 숨기기
        }
        Time.timeScale = 1f;              // 시간 정상화
        isPaused = false;                 // 상태 변경

        // 씬을 검사하여 커서 상태 결정
        CheckSceneAndApplyCursorState();
    }

    /// <summary>
    /// 씬에 'CursorManager2'가 있는지 확인하여
    /// 1인칭 씬(잠금) 또는 퍼즐 씬(보임)에 맞게 커서 상태를 적용합니다.
    /// </summary>
    private void CheckSceneAndApplyCursorState()
    {
        // [수정] 씬에 CursorManager2가 있는지 확인
        if (FindObjectOfType<CursorManager2>() == null)
        {
            // 1. CursorManager2가 없음 (1인칭 씬으로 간주)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // 2. CursorManager2가 있음 (퍼즐 씬으로 간주)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    // [기존] public void Pause() 메서드 (일시정지)
    /// <summary>
    /// 게임을 일시정지합니다. (ESC 키 입력 시 자동으로 호출됨)
    /// </summary>
    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // UI 보이기
        }
        Time.timeScale = 0f;             // 시간 정지
        isPaused = true;                 // 상태 변경

        // 일시정지 시에는 씬 종류와 관계없이 항상 커서를 풀어줌
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // [기존] public void ReturnToTitle() 메서드
    /// <summary>
    /// 타이틀 씬(메인 메뉴)으로 돌아갑니다. (UI의 '타이틀로' 버튼 OnClick() 이벤트에 연결)
    /// </summary>
    public void ReturnToTitle()
    {
        Time.timeScale = 1f; // 시간 정지 해제
        isPaused = false;    // 상태 초기화

        SceneManager.LoadScene(titleSceneName);
        // (씬이 로드되면 OnSceneLoaded가 호출되어 타이틀 씬용 커서로 자동 변경)
    }
}