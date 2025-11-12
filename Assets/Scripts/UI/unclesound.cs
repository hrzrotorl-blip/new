using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    [Header("사운드 설정")]
    public AudioSource audioSource;  // 효과음을 재생할 AudioSource
    public AudioClip triggerSound;   // 트리거 밟을 때 재생할 음향

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 밟았을 때만 작동
        if (other.CompareTag("Player"))
        {
            if (audioSource != null && triggerSound != null)
            {
                audioSource.clip = triggerSound;
                audioSource.Play(); // 🎧 밟을 때마다 소리 재생
            }
            else
            {
                Debug.LogWarning("AudioSource나 AudioClip이 연결되지 않았습니다!");
            }
        }
    }
}

