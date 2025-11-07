using UnityEngine;
using System.Collections.Generic; // List<T>를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement; // [수정 요청 1] 씬 관리를 위해 추가

/// <summary>
/// 타이틀 씬의 메인 UI 패널(설명, 설정 등)을 관리합니다.
/// 이 스크립트는 'ShowPanel'을 통해 한 번에 하나의 패널만 활성화되도록 보장합니다.
/// [수정] 게임 시작 및 종료 기능이 추가되었습니다.
/// </summary>
public class TitleManager : MonoBehaviour
{
    // [기존] 모든 관리 대상 패널을 담는 리스트
    [Header("UI Panel Management")]
    [Tooltip("관리가 필요한 모든 UI 패널(GameObject)을 이 리스트에 등록하세요.")]
    public List<GameObject> allPanels;

    // [수정 요청 2 & 3] 씬 관리 변수 추가
    [Header("Scene Management")]
    [Tooltip("Game Start 버튼 클릭 시 이동할 씬의 이름 (예: MainScene)")]
    public string gameSceneName;

    /// <summary>
    /// 씬이 시작될 때 모든 패널을 숨겨 깨끗한 상태로 만듭니다.
    /// </summary>
    void Start()
    {
        // (권장) 시작 시 모든 패널을 숨깁니다.
        HideAllPanels();
    }

    // [기존] 모든 패널을 숨기는 메서드
    /// <summary>
    /// 'allPanels' 리스트에 등록된 모든 패널을 비활성화(숨기기)합니다.
    /// </summary>
    public void HideAllPanels()
    {
        // 리스트가 비어있지 않은지 확인
        if (allPanels == null) return;

        // 리스트를 순회하며 모든 패널을 끈다
        foreach (GameObject panel in allPanels)
        {
            // (안정성) 리스트의 항목이 null이 아닐 때만 SetActive(false) 호출
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    // [기존] 특정 패널만 보여주는 메서드
    /// <summary>
    /// 모든 패널을 숨긴 후, 'panelToShow' 매개변수로 받은 특정 패널만 켭니다.
    /// (Unity Button의 OnClick() 이벤트에 연결하여 사용합니다)
    /// </summary>
    /// <param name="panelToShow">활성화할 UI 패널 GameObject</param>
    public void ShowPanel(GameObject panelToShow)
    {
        // [요구 사항 5a] 먼저 모든 패널을 닫습니다.
        HideAllPanels();

        // (안정성) 매개변수로 받은 패널이 null이 아닌지 확인
        if (panelToShow != null)
        {
            // [요구 사항 5b] 지정된 패널만 켭니다.
            panelToShow.SetActive(true);
        }
        else
        {
            Debug.LogWarning("TitleManager: ShowPanel()에 null이 전달되었습니다. 켜려고 한 패널이 할당되었는지 확인하세요.");
        }
    }

    // --- [수정 요청 4 & 5] 새 메서드 추가 ---

    // [수정 요청 4] 게임 시작 메서드
    /// <summary>
    /// 'gameSceneName'에 지정된 씬을 로드하여 게임을 시작합니다.
    /// (Unity Button의 OnClick() 이벤트에 연결)
    /// </summary>
    public void StartGame()
    {
        // (안정성) 인스펙터에서 씬 이름을 설정했는지 확인
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogWarning("TitleManager: 'Game Scene Name'이(가) 인스펙터에 설정되지 않았습니다!");
            return;
        }

        // 지정된 씬 로드
        SceneManager.LoadScene(gameSceneName);
    }

    // [수정 요청 5] 게임 종료 메서드
    /// <summary>
    /// 애플리케이션을 종료합니다.
    /// (Unity Button의 OnClick() 이벤트에 연결)
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("게임 종료 버튼 클릭됨");

        // 유니티 에디터에서 테스트 시 Play 모드 중지
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 실제 빌드된 게임에서 애플리케이션 종료
            Application.Quit();
#endif
    }
}