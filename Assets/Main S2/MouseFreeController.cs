using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseFreeController : MonoBehaviour
{
    [Tooltip("이 스크립트가 작동할 씬 이름")]
    public string targetSceneName = "MainScene2";

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // 지정된 씬일 경우에만 실행
        if (currentScene == targetSceneName)
        {
            // 마우스 잠금 해제 + 커서 표시
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 혹시 다른 씬에서 일시정지 상태로 넘어왔다면 정상화
            Time.timeScale = 1f;
        }
    }
}
