using UnityEngine;
// using UnityEngine.SceneManagement; // 1. 네임스페이스 삭제 (요구 사항 1)
using System.Collections; // 코루틴을 사용하기 위해 필요합니다.

/// <summary>
/// 인트로 씬의 애니메이션 시퀀스를 관리합니다.
/// (수정됨: 애니메이션 종료 후 다음 씬 이동 대신 게임 종료)
/// </summary>
public class IntroSceneController : MonoBehaviour
{
    [Tooltip("애니메이션 또는 인트로 대기 시간(초)")]
    public float animationDuration = 15f;

    // 2. nextSceneName 변수 삭제 (요구 사항 2)
    // public string nextSceneName = "MainGameScene"; 

    void Start()
    {
        StartCoroutine(StartIntroSequence());
    }

    // [수정 요구 사항 3, 4, 5]
    /// <summary>
    /// 인트로 시퀀스를 시작하고, 지정된 시간이 지나면 게임을 종료합니다.
    /// </summary>
    IEnumerator StartIntroSequence()
    {
        // 1. 애니메이션 재생 (이전 답변에서 설정 완료)
        // Animator 컴포넌트의 기본 상태로 설정했기 때문에 자동으로 재생됩니다.

        // 2. 지정된 시간(15초)만큼 기다립니다.
        Debug.Log($"인트로 애니메이션 재생 시작. {animationDuration}초 대기합니다.");
        yield return new WaitForSeconds(animationDuration);

        // 3. 게임 종료 (요구 사항 3)
        Debug.Log("애니메이션 종료! 게임을 종료합니다.");

        // 빌드된 게임에서 종료 (요구 사항 4)
        Application.Quit();

        // (유니티 에디터에서 테스트용으로 종료) (요구 사항 5)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}