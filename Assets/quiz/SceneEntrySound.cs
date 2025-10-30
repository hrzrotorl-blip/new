using System.Collections;
using System.Collections.Generic;
// SceneEntrySound.cs
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SceneEntrySound : MonoBehaviour
{
    public AudioClip entryClip;
    public bool playOnStart = true;
    public float delay = 0f; // 씬 로드 후 지연 재생을 원하면 사용

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = entryClip;
    }

    void Start()
    {
        if (playOnStart && entryClip != null)
        {
            if (delay > 0f) Invoke(nameof(Play), delay);
            else Play();
        }
    }

    void Play() => audioSource.Play();
}
