using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float timeRemaining = 15f;   // 타이머 시작 시간
    public Text timerText;              // UI 텍스트 연결
    public string nextSceneName;        // 다음 씬 이름 (Inspector에서 지정)

    private bool isTimerRunning = true;

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                SceneManager.LoadScene(nextSceneName); // 씬 전환
            }
        }
    }

    void UpdateTimerUI(float currentTime)
    {
        currentTime += 1; // 0초 표시 보정
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = seconds.ToString();
    }
}
