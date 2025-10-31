using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro�� ����ϱ� ���� �ʿ��մϴ�.
using UnityEngine.SceneManagement; // �� ������ ���� �߰�

public class SceneTimer : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static SceneTimer instance;

    // ������ ã�� �ؽ�Ʈ UI
    // (����: �� ������ DontDestroyOnLoad�� �������� �����Ƿ� ������ ���� ã�ƾ� ��)
    private TextMeshProUGUI timerText;

    // ���� �ٲ� ������ �� ��� �ð�
    private float totalElapsedTime;

    // (����) Ÿ�̸� UI�� �� ������Ʈ�� ���� �̸�
    // ��� ������ �� �̸��� ���� TextMeshPro ������Ʈ�� ã���ϴ�.
    public string timerTextObjectName = "TimerText";


    void Awake()
    {
        // --- �̱��� ���� ���� ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // ���� �ε�� ������ OnSceneLoaded �Լ��� ȣ���ϵ��� �̺�Ʈ�� ���
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϸ� �� ������Ʈ�� �ı�
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        // ������Ʈ�� �ı��� �� �̺�Ʈ���� ����
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // ���� �ε�Ǿ��� �� ȣ��Ǵ� �Լ�
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ������ 'timerTextObjectName' (�⺻�� "TimerText")�� ���� ������Ʈ�� ã���ϴ�.
        GameObject textObject = GameObject.Find(timerTextObjectName);

        if (textObject != null)
        {
            // ������Ʈ�� ã������, TextMeshProUGUI ������Ʈ�� �����ɴϴ�.
            timerText = textObject.GetComponent<TextMeshProUGUI>();

            if (timerText == null)
            {
                Debug.LogWarning($"'{timerTextObjectName}' ������Ʈ�� ã������ TextMeshProUGUI ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            // �� ������ Ÿ�̸� UI�� ���� ���
            timerText = null;
            // Debug.Log($"���� ��({scene.name})�� '{timerTextObjectName}' UI�� �����ϴ�.");
        }
    }

    // Update�� �� �����Ӹ��� ȣ��˴ϴ�.
    void Update()
    {
        // �ð� ����
        totalElapsedTime += Time.deltaTime;

        // timerText�� (���� ����) �Ҵ�Ǿ� ���� ���� UI�� ������Ʈ�մϴ�.
        if (timerText != null)
        {
            // �ð��� �а� �ʷ� ��ȯ
            float minutes = Mathf.FloorToInt(totalElapsedTime / 60);
            float seconds = Mathf.FloorToInt(totalElapsedTime % 60);

            // "00:00" �������� �ؽ�Ʈ UI�� ������Ʈ
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // (���� ����) Ÿ�̸� �ʱ�ȭ �Լ�
    public void ResetTimer()
    {
        totalElapsedTime = 0f;
    }
}