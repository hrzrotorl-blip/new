using UnityEngine;

/// <summary>
/// 이 씬이 로드될 때, 싱글톤 BGMController를 찾아 BGM 재생을 요청합니다.
/// (예: 비디오 씬 이후의 메인 메뉴 씬, 또는 BGM이 필요한 씬의 시작 시점)
/// </summary>
public class ResumeBGMOnLoad : MonoBehaviour
{
    // [요구 사항 1] Start() 함수
    void Start()
    {
        // 1. BGMController 싱글톤 인스턴스를 찾습니다.
        if (BGMController.instance != null)
        {
            // 2. 인스턴스가 존재하면 PlayBGM() 함수를 호출합니다.
            BGMController.instance.PlayBGM();

            // 참고: 만약 함수 이름이 'ResumeBGM'이라면 
            // BGMController.instance.ResumeBGM(); 으로 변경하세요.
        }
        else
        {
            // 3. (null 체크) BGMController.instance를 찾을 수 없는 경우 경고를 출력합니다.
            Debug.LogWarning("BGMController.instance를 찾을 수 없습니다. BGM 재생을 시작할 수 없습니다.");
        }
    }
}