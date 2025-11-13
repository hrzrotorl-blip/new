using UnityEngine;
using UnityEngine.Audio; // AudioMixer 사용을 위해 필수

/// <summary>
/// 이 오브젝트에 있는 모든 AudioSource를 찾아서
/// 지정된 AudioMixer Group으로 출력을 연결해주는 스크립트입니다.
/// (VideoPlayer 없이 순수 오디오용)
/// </summary>
public class AudioMixerConnector : MonoBehaviour
{
    [Header("Audio Mixer Settings")]
    [Tooltip("사운드를 출력할 AudioMixer 그룹을 할당하세요 (예: SFX, Master).")]
    // 기존 코드의 targetMixerGroup 변수 재사용
    public AudioMixerGroup targetMixerGroup;

    void Awake()
    {
        ConnectAudioSources();
    }

    void ConnectAudioSources()
    {
        if (targetMixerGroup == null)
        {
            Debug.LogWarning("[AudioMixerConnector] Target Mixer Group이 할당되지 않았습니다.");
            return;
        }

        // 1. 이 게임 오브젝트에 붙어있는 '모든' AudioSource 컴포넌트를 가져옵니다.
        // (이미지에 Audio Source가 2개 있으므로 GetComponents를 사용합니다)
        AudioSource[] audioSources = GetComponents<AudioSource>();

        // 2. 찾아낸 모든 AudioSource의 출력을 믹서 그룹으로 설정합니다.
        foreach (AudioSource source in audioSources)
        {
            source.outputAudioMixerGroup = targetMixerGroup;
        }
    }
}