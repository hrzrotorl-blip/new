using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요
using UnityEngine.UI; // [수정 요구 사항 1] TMPro에서 UI로 변경

/// <summary>
/// 씬이 로드된 후부터 시간을 누적하는 스톱워치 (싱글톤).
/// (UI.Text 버전을 사용하며, 정지 및 시간 반환 기능이 포함됨)
/// </summary>
public class SceneTimer : MonoBehaviour
{
    public static SceneTimer instance; // 싱글톤 인스턴스

    [Header("UI 설정")]
    [Tooltip("씬에서 찾을 UI Text 오브젝트의 이름 (Hierarchy 상의 이름)")]
    public string timerTextObjectName = "TimerText"; // 예: "TimeText_UI"

    // [수정 요구 사항 2] 변수 타입 변경
    private Text timerText; // TextMeshProUGUI -> Text

    private float totalElapsedTime; // 총 누적 시간

    // [기능 추가 1]
    private bool isRunning = true;   // 타이머가 실행 중인지 여부

    void Awake()
    {
        // (싱글톤 패턴)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 씬이 로드될 때마다 OnSceneLoaded 함수가 호출되도록 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // (싱글톤 패턴) 이벤트 등록 해제
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // [수정 요구 사항 3] OnSceneLoaded 함수 수정
    /// <summary>
    /// 씬이 새로 로드될 때마다 호출되어, 'timerTextObjectName'에 해당하는
    /// UI Text 컴포넌트를 찾아서 'timerText' 변수에 할당합니다.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에서 'timerTextObjectName' 이름을 가진 게임 오브젝트를 찾습니다.
        GameObject textObject = GameObject.Find(timerTextObjectName);

        if (textObject != null)
        {
            // 찾은 오브젝트에서 'Text' 컴포넌트를 가져옵니다.
            timerText = textObject.GetComponent<Text>(); // [수정 3-1]

            if (timerText != null)
            {
                // 성공적으로 찾았으면, 현재 누적 시간으로 UI를 즉시 업데이트
                UpdateTimerUI(totalElapsedTime);
            }
            else
            {
                // [수정 3-2] 경고 메시지 변경 (Text)
                Debug.LogWarning($"SceneTimer: '{timerTextObjectName}' 오브젝트를 찾았으나, 'Text' 컴포넌트가 없습니다.");
            }
        }
        else
        {
            // [수정 3-2] 경고 메시지 변경 (Text)
            Debug.LogWarning($"SceneTimer: '{scene.name}' 씬에서 '{timerTextObjectName}' 이름을 가진 'Text' 오브젝트를 찾을 수 없습니다.");
            timerText = null; // 못 찾았으므로 null로 설정
        }
    }

    // [기능 추가 2] Update() 함수 수정
    void Update()
    {
        // isRunning이 true일 때만 시간을 누적하고 UI를 업데이트
        if (isRunning)
        {
            totalElapsedTime += Time.deltaTime;

            // timerText가 (OnSceneLoaded에 의해) 성공적으로 할당된 상태라면
            if (timerText != null)
            {
                // [기능 추가 5] 분리된 함수 호출
                UpdateTimerUI(totalElapsedTime);
            }
        }
    }

    // [기능 추가 5] UI 업데이트 로직 분리
    /// <summary>
    /// 지정된 시간을 "00:00" 형식으로 timerText에 업데이트합니다.
    /// </summary>
    void UpdateTimerUI(float timeToDisplay)
    {
        if (timerText == null) return; // 안전 장치

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // [기능 추가 3] 타이머 정지 함수
    /// <summary>
    /// 타이머의 시간 누적을 멈춥니다.
    /// (예: 게임 클리어 시 GameManager0에서 호출)
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;

        // 멈추는 순간의 최종 시간으로 UI를 한 번 더 업데이트
        if (timerText != null)
        {
            UpdateTimerUI(totalElapsedTime);
        }
    }

    // [기능 추가 4] 포맷된 시간 반환 함수
    /// <summary>
    /// 현재까지 누적된 시간을 "00:00" 형식의 문자열로 반환(return)합니다.
    /// (예: 결과창 UI에 시간을 표시할 때 사용)
    /// </summary>
    /// <returns>포맷팅된 시간 문자열 (예: "01:30")</returns>
    public string GetFormattedTime()
    {
        float minutes = Mathf.FloorToInt(totalElapsedTime / 60);
        float seconds = Mathf.FloorToInt(totalElapsedTime % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// 타이머 시간을 0으로 리셋하고 다시 시작합니다.
    /// </summary>
    public void ResetTimer()
    {
        totalElapsedTime = 0f;
        isRunning = true;

        if (timerText != null)
        {
            UpdateTimerUI(totalElapsedTime); // UI를 "00:00"으로 리셋
        }
    }
}