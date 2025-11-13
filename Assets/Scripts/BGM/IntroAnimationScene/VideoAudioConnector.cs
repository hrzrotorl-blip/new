using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Audio;

[RequireComponent(typeof(VideoPlayer))]
public class VideoAudioConnector : MonoBehaviour
{
    [Header("Audio Mixer Settings")]
    [Tooltip("비디오 소리를 출력할 AudioMixer 그룹을 직접 할당하세요 (예: Master, SFX).")]
    public AudioMixerGroup targetMixerGroup;

    private VideoPlayer _videoPlayer;
    private AudioSource _audioSource;

    void Awake()
    {
        // 1. 컴포넌트 가져오기
        _videoPlayer = GetComponent<VideoPlayer>();
        _audioSource = GetComponent<AudioSource>();

        // AudioSource가 없으면 추가
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false; // 오디오 자체 자동 재생 방지
        }

        // 2. [중요] 기존 재생/초기화 프로세스 강제 중단
        // Inspector에서 'Play On Awake'가 체크되어 있어도 설정을 변경하기 위해 멈춥니다.
        // 비디오 엔진이 이미 'Direct' 모드로 초기화되는 것을 막습니다.
        _videoPlayer.Stop();

        // 3. 오디오 믹서 그룹 연결
        if (targetMixerGroup != null)
        {
            _audioSource.outputAudioMixerGroup = targetMixerGroup;
        }
        else
        {
            Debug.LogWarning("[VideoAudioConnector] Target Mixer Group이 할당되지 않았습니다.");
        }

        // 4. VideoPlayer 출력 모드 강제 변경 (Direct -> AudioSource)
        // 이 설정은 비디오가 Stop 상태이거나 Prepare 전일 때만 안전하게 적용됩니다.
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        // 5. 오디오 트랙 명시적 연결
        // 0번 트랙을 활성화하고 우리가 제어하는 AudioSource로 라우팅합니다.
        _videoPlayer.EnableAudioTrack(0, true);
        _videoPlayer.SetTargetAudioSource(0, _audioSource);

        // 6. 재초기화 및 재생 로직
        // 설정을 변경했으므로 다시 Prepare를 호출해야 합니다.
        _videoPlayer.prepareCompleted -= OnPrepareCompleted; // 중복 등록 방지
        _videoPlayer.prepareCompleted += OnPrepareCompleted;

        _videoPlayer.Prepare(); // 변경된 설정으로 비디오 로드 시작
    }

    /// <summary>
    /// 비디오 준비(Prepare)가 완료되면 호출됩니다.
    /// </summary>
    private void OnPrepareCompleted(VideoPlayer source)
    {
        // 준비가 끝났으므로 비디오와 오디오를 재생합니다.
        source.Play();
    }
}