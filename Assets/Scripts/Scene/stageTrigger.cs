using System.Collections;
using System.Collections.Generic;
// StageTrigger.cs

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class StageTrigger : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // 이동할 씬 이름
    public AudioClip triggerClip;              // 트리거 밟을 때 재생할 사운드
    public AudioSource audioSource;            // 할당하거나 null이면 자동 생성
    public float extraWait = 0f;               // 사운드 끝난 후 추가 대기 시간

    bool isTriggered = false;

    void Awake()
    {
        // Collider가 트리거인지 확인
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"[{name}] Collider is not set to 'isTrigger'. Setting it automatically.");
            col.isTrigger = true;
        }

        // 오디오소스 없으면 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D 사운드로(원하면 조정)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        if (!other.CompareTag("Player")) return;

        isTriggered = true;
        StartCoroutine(PlaySoundThenLoad());
    }

    IEnumerator PlaySoundThenLoad()
    {
        // 재생할 클립이 있으면 재생
        if (triggerClip != null)
        {
            audioSource.clip = triggerClip;
            audioSource.Play();
            // audioSource.isPlaying이 true인 동안 기다림
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        else
        {
            Debug.LogWarning("StageTrigger: triggerClip is null. Loading scene immediately.");
        }

        // 추가 대기(옵션)
        if (extraWait > 0f) yield return new WaitForSeconds(extraWait);

        // 씬 로드
        SceneManager.LoadScene(nextSceneName);
    }
}
