using UnityEngine;
using UnityEngine.SceneManagement; // 네임스페이스 포함

/// <summary>
/// '계속하기'와 '타이틀로' 버튼이 있는 네비게이션 UI를 관리합니다.
/// 이 스크립트는 스스로 UI를 켜지 않으며, 'ShowMenu()' 메서드를 통해
/// 외부 스크립트(예: GameManager, Timeline)에 의해 호출되어야 합니다.
/// </summary>
public class NavigationMenu : MonoBehaviour
{
    // [Header("UI 설정")]
    [Tooltip("'계속하기'와 '타이틀로' 버튼이 있는 UI 패널")]
    public GameObject navigationUI;

    // [Header("씬 설정")]
    [Tooltip("이동할 다음 씬 이름 (예: MainGameScene)")]
    public string nextSceneName = "MainGameScene";

    [Tooltip("돌아갈 타이틀 씬 이름 (예: TitleScene)")]
    public string titleSceneName = "TitleScene";

    // Start() 메서드
    void Start()
    {
       
        // 이 씬은 커서가 필요하다고 가정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- Public 함수 (UI 버튼 및 외부 호출용) ---

    /// <summary>
    /// 지정된 '다음 씬'(nextSceneName)을 로드합니다.
    /// (UI의 '계속하기' 버튼 OnClick() 이벤트에 연결)
    /// </summary>
    public void LoadNextScene()
    {
        // (안전 장치) 혹시 Time.timeScale이 0이었다면 1로 복구
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// 지정된 '타이틀 씬'(titleSceneName)을 로드합니다.
    /// (UI의 '타이틀로' 버튼 OnClick() 이벤트에 연결)
    /// </summary>
    public void ReturnToTitle()
    {
        // (안전 장치) Time.timeScale 복구
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }

    /// <summary>
    /// 네비게이션 UI를 활성화하고 커서를 보이게 합니다.
    /// (외부 스크립트에서 이 함수를 호출하여 메뉴를 엽니다)
    /// </summary>
    public void ShowMenu()
    {
        // 1. UI 활성화
        if (navigationUI != null)
        {
            navigationUI.SetActive(true);
        }

        // 2. 메뉴가 보일 때 커서 활성화 (확실하게)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}