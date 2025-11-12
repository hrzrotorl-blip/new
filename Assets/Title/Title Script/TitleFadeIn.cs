using UnityEngine;
using UnityEngine.UI;     // Image 컴포넌트를 사용하기 위해 필요합니다.
using System.Collections; // 코루틴(IEnumerator)을 사용하기 위해 필요합니다.

/// <summary>
/// 씬이 시작될 때 화면을 밝아지게(Fade-In) 하는 효과를 한 번 실행합니다.
/// (이 스크립트를 페이드 효과용 UI Image에 붙여넣으세요.)
/// </summary>
public class TitleFadeIn : MonoBehaviour
{
    // [요구 사항 1] 페이드 효과를 적용할 UI 이미지
    [SerializeField]
    [Tooltip("화면을 덮는 페이드 효과용 UI Image 컴포넌트")]
    private Image fadeImage;

    // [요구 사항 2] 페이드인에 걸리는 시간
    [SerializeField]
    [Tooltip("페이드인(밝아지기) 효과에 걸리는 시간(초)")]
    private float fadeInDuration = 1.5f;

    // [요구 사항 3] 스크립트가 시작될 때 코루틴 자동 실행
    void Start()
    {
        // 페이드인 효과 코루틴을 시작합니다.
        StartCoroutine(FadeInEffect());
    }

    // [요구 사항 4] FadeInEffect() 코루틴 로직
    /// <summary>
    /// 지정된 시간(fadeInDuration) 동안 이미지를 투명하게 만듭니다. (알파 1 -> 0)
    /// </summary>
    private IEnumerator FadeInEffect()
    {
        // --- [요구 사항 4a] 시작 시 설정 ---
        Color currentColor = fadeImage.color;

        // 1. 시작 시 알파 값을 즉시 1 (완전 불투명)로 설정
        currentColor.a = 1.0f;
        fadeImage.color = currentColor;

        // 2. 페이드인 동안 사용자의 입력(클릭 등)을 막음
        fadeImage.raycastTarget = true;

        // --- [요구 사항 4b] 페이드인 진행 ---
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInDuration)
        {
            // 지난 시간(elapsedTime)을 기준으로 현재 알파 값 계산 (1에서 0으로)
            float alpha = 1.0f - (elapsedTime / fadeInDuration);

            currentColor.a = alpha;
            fadeImage.color = currentColor;

            // 다음 프레임까지 대기
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // --- 페이드인 완료 후 ---

        // 1. 알파 값을 0 (완전 투명)으로 확실하게 설정
        currentColor.a = 0.0f;
        fadeImage.color = currentColor;

        // [요구 사항 4c] 페이드인이 완료되면 입력(클릭)을 허용
        fadeImage.raycastTarget = false;
    }
}