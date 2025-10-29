using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTrigger : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // �̵��� �� �̸�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾� �±� Ȯ��
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
