using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class TriggerVideoPanel : MonoBehaviour
{
    [Header("패널 & 비디오 설정")]
    public GameObject videoPanel;    // 영상이 나올 Plane
    private VideoPlayer videoPlayer;

    [Header("사운드 설정")]
    public AudioSource audioSource;  // 효과음을 재생할 AudioSource
    public AudioClip triggerSound;   // 트리거 밟을 때 재생할 효과음
    public float soundDelay = 2f;    // 영상 재생 전 대기 시간 (초)

    private Coroutine playCoroutine;

    private void Start()
    {
        if (videoPanel != null)
        {
            videoPlayer = videoPanel.GetComponent<VideoPlayer>();
            videoPanel.SetActive(false); // 처음엔 숨김
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 이미 실행 중인 코루틴이 있으면 중단
            if (playCoroutine != null)
                StopCoroutine(playCoroutine);

            // 코루틴 시작
            playCoroutine = StartCoroutine(PlaySequence());
        }
    }

    private IEnumerator PlaySequence()
    {
        // 1️⃣ 효과음 재생
        if (audioSource != null && triggerSound != null)
        {
            audioSource.clip = triggerSound;
            audioSource.Play();
        }

        // 2️⃣ 지정된 시간 대기
        yield return new WaitForSeconds(soundDelay);

        // 3️⃣ 영상 재생 시작
        if (videoPanel != null)
        {
            videoPanel.SetActive(true);
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
                videoPlayer.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 코루틴 중단 (중복 방지)
            if (playCoroutine != null)
                StopCoroutine(playCoroutine);

            // 영상 정지 & 패널 숨김
            if (videoPanel != null)
            {
                if (videoPlayer != null)
                    videoPlayer.Stop();

                videoPanel.SetActive(false);
            }

            // 효과음도 중단 (원한다면)
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
