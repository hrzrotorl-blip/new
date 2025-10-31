using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private static BGMController instance;
    private AudioSource audioSource;

    void Awake()
    {
        // �ٸ� ������ ��ȯ�ŵ� ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���
    public void PlayBGM()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    // �Ͻ�����
    public void PauseBGM()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    // ���� ����
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
