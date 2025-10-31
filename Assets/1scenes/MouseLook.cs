using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    int speed = 5;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺�� ȭ�� �߾ӿ� ����
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime*speed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime*speed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ���Ʒ� ȸ�� ����

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // ī�޶�(���Ʒ�)
        playerBody.Rotate(Vector3.up * mouseX); // �÷��̾� ����(�¿�)
    }
}