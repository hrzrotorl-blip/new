using UnityEngine;
using System.Collections.Generic; // Dictionary를 사용하기 위해 추가

/// <summary>
/// 이 씬이 'visitsToDisappear' 횟수만큼 로드되면,
/// 이 스크립트가 붙어있는 게임 오브젝트를 자동으로 비활성화(숨김)합니다.
/// 씬 방문 횟수는 'static' 변수에 저장되어, 
/// **게임이 실행되는 동안에만** 유지되며 게임을 끄면 초기화됩니다.
/// </summary>
public class HideAfterVisitsSession : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("이 씬을 몇 번째 방문했을 때 오브젝트가 사라질지 설정합니다. (예: 3이면 3번째 방문부터 사라짐)")]
    public int visitsToDisappear = 3;

    [Header("데이터 키 (고유해야 함)")]
    [Tooltip("방문 횟수를 추적할 고유 키입니다. 이 오브젝트만의 고유한 이름을 사용하세요.")]
    public string sceneVisitKey = "MyUniqueSceneVisitKey";

    // --- Static 변수 ---
    // 'static'은 이 변수가 게임이 실행되는 동안 단 하나만 존재하게 합니다.
    // 게임이 종료되면 이 데이터는 사라집니다.
    private static Dictionary<string, int> sessionVisitCounts = new Dictionary<string, int>();

    void Awake()
    {
        // 1. static Dictionary에서 이 'sceneVisitKey'의 현재 방문 횟수를 불러옵니다.
        // TryGetValue는 키가 존재하면 true와 함께 값을 out 변수(currentVisits)에 담고,
        // 키가 없으면 false를 반환하고 currentVisits는 기본값(0)이 됩니다.
        sessionVisitCounts.TryGetValue(sceneVisitKey, out int currentVisits);

        // 2. 현재 방문을 포함하기 위해 횟수를 1 증가시킵니다.
        int newVisits = currentVisits + 1;

        // 3. 증가된 횟수를 다시 static Dictionary에 저장합니다.
        // (키가 이미 있으면 값을 덮어쓰고, 없으면 새로 추가합니다.)
        sessionVisitCounts[sceneVisitKey] = newVisits;

        Debug.Log($"[{sceneVisitKey}] 씬 방문 {newVisits}회차 입니다. (이번 세션 한정)");

        // 4. 새로 계산된 방문 횟수가 설정한 '사라질 횟수'보다 크거나 같은지 확인합니다.
        if (newVisits >= visitsToDisappear)
        {
            // 3번째 (또는 그 이상) 방문: 오브젝트를 비활성화(숨김)
            gameObject.SetActive(false);
            Debug.Log($"방문 횟수가 {visitsToDisappear}회 이상이므로 오브젝트를 비활성화합니다.");
        }
        else
        {
            // 1, 2번째 방문: 오브젝트가 활성화된 상태를 유지
            gameObject.SetActive(true);
        }
    }
}