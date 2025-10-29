using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro�� ����ϱ� ���� �ʿ��մϴ�.

public class SceneTimer : MonoBehaviour
{
    // �ν����� â���� ������ �ؽ�Ʈ UI
    public TextMeshProUGUI timerText;

    // Update�� �� �����Ӹ��� ȣ��˴ϴ�.
    void Update()
    {
        // timeSinceLevelLoad�� ���� ���� �ε�� �� ����� �ð��� �� ������ ��ȯ�մϴ�.
        float timeInScene = Time.timeSinceLevelLoad;

        // �ð��� �а� �ʷ� ��ȯ�մϴ�.
        float minutes = Mathf.FloorToInt(timeInScene / 60);
        float seconds = Mathf.FloorToInt(timeInScene % 60);

        // "00:00" �������� �ؽ�Ʈ UI�� ������Ʈ�մϴ�.
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}