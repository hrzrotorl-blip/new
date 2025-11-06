using System.Collections; // 코루틴(IEnumerator)을 사용하기 위해 필요
using UnityEngine;

/// <summary>
/// 지정된 모든 드롭 슬롯(requiredSlots)의 정답 상태를 확인하고,
/// 모든 슬롯이 정답일 경우 2초간 알림 패널을 띄웁니다.
/// (OnEndDrag 이벤트 등에서 CheckCompletionState()를 호출해야 합니다)
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [Header("퍼즐 정답 조건")]
    [Tooltip("이 슬롯들이 모두 IsCorrect() 상태여야 합니다.")]
    public DropSlot[] requiredSlots; // 1. DropSlot 배열

    [Header("알림 설정")]
    [Tooltip("정답 조건을 만족했을 때 2초간 나타날 패널")]
    public GameObject notificationPanel; // 2. 알림 패널

    // 3. 알림이 현재 표시 중인지 확인 (중복 실행 방지용)
    private bool isShowingNotification = false;

    // 4. Start() 메서드
    void Start()
    {
        // 알림 패널이 할당되었다면, 게임 시작 시 비활성화
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }

        // 알림 상태 초기화
        isShowingNotification = false;
    }

    // 5. CheckCompletionState() 메서드
    /// <summary>
    /// 모든 requiredSlots가 정답인지 확인합니다.
    /// (DraggableItem의 OnEndDrag 이벤트 등에 연결하세요)
    /// </summary>
    public void CheckCompletionState()
    {
        // 1. (중복 방지) 알림이 이미 표시 중이라면 아무것도 하지 않음
        if (isShowingNotification)
        {
            return;
        }

        // 2. requiredSlots 배열의 모든 슬롯을 순회
        foreach (var slot in requiredSlots)
        {
            // 3. 만약 slot.IsCorrect()가 false인 슬롯이 "하나라도" 있다면
            if (!slot.IsCorrect())
            {
                // 즉시 메서드를 종료 (아직 정답이 아님)
                return;
            }
        }

        // 4. (모두 정답) 반복문이 중단 없이 완료되었다면 (모든 슬롯이 정답)
        //    알림 표시 코루틴을 시작합니다.
        StartCoroutine(ShowNotificationRoutine());
    }

    // 6. ShowNotificationRoutine() 코루틴
    /// <summary>
    /// 알림 패널을 2초간 켰다가 끄는 코루틴입니다.
    /// </summary>
    private IEnumerator ShowNotificationRoutine()
    {
        // 1. 알림이 "표시 중" 상태임을 기록 (중복 방지)
        isShowingNotification = true;

        // 2. 알림 패널 활성화
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(true);
        }

        // 3. 2초간 대기
        yield return new WaitForSeconds(2.0f);

        // 4. 알림 패널 다시 비활성화
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }

        // 5. 알림이 "끝났음"을 기록
        isShowingNotification = false;
    }
}