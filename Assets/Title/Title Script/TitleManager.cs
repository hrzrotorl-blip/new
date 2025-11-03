using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 타이틀 씬의 UI 및 씬 전환을 관리합니다.
/// (패널 관리 기능이 추가됨)
/// </summary>
public class TitleManager : MonoBehaviour
{
    // 1. 인스펙터에서 연결할 패널 변수 선언
    [Header("UI Panels")]
    public GameObject descriptionPanel;
    public GameObject settingsPanel;
    // (참고: 나중에 패널이 더 늘어난다면 List<GameObject>로 관리하는 것이 효율적입니다.)

    // 5. StartGame() 함수는 그대로 유지
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // 게임 본편 씬 이름
    }

    // 2. OpenOptions() 함수 제거 (제거됨)

    // 5. ExitGame() 함수는 그대로 유지
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 경우, 플레이 모드를 중지합니다.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // 4. HideAllPanels() 함수 새로 생성
    /// <summary>
    /// 관리되는 모든 패널(설명, 설정)을 비활성화합니다.
    /// </summary>
    public void HideAllPanels()
    {
        // NullReferenceException 방지를 위해 null 체크
        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // 3. ShowPanel(GameObject panelToShow) 함수 새로 생성
    /// <summary>
    /// 먼저 모든 관리 패널을 숨긴 후, 
    /// 지정된 패널(panelToShow)만 활성화합니다.
    /// </summary>
    /// <param name="panelToShow">활성화할 UI 패널 GameObject</param>
    public void ShowPanel(GameObject panelToShow)
    {
        // 먼저 모든 패널을 끄는 함수를 재사용합니다.
        HideAllPanels();

        // 그 다음, 요청된 패널만 켭니다.
        // (마찬가지로 null 체크)
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }
}