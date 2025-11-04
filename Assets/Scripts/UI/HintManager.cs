using UnityEngine;

/// <summary>
/// 힌트 UI 패널(GameObject)의 활성화(보이기) / 비활성화(숨기기)를 제어합니다.
/// </summary>
public class HintManager : MonoBehaviour
{
    // [변수 선언]
    [Tooltip("인스펙터에서 씬에 있는 힌트 패널(예: HintPanel_Image)을 연결하세요.")]
    public GameObject hintPanel; // 인스펙터에서 힌트 패널을 연결할 변수

    // [Public 함수 1: 힌트 보이기]
    /// <summary>
    /// 힌트 패널을 활성화(true)하여 화면에 표시합니다.
    /// (Unity의 '힌트 보기' 버튼 OnClick() 이벤트에 연결)
    /// </summary>
    public void ShowHintPanel()
    {
        // hintPanel이 null이 아닌지 (인스펙터에서 할당되었는지) 확인
        if (hintPanel != null)
        {
            // 힌트 패널을 활성화합니다.
            hintPanel.SetActive(true);
        }
        else
        {
            // 할당되지 않았을 경우, 콘솔에 경고를 출력하여 실수를 방지합니다.
            Debug.LogWarning("HintManager: 'hintPanel' 변수가 인스펙터에 할당되지 않았습니다.");
        }
    }

    // [Public 함수 2: 힌트 숨기기]
    /// <summary>
    /// 힌트 패널을 비활성화(false)하여 화면에서 숨깁니다.
    /// (Unity의 '닫기' 버튼 OnClick() 이벤트에 연결)
    /// </summary>
    public void HideHintPanel()
    {
        // hintPanel이 null이 아닌지 확인
        if (hintPanel != null)
        {
            // 힌트 패널을 비활성화합니다.
            hintPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("HintManager: 'hintPanel' 변수가 인스펙터에 할당되지 않았습니다.");
        }
    }
}