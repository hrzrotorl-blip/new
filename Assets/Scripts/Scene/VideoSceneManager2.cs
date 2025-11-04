using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
// 레거시 Text를 사용하셨다면 이 using은 필요 없습니다.

public class VideoSceneManager2 : MonoBehaviour
{
    // --- Public 변수 ---
    public string nextSceneName = "다음_씬_이름";
    public VideoPlayer videoPlayer;
    public GameObject skipTextObject; // 레거시 Text 오브젝트를 연결한 GameObject

    // --- Private 변수 ---
    private const string VisitedKey = "VideoSceneVisited";
    private bool canSkip = false;

    // Awake()로 변경하여 Start()보다 먼저 실행되도록 합니다.
    void Awake()
    {
        // 1. 씬 방문 횟수 확인 및 스킵 활성화 결정
        if (PlayerPrefs.GetInt(VisitedKey, 0) == 1)
        {
            // 2회차 이상 (스킵 활성화, 텍스트 표시)
            canSkip = true;
            if (skipTextObject != null)
            {
                // 2회차: 텍스트 활성화
                skipTextObject.SetActive(true);
            }
        }
        else
        {
            // 1회차 방문 (스킵 비활성화, 텍스트 숨김)
            canSkip = false;
            if (skipTextObject != null)
            {
                // 1회차: 텍스트 비활성화 (빌드 시 깜빡임 방지)
                skipTextObject.SetActive(false);
            }
        }

        // *주의: videoPlayer 관련 이벤트 등록 및 재생 시작은 Start()에서 해도 무방합니다.
        // 하지만 Awake()에서 같이 처리해도 문제 없습니다.
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    // Start()는 생략하거나 다른 초기화 코드가 필요할 때만 사용합니다.
    void Start()
    {
        // * 여기에 videoPlayer.Play()를 넣을 수 있습니다.
    }

    void Update()
    {
        // 4. 스킵 가능 상태일 때 스페이스바 입력 감지
        if (canSkip && Input.GetKeyDown(KeyCode.Space))
        {
            GoToNextScene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        GoToNextScene();
    }

    void GoToNextScene()
    {
        // 씬을 떠나기 전에 방문 기록 저장
        PlayerPrefs.SetInt(VisitedKey, 1);
        PlayerPrefs.Save();

        // 다음 씬으로 이동
        SceneManager.LoadScene(nextSceneName);
    }
}