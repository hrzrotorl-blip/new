using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDescription : MonoBehaviour
{
    public GameObject descriptionPanel; // ���� �̹���(�г�)

    // ���� ��ư Ŭ�� �� ����
    public void ShowPanel()
    {
        descriptionPanel.SetActive(true);
    }

}
