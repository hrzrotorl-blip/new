using System.Collections;
using System.Collections.Generic;
// StageTrigger.cs

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class StageTrigger : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // �̵��� �� �̸�
    public AudioClip triggerClip;              // Ʈ���� ���� �� ����� ����
    public AudioSource audioSource;            // �Ҵ��ϰų� null�̸� �ڵ� ����
    public float extraWait = 0f;               // ���� ���� �� �߰� ��� �ð�

    bool isTriggered = false;

    void Awake()
    {
        // Collider�� Ʈ�������� Ȯ��
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"[{name}] Collider is not set to 'isTrigger'. Setting it automatically.");
            col.isTrigger = true;
        }

        // ������ҽ� ������ �߰�
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D �����(���ϸ� ����)
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
        // ����� Ŭ���� ������ ���
        if (triggerClip != null)
        {
            audioSource.clip = triggerClip;
            audioSource.Play();
            // audioSource.isPlaying�� true�� ���� ��ٸ�
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        else
        {
            Debug.LogWarning("StageTrigger: triggerClip is null. Loading scene immediately.");
        }

        // �߰� ���(�ɼ�)
        if (extraWait > 0f) yield return new WaitForSeconds(extraWait);

        // �� �ε�
        SceneManager.LoadScene(nextSceneName);
    }
}
