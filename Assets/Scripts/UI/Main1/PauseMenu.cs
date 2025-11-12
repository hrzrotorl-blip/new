using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // [기존] List를 사용하기 위해 필요

/// <summary>
/// 게임 전체의 일시정지 메뉴를 관리하는 싱글톤(Singleton) 스크립트입니다.
/// [수정] 씬에 'CursorManager2'가 있는지 여부를 감지하여 대응합니다.
/// [수정] 일시정지 메뉴 내의 여러 하위 패널(메인, 설정 등)을 관리하는 기능이 추가되었습니다.
/// [수정] 여러 씬에서 비활성화하는 기능이 추가되었습니다.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    // [기존] 싱글톤 인스턴스
    public static PauseMenu instance;

    // [수정] UI 설정
    [Header("UI 설정")]
    [Tooltip("활성화/비활성화할 일시정지 메뉴의 최상위 부모/배경 패널")]
    public GameObject pauseMenuBackdrop;
    [Tooltip("관리가 필요한 모든 하위 패널 (메인 버튼, 설정, 조작법 등)")]
    public List<GameObject> allPausePanels;
    [Tooltip("일시정지 시 기본으로 표시될 메인 버튼 패널")]
    public GameObject mainButtonsPanel;

    [Header("씬 설정")]
    [Tooltip("기능을 비활성화할 타이틀 씬(메인 메뉴)의 이름")]
    public string titleSceneName = "TitleScene";

    // [수정] 비활성화할 씬 목록 추가
    [Tooltip("일시정지 기능을 비활성화할 추가 씬 목록 (예: 맵 선택 씬, 상점 씬 등)")]
    public List<string> additionalDisabledScenes;

    // [기존] 현재 일시정지 상태 추적
    private bool isPaused = false;

    // [기존] Awake() 메서드
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

    // [기존] OnEnable()
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // [기존] OnDisable()
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // [기존] Start() 메서드
    void Start()
    {
        if (pauseMenuBackdrop != null)
        {
            pauseMenuBackdrop.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    // [수정] Update() 메서드
    // --- [!!! 여기가 수정된 Update() 메서드입니다 !!!] ---
    /// <summary>
    /// ESC 키 입력을 감지하여 일시정지를 토글합니다.
    /// 만약 하위 패널(옵션 등)이 열린 상태라면 ESC 키 입력을 무시합니다.
    /// </summary>
    void Update() //
    {
        Scene currentScene = SceneManager.GetActiveScene(); //

        // [기존] 비활성화 씬 검사
        if (currentScene.name == titleSceneName || additionalDisabledScenes.Contains(currentScene.name)) //
        {
            return; // 비활성화 씬이면 ESC 입력 무시
        }

        // [수정된 ESC 키 로직]
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) // 1. 이미 일시정지 상태일 때
            {
                // 2. [!!!] 만약 '메인 버튼 패널'이 "비활성화" 상태라면 
                //    (즉, '옵션'이나 '조작법' 같은 하위 패널이 켜져 있다면)
                if (mainButtonsPanel != null && !mainButtonsPanel.activeInHierarchy)
                {
                    // 아무것도 하지 않고 Update 함수를 종료합니다.
                    // (ESC 키 무시)
                    return;
                }
                else
                {
                    // 3. '메인 패널'이 켜져 있었다면, 일시정지를 해제합니다.
                    Resume(); //
                }
            }
            else // 4. 일시정지가 아니었다면
            {
                // 일시정지를 시작합니다.
                Pause(); //
            }
        }
    }

    // [수정] OnSceneLoaded() 메서드
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // [수정] titleSceneName 또는 additionalDisabledScenes 목록에 포함된 씬인지 확인
        if (scene.name == titleSceneName || additionalDisabledScenes.Contains(scene.name))
        {
            // "비활성화 씬"이 로드된 경우
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (pauseMenuBackdrop != null)
            {
                pauseMenuBackdrop.SetActive(false);
            }
        }
        else
        {
            // "게임 씬"(활성화 씬)이 로드된 경우
            Time.timeScale = 1f;
            isPaused = false;
            CheckSceneAndApplyCursorState();
        }
    }


    // [기존] public void Resume() 메서드
    public void Resume()
    {
        if (pauseMenuBackdrop != null)
        {
            pauseMenuBackdrop.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
        CheckSceneAndApplyCursorState();
    }

    // [기존] CheckSceneAndApplyCursorState() 메서드
    private void CheckSceneAndApplyCursorState()
    {
        if (FindObjectOfType<CursorManager2>() == null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // [기존] public void Pause() 메서드
    public void Pause()
    {
        if (pauseMenuBackdrop != null)
        {
            pauseMenuBackdrop.SetActive(true);
        }

        ShowPausePanel(mainButtonsPanel);

        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // [기존] public void ReturnToTitle() 메서드
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(titleSceneName); // [수정] 이 부분은 mainTitleSceneName으로 바꿀 수 있지만, 현재는 titleSceneName을 유지합니다.
    }

    // [기존] 하위 패널 관리를 위한 새 메서드
    public void HideAllPausePanels()
    {
        if (allPausePanels == null) return;
        foreach (GameObject panel in allPausePanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    public void ShowPausePanel(GameObject panelToShow)
    {
        HideAllPausePanels();

        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }
}