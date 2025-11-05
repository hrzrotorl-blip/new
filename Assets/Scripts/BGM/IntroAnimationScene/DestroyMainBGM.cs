using UnityEngine;

/// <summary>
/// 이 씬이 로드될 때, DontDestroyOnLoad로 유지되던 'BGMController' 
/// 싱글톤 오브젝트를 찾아 씬에서 파괴합니다.
/// (예: 컷신, 보스 씬 등 메인 BGM이 필요 없는 씬에서 사용)
/// </summary>
public class DestroyMainBGM : MonoBehaviour
{
    // [요구 사항 2] Start() 함수
    void Start()
    {
        // [Start() 함수 로직]
        // 1. BGMController 싱글톤 인스턴스가 씬에 존재하는지 확인합니다.
        if (BGMController.instance != null)
        {
            // 2. 만약 존재한다면, 해당 인스턴스의 게임 오브젝트(gameObject)를 파괴합니다.
            Debug.Log("DestroyMainBGM: 기존 BGMController.instance를 씬에서 파괴합니다.");
            Destroy(BGMController.instance.gameObject);
        }
        else
        {
            // (참고) 씬에 BGMController가 없는 경우
            Debug.Log("DestroyMainBGM: 파괴할 BGMController.instance가 씬에 존재하지 않습니다.");
        }
    }
}