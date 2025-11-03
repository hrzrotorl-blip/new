using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// 오디오 설정을 관리하고, 슬라이더와 연동하며, PlayerPrefs에 저장합니다.
/// (마스터 볼륨 및 음소거 기능 확장됨)
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("오디오 믹서")]
    public AudioMixer masterMixer;

    [Header("UI 슬라이더")]
    // 1. Master Slider 변수 추가 (요구사항의 논리적 흐름상 필수)
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    // [변수 추가] (SoundToggle.cs에서 가져옴)
    [Header("UI 토글 버튼")]
    public Button muteButton;
    public Image muteIcon;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    // 오디오 믹서 파라미터 이름
    private const string MASTER_VOLUME_PARAM = "MasterVolume"; // 확장
    private const string BGM_VOLUME_PARAM = "BGMVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";

    // PlayerPrefs 키
    private const string MASTER_PREFS_KEY = "MasterVolume"; // 확장
    private const string BGM_PREFS_KEY = "BGMVolume";
    private const string SFX_PREFS_KEY = "SFXVolume";
    private const string MUTE_PREFS_KEY = "IsMuted"; // 확장

    // [내부 변수 추가]
    private bool isMuted = false;
    private float lastMasterVolume = 1f;
    // (참고: BGM, SFX의 마지막 볼륨은 PlayerPrefs에서 직접 읽어오므로 별도 변수 필요 없음)

    void Start()
    {
        // [Start() 함수 수정]
        // 1. 리스너 연결
        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        if (muteButton != null)
            muteButton.onClick.AddListener(ToggleMute); // muteButton 리스너 추가

        // 2. PlayerPrefs에서 "IsMuted" 상태 불러오기
        isMuted = PlayerPrefs.GetInt(MUTE_PREFS_KEY, 0) == 1;

        // 3. PlayerPrefs에서 볼륨 값 불러오기
        // 마스터 볼륨 로드 (음소거 복구에 사용될 'lastMasterVolume' 업데이트)
        lastMasterVolume = PlayerPrefs.GetFloat(MASTER_PREFS_KEY, 1f);
        masterSlider.value = lastMasterVolume;

        // BGM/SFX 볼륨 로드
        float bgmVol = PlayerPrefs.GetFloat(BGM_PREFS_KEY, 0.75f);
        bgmSlider.value = bgmVol;
        float sfxVol = PlayerPrefs.GetFloat(SFX_PREFS_KEY, 0.75f);
        sfxSlider.value = sfxVol;

        // 4. 믹서에 값 적용 (음소거 상태가 아닐 때만)
        // (UpdateMuteUI가 어차피 덮어쓰지만, 초기 로직 명확성을 위해)
        if (!isMuted)
        {
            SetMasterVolume(lastMasterVolume);
            SetBGMVolume(bgmVol);
            SetSFXVolume(sfxVol);
        }

        // 5. [Start() 끝에서] UI 상태 동기화 (가장 마지막에 호출)
        UpdateMuteUI();
    }

    // [신규 함수] 마스터 볼륨 설정
    /// <summary>
    /// 마스터 볼륨을 설정합니다. (슬라이더 이벤트에 연결됨)
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        // 1. 믹서에 적용 (단, 음소거 상태가 아닐 때만)
        if (!isMuted)
        {
            masterMixer.SetFloat(MASTER_VOLUME_PARAM, Mathf.Log10(volume) * 20);
        }

        // 2. 값을 PlayerPrefs에 저장 (음소거 상태여도 값은 저장되어야 함)
        PlayerPrefs.SetFloat(MASTER_PREFS_KEY, volume);

        // 3. 음소거 해제 시 사용할 마지막 볼륨 값 업데이트
        lastMasterVolume = volume;
    }

    // [기존 함수 수정] BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    {
        // 음소거 상태가 아닐 때만 믹서에 적용
        if (!isMuted)
        {
            masterMixer.SetFloat(BGM_VOLUME_PARAM, Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(BGM_PREFS_KEY, volume);
    }

    // [기존 함수 수정] SFX 볼륨 설정
    public void SetSFXVolume(float volume)
    {
        // 음소거 상태가 아닐 때만 믹서에 적용
        if (!isMuted)
        {
            masterMixer.SetFloat(SFX_VOLUME_PARAM, Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat(SFX_PREFS_KEY, volume);
    }


    // [신규 함수] public void ToggleMute()
    /// <summary>
    /// 마스터 볼륨 음소거 상태를 토글합니다. (버튼 onClick 이벤트에 연결됨)
    /// </summary>
    public void ToggleMute()
    {
        // 1. 음소거 상태 반전
        isMuted = !isMuted;

        // 2. 음소거 상태 저장 (1 또는 0)
        PlayerPrefs.SetInt(MUTE_PREFS_KEY, isMuted ? 1 : 0);

        // 3. UI 및 믹서 상태 업데이트
        UpdateMuteUI();
    }

    // [신규 함수] private void UpdateMuteUI()
    /// <summary>
    /// 현재 isMuted 상태에 따라 믹서 볼륨, 아이콘, 슬라이더 상호작용을 업데이트합니다.
    /// </summary>
    private void UpdateMuteUI()
    {
        if (isMuted)
        {
            // [음소거 상태일 때]
            // 1. (요청) 음소거 직전 볼륨 저장 (버그 방지)
            //    -> SetMasterVolume에서 항상 lastMasterVolume을 업데이트하므로 이 시점엔 필요 없음.
            //    (혹은 Start에서 로드한 lastMasterVolume 값을 신뢰)

            // 2. 믹서 마스터 볼륨을 최소로 (즉시 음소거)
            masterMixer.SetFloat(MASTER_VOLUME_PARAM, -80f); // (Mathf.Log10(0.0001f) * 20)과 동일

            // 3. 아이콘 변경
            muteIcon.sprite = soundOffSprite;

            // 4. 모든 슬라이더 비활성화
            masterSlider.interactable = false;
            bgmSlider.interactable = false;
            sfxSlider.interactable = false;
        }
        else
        {
            // [음소거 해제 상태일 때]
            // 1. 저장된 마지막 마스터 볼륨으로 복구
            masterMixer.SetFloat(MASTER_VOLUME_PARAM, Mathf.Log10(lastMasterVolume) * 20);

            // (참고: BGM, SFX는 마스터 볼륨에 종속되므로 별도 복구 필요 없음)

            // 2. 아이콘 변경
            muteIcon.sprite = soundOnSprite;

            // 3. 모든 슬라이더 다시 활성화
            masterSlider.interactable = true;
            bgmSlider.interactable = true;
            sfxSlider.interactable = true;
        }
    }

    void OnDestroy()
    {
        // 리스너 제거 (메모리 누수 방지)
        if (masterSlider != null)
            masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        if (bgmSlider != null)
            bgmSlider.onValueChanged.RemoveListener(SetBGMVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        if (muteButton != null)
            muteButton.onClick.RemoveListener(ToggleMute);
    }
}