using UnityEngine;
using UnityEngine.UI; // 1. TMPro에서 UI.Text를 사용하기 위해 네임스페이스 변경

/// <summary>
/// 드래그 앤 드롭 퍼즐의 정답을 체크하고,
/// 완료 시 SceneTimer를 중지시킨 후 결과 패널에 최종 시간을 (레거시 UI Text로) 표시합니다.
/// </summary>
public class GameManager0 : MonoBehaviour
{
    [Header("퍼즐 요소")]
    public DropSlot[] slots;          // 인스펙터에서 슬롯들 연결

    [Header("UI 패널 설정")]
    public GameObject resultPanel;    // 결과창 UI
    public GameObject mainGameUI;     // 인게임 UI (숨길 대상)

    [Header("결과 텍스트 (Legacy UI Text)")]
    [Tooltip("결과창 내부에 시간을 표시할 레거시 'Text' 컴포넌트")]
    // [수정 요구 사항 2] 변수 타입 변경
    public Text finalTimeText; // TextMeshProUGUI -> Text

    private bool resultShown = false; // 결과창 중복 표시 방지

    void Start()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (mainGameUI != null)
            mainGameUI.SetActive(true);

        if (finalTimeText == null)
        {
            Debug.LogWarning("GameManager0: 'finalTimeText' (UI.Text)가 인스펙터에 할당되지 않았습니다.");
        }
    }

    // [수정 요구 사항 3] Update() 함수 로직
    void Update()
    {
        // 아직 결과가 표시되지 않았고, 모든 슬롯이 정답이라면
        if (!resultShown && AreAllSlotsCorrect())
        {
            // 1. 결과 처리를 한 번만 하도록 플래그 설정
            resultShown = true;

            // (안정성) SceneTimer 싱글톤이 존재하는지 확인
            if (SceneTimer.instance != null)
            {
                // 2. SceneTimer(스톱워치)를 멈춤
                SceneTimer.instance.StopTimer();

                // 3. 멈춘 시점의 최종 시간을 "00:00" 형식의 문자열로 가져옴
                string timeString = SceneTimer.instance.GetFormattedTime();

                // 4. 결과창 텍스트 설정 (finalTimeText가 할당되었는지 재확인)
                if (finalTimeText != null)
                {
                    // (이 .text 할당 코드는 UI.Text와 TMPro 모두 동일하게 작동합니다)
                    finalTimeText.text = "총 걸린 시간: " + timeString;
                }
            }
            else
            {
                Debug.LogWarning("GameManager0: SceneTimer.instance를 찾을 수 없습니다. 시간 기록을 스킵합니다.");
            }

            // 5. 결과창 띄우기
            if (resultPanel != null)
                resultPanel.SetActive(true);

            // 6. 인게임 UI 숨기기
            if (mainGameUI != null)
                mainGameUI.SetActive(false);
        }
    }

    /// <summary>
    /// 'slots' 배열에 있는 모든 슬롯이 정답인지 확인합니다.
    /// </summary>
    bool AreAllSlotsCorrect()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsCorrect())
            {
                return false; // 하나라도 틀리면 즉시 false 반환
            }
        }

        Debug.Log("✅ 모든 슬롯이 올바름");
        return true; // 모든 슬롯이 정답
    }
}