using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다!

public class SkipButtonController : MonoBehaviour
{
    // 다음 씬의 빌드 인덱스 또는 이름을 사용합니다.
    // 여기서는 빌드 인덱스(Build Settings에 등록된 순서)를 사용해 볼게요.
    // 0이 첫 번째 씬이므로, 다음 씬은 보통 1 또는 그 이후입니다.
    public int nextSceneBuildIndex = 1; // Inspector에서 값을 바꿔줄 수 있습니다.

    // 버튼 클릭 시 호출될 함수
    public void LoadNextScene()
    {
        // 다음 씬 로드
       SceneManager.LoadScene(nextSceneBuildIndex);

        // 씬 이름으로 로드하는 방법:
       // SceneManager.LoadScene("다음_씬_이름");
    }
}