using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float timeRemaining = 15f;   // Ÿ�̸� ���� �ð�
    public Text timerText;              // UI �ؽ�Ʈ ����
    public string nextSceneName;        // ���� �� �̸� (Inspector���� ����)

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
                SceneManager.LoadScene(nextSceneName); // �� ��ȯ
            }
        }
    }

    void UpdateTimerUI(float currentTime)
    {
        currentTime += 1; // 0�� ǥ�� ����
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = seconds.ToString();
    }
}
