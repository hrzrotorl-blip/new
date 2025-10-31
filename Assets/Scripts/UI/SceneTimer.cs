using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요합니다.
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가

public class SceneTimer : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SceneTimer instance;

    // 씬에서 찾을 텍스트 UI
    // (참고: 이 변수는 DontDestroyOnLoad로 유지되지 않으므로 씬마다 새로 찾아야 함)
    private TextMeshProUGUI timerText;

    // 씬이 바뀌어도 유지될 총 경과 시간
    private float totalElapsedTime;

    // (선택) 타이머 UI가 될 오브젝트의 고정 이름
    // 모든 씬에서 이 이름을 가진 TextMeshPro 오브젝트를 찾습니다.
    public string timerTextObjectName = "TimerText";


    void Awake()
    {
        // --- 싱글톤 패턴 구현 ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // 씬이 로드될 때마다 OnSceneLoaded 함수를 호출하도록 이벤트에 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // 이미 인스턴스가 존재하면 이 오브젝트는 파괴
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 이벤트에서 제거
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 씬이 로드되었을 때 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에서 'timerTextObjectName' (기본값 "TimerText")을 가진 오브젝트를 찾습니다.
        GameObject textObject = GameObject.Find(timerTextObjectName);

        if (textObject != null)
        {
            // 오브젝트를 찾았으면, TextMeshProUGUI 컴포넌트를 가져옵니다.
            timerText = textObject.GetComponent<TextMeshProUGUI>();

            if (timerText == null)
            {
                Debug.LogWarning($"'{timerTextObjectName}' 오브젝트를 찾았으나 TextMeshProUGUI 컴포넌트가 없습니다.");
            }
        }
        else
        {
            // 이 씬에는 타이머 UI가 없는 경우
            timerText = null;
            // Debug.Log($"현재 씬({scene.name})에 '{timerTextObjectName}' UI가 없습니다.");
        }
    }

    // Update는 매 프레임마다 호출됩니다.
    void Update()
    {
        // 시간 누적
        totalElapsedTime += Time.deltaTime;

        // timerText가 (현재 씬에) 할당되어 있을 때만 UI를 업데이트합니다.
        if (timerText != null)
        {
            // 시간을 분과 초로 변환
            float minutes = Mathf.FloorToInt(totalElapsedTime / 60);
            float seconds = Mathf.FloorToInt(totalElapsedTime % 60);

            // "00:00" 형식으로 텍스트 UI를 업데이트
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // (선택 사항) 타이머 초기화 함수
    public void ResetTimer()
    {
        totalElapsedTime = 0f;
    }
}