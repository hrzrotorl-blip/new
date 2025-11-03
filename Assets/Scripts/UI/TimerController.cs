using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요

/// <summary>
/// 카운트다운 타이머를 관리하며, 시간이 0이 되면 다음 씬을 로드합니다.
/// (지정된 시간이 되면 경고 색상으로 변경되는 기능 추가됨)
/// (0초 UI를 확실히 보여주고 씬을 로드하는 기능으로 수정됨)
/// </summary>
public class TimerController : MonoBehaviour
{
    [Header("타이머 설정")]
    public float timeRemaining = 20f; // 남은 시간
    public Text timerText;            // 시간을 표시할 UI Text
    public string nextSceneName;      // 시간이 다 되면 이동할 씬 이름

    [Header("경고 설정")]
    public Color warningColor = Color.red; // 경고를 표시할 색상
    public float warningTime = 10f;        // 경고를 시작할 남은 시간

    private bool isTimerRunning = true;
    private Color defaultColor;         // 텍스트의 원래 색상을 저장할 변수

    // [Start() 함수] - (기존과 동일)
    void Start()
    {
        // 1. timerText가 할당되었는지 확인합니다. (오류 방지)
        if (timerText != null)
        {
            // 2. Start 시점에 텍스트의 원래 색상을 저장합니다.
            defaultColor = timerText.color;
        }
        else
        {
            Debug.LogWarning("TimerController: timerText가 인스펙터에 할당되지 않았습니다.");
            isTimerRunning = false; // 텍스트가 없으면 타이머 중지
        }
    }

    // [Update() 함수 수정됨]
    void Update()
    {
        if (isTimerRunning)
        {
            // 1. 시간이 남아있을 때
            if (timeRemaining > 0f) // (0f로 명시)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);

                // 경고 시간 로직 (기존과 동일)
                if (timeRemaining <= warningTime)
                {
                    timerText.color = warningColor;
                }
                else
                {
                    timerText.color = defaultColor;
                }
            }
            // 2. 시간이 다 되었을 때 (타임아웃)
            // [수정됨] else -> else if (isTimerRunning) : 중복 실행 방지
            else if (isTimerRunning)
            {
                timeRemaining = 0f;
                isTimerRunning = false; // ★★★ 중요: 이 로직이 다시 실행되지 않도록 잠급니다.
                UpdateTimerUI(timeRemaining); // UI를 00:00으로 확실하게 업데이트

                timerText.color = defaultColor; // 색상 복구

                // [수정됨] SceneManager.LoadScene() -> StartCoroutine()
                // 0초 UI를 한 프레임 보여준 후 씬을 로드합니다.
                StartCoroutine(LoadNextSceneDelayed());
            }
        }
    }

    /// <summary>
    /// 시간을 분:초 형식의 문자열로 변환하여 UI Text에 표시합니다.
    /// </summary>
    void UpdateTimerUI(float currentTime)
    {
        // 시간이 0 미만으로 내려가지 않도록 보정
        if (currentTime < 0) currentTime = 0;

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // string.Format을 사용하여 "00:00" 형식으로 표시
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // [새로 추가된 함수]
    /// <summary>
    /// 0초 UI가 화면에 렌더링될 수 있도록 한 프레임 대기한 후, 다음 씬을 로드합니다.
    /// </summary>
    IEnumerator LoadNextSceneDelayed()
    {
        // 한 프레임 대기 (이 프레임 동안 00:00이 화면에 그려짐)
        yield return null;

        // 다음 씬 로드
        SceneManager.LoadScene(nextSceneName);
    }
}