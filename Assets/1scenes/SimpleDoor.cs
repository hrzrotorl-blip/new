using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDoor : MonoBehaviour
{
    public Transform door;          // �� ������Ʈ
    public float openAngle = 90f;   // ������ ����
    public float speed = 2f;        // ������ �ӵ�
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = door.rotation;
        openRotation = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0));
    }

    private void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        door.rotation = Quaternion.Slerp(door.rotation, targetRotation, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isOpen = false;
    }
}
