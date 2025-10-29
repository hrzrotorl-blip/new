using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanelButton : MonoBehaviour
{
    public GameObject targetPanel; // ���� ���� �г� ������Ʈ

    private bool isOpen = false;

    public void TogglePanel()
    {
        isOpen = !isOpen;
        targetPanel.SetActive(isOpen);
    }
}
