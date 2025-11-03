using System.Collections; // 코루틴(IEnumerator)을 사용하기 위해 필요
using UnityEngine;
using UnityEngine.UI;        // Text 컴포넌트를 사용하기 위해 필요
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 필요

/// <summary>
/// 퀴즈용 카운트다운 타이머를 관리합니다.
/// 지정된 시간이 되면 경고색을 표시하고, 0초가 되면 다음 씬을 로드합니다.
/// </summary>
public class QuizTimer : MonoBehaviour
{
    // [Public 변수 선언]
    [Header("타이머 설정")]
    public float timeRemaining = 20f; // 기본값 20초
    public Text timerText;           // 시간을 표시할 UI Text
    public string nextSceneName;     // 0초가 되었을 때 이동할 씬 이름

    [Header("경고 설정")]
    public Color warningColor = Color.red; // 경고 색상
    public float warningTime = 10f;      // 10초 이하일 때 경고

    // [Private 변수 선언]
    private bool isTimerRunning = true; // 타이머가 현재 작동 중인지
    private Color defaultColor;        // 텍스트의 기본 색상

    // [Start() 함수]
    void Start()
    {
        // 1. timerText가 인스펙터에 할당되었는지 확인
        if (timerText != null)
        {
            // 2. 할당되었다면, 텍스트의 현재 색상을 defaultColor에 저장
            defaultColor = timerText.color;
        }
        else
        {
            // 3. timerText가 비어있다면(null) 경고를 출력하고 타이머 중지
            Debug.LogWarning("QuizTimer: 'timerText'가 할당되지 않았습니다. 타이머를 비활성화합니다.");
            isTimerRunning = false;
        }
    }

    // [Update() 함수]
    void Update()
    {
        // 타이머가 실행 중일 때만 Update 로직을 실행
        if (isTimerRunning)
        {
            // 1. 시간이 0초보다 많이 남았을 때
            if (timeRemaining > 0f)
            {
                // 시간을 감소시킴
                timeRemaining -= Time.deltaTime;

                // UI 텍스트 업데이트
                UpdateTimerUI(timeRemaining);

                // [경고 로직]
                // 남은 시간이 warningTime(예: 10초) 이하라면
                if (timeRemaining <= warningTime)
                {
                    timerText.color = warningColor;
                }
                else
                {
                    // 아직 warningTime보다 많다면 기본 색상으로
                    timerText.color = defaultColor;
                }
            }
            // 2. 시간이 0초 이하가 되었고, "isTimerRunning"이 여전히 true일 때
            //    (이 블록은 시간이 0이 된 "첫 번째 프레임"에만 실행됩니다)
            else if (isTimerRunning)
            {
                // [타임아웃 처리]
                // 1. 타이머를 멈춥니다. (Update()의 이 블록이 다시 실행되지 않도록)
                isTimerRunning = false;

                // 2. 시간을 0으로 고정
                timeRemaining = 0f;

                // 3. UI를 "00:00"으로 확실하게 표시
                UpdateTimerUI(timeRemaining);

                // 4. 텍스트 색상을 기본으로 복구
                timerText.color = defaultColor;

                // 5. 씬 전환 코루틴 호출
                StartCoroutine(LoadNextSceneDelayed());
            }
        }
    }

    // [UpdateTimerUI(float currentTime) 함수]
    /// <summary>
    /// 남은 시간을 "00:00" 형식으로 변환하여 timerText에 표시합니다.
    /// </summary>
    /// <param name="currentTime">표시할 시간 (초)</param>
    void UpdateTimerUI(float currentTime)
    {
        // 0초 미만으로 내려가지 않도록 보정
        if (currentTime < 0) currentTime = 0;

        // 분과 초 계산
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // "00:00" 형식(포맷)으로 텍스트 설정
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // [IEnumerator LoadNextSceneDelayed() 함수]
    /// <summary>
    // "00:00" UI를 한 프레임 보여준 뒤, 다음 씬을 로드합니다.
    /// </summary>
    IEnumerator LoadNextSceneDelayed()
    {
        // Update() 함수가 종료된 후, 현재 프레임의 렌더링이 끝날 때까지 대기
        // (즉, 사용자가 "00:00"을 확실히 볼 수 있도록 한 프레임 기다림)
        yield return null;

        Debug.LogWarning("=== QuizTimer가 0초가 되어 씬을 로드합니다! ===");

        // 다음 씬(nextSceneName) 로드
        SceneManager.LoadScene(nextSceneName);
    }
}