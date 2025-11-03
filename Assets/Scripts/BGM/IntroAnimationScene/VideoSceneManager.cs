using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요
using UnityEngine.Video;         // VideoPlayer를 사용하기 위해 필요

/// <summary>
/// 이 컴포넌트는 VideoPlayer 컴포넌트가 반드시 필요합니다.
/// 인스펙터에서 자동으로 VideoPlayer 컴포넌트를 추가해줍니다.
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
public class VideoSceneManager : MonoBehaviour
{
    // [요구 사항 1]
    [Header("씬 설정")]
    [Tooltip("비디오 재생이 끝난 후 이동할 씬의 이름")]
    public string nextSceneName; // 인스펙터에서 설정할 다음 씬 이름

    // [요구 사항 2]
    private VideoPlayer videoPlayer; // 이 스크립트와 같은 오브젝트에 있는 VideoPlayer

    // [요구 사항 3] Start() 함수
    void Start()
    {
        // 1. BGMController 싱글톤 인스턴스를 찾아 PauseBGM() 호출
        if (BGMController.instance != null)
        {
            BGMController.instance.PauseBGM();
        }
        else
        {
            // BGMController가 씬에 없는 경우를 대비한 경고
            Debug.LogWarning("BGMController.instance를 찾을 수 없습니다. BGM이 계속 재생될 수 있습니다.");
        }

        // 2. GetComponent로 VideoPlayer 컴포넌트 가져오기
        videoPlayer = GetComponent<VideoPlayer>();

        // 3. 비디오 재생이 끝나면 OnVideoFinished 함수를 호출하도록 이벤트 리스너 등록
        //    (loopPointReached는 비디오가 끝에 도달했을 때 발생하는 이벤트입니다.)
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    // [요구 사항 4] OnVideoFinished(VideoPlayer vp) 함수
    /// <summary>
    /// 비디오 재생이 완료되었을 때 VideoPlayer에 의해 호출되는 콜백 함수입니다.
    /// </summary>
    /// <param name="vp">이벤트를 발생시킨 VideoPlayer 컴포넌트</param>
    private void OnVideoFinished(VideoPlayer vp)
    {
        // (안정성을 위해) 이벤트 리스너를 먼저 제거합니다.
        vp.loopPointReached -= OnVideoFinished;

        // 지정된 다음 씬을 로드합니다.
        Debug.Log(nextSceneName + " 씬을 로드합니다.");
        SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// 이 오브젝트가 파괴될 때(씬이 바뀌는 등) 이벤트 리스너를 확실하게 제거합니다.
    /// </summary>
    void OnDestroy()
    {
        // videoPlayer 변수가 할당된 상태라면(Start가 성공적으로 실행되었다면)
        if (videoPlayer != null)
        {
            // 등록했던 리스너를 제거합니다. (메모리 누수 및 오류 방지)
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}