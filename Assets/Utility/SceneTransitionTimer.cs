using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수입니다.

public class SceneTransitionTimer : MonoBehaviour
{
    [Header("타이머 설정")]
    [Tooltip("이 시간이 지나면 씬이 이동됩니다. (초 단위)")]
    public float timeToWait = 60.0f; // 예: 60초

    [Header("이동할 씬 설정")]
    [Tooltip("시간이 다 되었을 때 이동할 씬의 이름")]
    public string sceneToLoad;

    // 내부적으로 경과 시간을 추적하는 변수
    private float currentTime = 0.0f;

    // Update는 매 프레임마다 호출됩니다.
    void Update()
    {
        // 1. 매 프레임마다 시간을 더합니다.
        currentTime += Time.deltaTime;

        // 2. 현재 시간이 설정한 대기 시간(timeToWait)을 초과했는지 확인합니다.
        if (currentTime > timeToWait)
        {
            // 3. 시간이 다 되면, 'sceneToLoad'에 지정된 씬을 로드합니다.

            // 씬 이름이 비어있는지 확인
            if (string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.LogError("SceneTransitionTimer: 'Scene To Load'에 이동할 씬 이름이 설정되지 않았습니다!");
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }

            // 씬이 로드되면 이 스크립트는 멈추므로, 타이머를 다시 0으로 리셋할 필요는 없습니다.
            // (만약 씬을 리셋하는 기능이 필요하다면 enabled = false; 처리를 할 수 있습니다)
        }
    }
}