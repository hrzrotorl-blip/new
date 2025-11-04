using UnityEngine;
using UnityEngine.SceneManagement; // 1. 씬 관리를 위해 네임스페이스 사용

/// <summary>
/// UI 버튼의 OnClick() 이벤트에 연결하여,
/// 인스펙터의 이벤트 필드에 입력한 'sceneName' 문자열로 씬을 로드합니다.
/// 씬의 어떤 'Manager' 오브젝트에 붙여두고 재사용할 수 있습니다.
/// </summary>
public class SceneLoaderButton : MonoBehaviour
{
    // [요구 사항 2, 3]
    /// <summary>
    /// 문자열로 받은 씬 이름(sceneName)을 로드합니다.
    /// 이 함수를 Button의 OnClick() 이벤트에 연결하세요.
    /// </summary>
    /// <param name="sceneName">로드할 씬의 이름 (Build Settings에 포함되어 있어야 함)</param>
    public void LoadSceneByName(string sceneName)
    {
        // (안정성 보강)
        // OnClick 이벤트의 텍스트 필드가 비어있는지 확인합니다.
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneLoaderButton: 로드할 씬 이름(sceneName)이 비어있습니다! " +
                             "버튼 OnClick() 이벤트의 텍스트 필드를 확인하세요.");
            return;
        }

        // 씬 로드 실행
        Debug.Log(sceneName + " 씬을 로드합니다...");
        SceneManager.LoadScene(sceneName);
    }

    /*
    // (참고) 게임 종료 함수
    public void QuitGame()
    {
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
    
    // (참고) 현재 씬을 다시 로드하는 함수
    public void ReloadCurrentScene()
    {
        // 현재 활성화된 씬의 빌드 인덱스를 가져와서 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    */
}