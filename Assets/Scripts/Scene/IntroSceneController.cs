using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.
using System.Collections; // 코루틴을 사용하기 위해 필요합니다.

public class IntroSceneController : MonoBehaviour
{
    // 애니메이션의 정확한 재생 시간 (15초)을 설정합니다.
    // Inspector 창에서 수정 가능합니다.
    public float animationDuration = 15f;

    // 애니메이션 재생 후 이동할 씬의 이름
    public string nextSceneName = "MainGameScene";

    void Start()
    {
        // 씬이 로드되자마자 인트로 시퀀스를 시작합니다.
        StartCoroutine(StartIntroSequence());
    }

    IEnumerator StartIntroSequence()
    {
        // 1. 애니메이션 재생 (이전 답변에서 설정 완료)
        // Animator 컴포넌트의 기본 상태로 설정했기 때문에 자동으로 재생됩니다.

        // 2. 지정된 시간(15초)만큼 기다립니다.
        Debug.Log($"인트로 애니메이션 재생 시작. {animationDuration}초 대기합니다.");
        yield return new WaitForSeconds(animationDuration);

        // 3. 다음 씬으로 이동
        Debug.Log("애니메이션 종료! 다음 씬으로 이동합니다.");
        SceneManager.LoadScene(nextSceneName);
    }
}