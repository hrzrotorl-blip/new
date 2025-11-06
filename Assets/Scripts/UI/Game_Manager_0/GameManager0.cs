using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // 코루틴을 위해 필요

/// <summary>
/// 드래그 앤 드롭 퍼즐의 정답을 체크하고,
/// 완료 시 SceneTimer를 중지시킨 후 결과 패널에 최종 시간을 (레거시 UI Text로) 표시합니다.
/// 완료 n초 후 지정된 다음 씬으로 이동합니다.
/// [추가] 다음 씬으로 이동할 때 페이드 아웃 효과를 적용합니다.
/// [수정] 결과 패널이 뜨기 전 딜레이가 추가되었습니다.
/// </summary>
public class GameManager0 : MonoBehaviour
{
    [Header("퍼즐 요소")]
    public DropSlot[] slots;          // 인스펙터에서 슬롯들 연결

    [Header("UI 패널 설정")]
    public GameObject resultPanel;    // 결과창 UI
    public GameObject mainGameUI;     // 인게임 UI (숨길 대상)

    // [요구 사항 1] '결과 패널' 지연 시간 변수 추가
    [Tooltip("모든 슬롯이 맞은 후, 결과 패널이 열리기 전까지의 대기 시간 (초)")]
    public float resultPanelDelay = 1.0f; // 1초 지연

    [Header("결과 텍스트 (Legacy UI Text)")]
    [Tooltip("결과창 내부에 시간을 표시할 레거시 'Text' 컴포넌트")]
    public Text finalTimeText; //

    [Header("씬 설정")]
    [Tooltip("퍼즐 완료 후 로드할 씬의 이름")]
    public string nextSceneName = "NextScene"; //

    [Tooltip("결과창이 표시된 후, 다음 씬으로 넘어가기 전 대기 시간 (초)")]
    public float sceneLoadDelay = 2.0f; //

    // [요청 사항 1] 페이드 효과를 위한 변수 추가
    [Header("페이드 효과 설정")]
    [Tooltip("화면 전체를 덮는 Image UI (Canvas의 맨 위에 위치)")]
    public Image fadePanel; // 페이드 아웃에 사용할 Image 컴포넌트

    [Tooltip("페이드 아웃이 완료되는 데 걸리는 시간 (초)")]
    public float fadeDuration = 1.0f; // 페이드 아웃 지속 시간

    private bool resultShown = false; //

    void Start() //
    {
        if (resultPanel != null) //
            resultPanel.SetActive(false); //

        if (mainGameUI != null) //
            mainGameUI.SetActive(true); //

        if (finalTimeText == null) //
        {
            Debug.LogWarning("GameManager0: 'finalTimeText' (UI.Text)가 인스펙터에 할당되지 않았습니다."); //
        }

        // [요청 사항 2] 페이드 패널 초기화 (완전히 투명하게 만들고 비활성화)
        if (fadePanel != null) //
        {
            Color panelColor = fadePanel.color; //
            panelColor.a = 0f; // 투명하게 시작
            fadePanel.color = panelColor; //
            fadePanel.gameObject.SetActive(false); // 처음에는 비활성화
        }
        else
        {
            Debug.LogWarning("GameManager0: 'Fade Panel' (Image UI)이 인스펙터에 할당되지 않았습니다. 페이드 효과를 사용할 수 없습니다."); //
        }
    }

    void Update() //
    {
        if (!resultShown && AreAllSlotsCorrect()) //
        {
            StartCoroutine(ShowResultAndLoadNextScene()); //
        }
    }

    bool AreAllSlotsCorrect() //
    {
        foreach (var slot in slots) //
        {
            if (!slot.IsCorrect()) //
            {
                return false; //
            }
        }
        Debug.Log("✅ 모든 슬롯이 올바름"); //
        return true; //
    }

    // [요구 사항 2] 코루틴 수정
    private IEnumerator ShowResultAndLoadNextScene() //
    {
        resultShown = true; //

        // [추가된 요구 사항]
        // 결과 패널을 띄우기 전에 'resultPanelDelay' 만큼 대기합니다.
        yield return new WaitForSeconds(resultPanelDelay);

        // (...이후 기존 코드 실행...)
        if (SceneTimer.instance != null) //
        {
            SceneTimer.instance.StopTimer(); //
            string timeString = SceneTimer.instance.GetFormattedTime(); //

            if (finalTimeText != null) //
            {
                finalTimeText.text = "총 걸린 시간: " + timeString; //
            }
        }
        else
        {
            Debug.LogWarning("GameManager0: SceneTimer.instance를 찾을 수 없습니다. 시간 기록을 스킵합니다."); //
        }

        if (resultPanel != null) //
            resultPanel.SetActive(true); //
        if (mainGameUI != null) //
            mainGameUI.SetActive(false); //

        // 결과창 표시 후 sceneLoadDelay 만큼 대기
        yield return new WaitForSeconds(sceneLoadDelay); //

        // [요청 사항 3] 페이드 아웃 코루틴 시작
        if (fadePanel != null) //
        {
            yield return StartCoroutine(FadeOut()); // 페이드 아웃이 끝날 때까지 기다림
        }
        else
        {
            Debug.LogWarning("Fade Panel이 할당되지 않아 페이드 아웃 없이 다음 씬으로 넘어갑니다."); //
        }

        // 다음 씬 로드
        if (!string.IsNullOrEmpty(nextSceneName)) //
        {
            SceneManager.LoadScene(nextSceneName); //
        }
        else
        {
            Debug.LogWarning("GameManager0: 'Next Scene Name'이(가) 인스펙터에 설정되지 않았습니다. 씬을 로드할 수 없습니다."); //
        }
    }

    /// <summary>
    /// 화면을 검정색으로 페이드 아웃시키는 코루틴입니다.
    /// </summary>
    private IEnumerator FadeOut() //
    {
        if (fadePanel == null) yield break; // 패널이 없으면 바로 종료

        fadePanel.gameObject.SetActive(true); // 페이드 패널 활성화
        Color panelColor = fadePanel.color; //
        float timer = 0f; //

        // 투명도(alpha)를 0에서 1로 점차 증가
        while (timer < fadeDuration) //
        {
            timer += Time.deltaTime; //
            panelColor.a = Mathf.Clamp01(timer / fadeDuration); // 0과 1 사이로 클램프
            fadePanel.color = panelColor; //
            yield return null; // 다음 프레임까지 대기
        }

        panelColor.a = 1f; // 완전히 불투명하게 만듦
        fadePanel.color = panelColor; //
    }
}