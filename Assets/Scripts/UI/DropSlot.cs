using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropSlot : MonoBehaviour
{
    public int id = 0;                // ���� ���̵� (0~10)
    public Transform snapPoint;       // Inspector���� �Ҵ� (�ڽ� SnapPoint)
    [HideInInspector] public bool isOccupied = false;
    Draggable occupant;

    public string successMessage = "ȫ���鼭 : �������� ����, �Ͼ���� ����";
    private UIManager uiManager;

    void Start()
    {
        // ������ UIManager�� �ڵ����� ã�� ���� (������ġ)
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
    }

    // ���콺�� �� ������Ʈ(����)�� Ŭ������ �� ȣ���
    void OnMouseDown()
    {
        // �̰��� "��ȣ�ۿ�" �κ��Դϴ�.
        // ���� ������ �����Ǿ� �ְ� (isOccupied == true)
        // UIManager�� ����Ǿ� �ִٸ�
        if (isOccupied && uiManager != null)
        {
            // UIManager���� �޽��� ǥ�ø� ��û
            uiManager.ShowMessage(successMessage);
        }
    }
    // --- ---

    // (Reset, GetSnapPosition, GetSnapRotation, Occupy, Vacate �Լ��� �״�� �Ӵϴ�)
    void Reset()
    {
        // Collider�� trigger�� ���� ����
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    public Vector3 GetSnapPosition()
    {
        if (snapPoint != null) return snapPoint.position;
        return transform.position;
    }

    public Quaternion GetSnapRotation()
    {
        if (snapPoint != null) return snapPoint.rotation;
        return transform.rotation;
    }

    public bool Occupy(Draggable d)
    {
        if (isOccupied) return false;
        isOccupied = true;
        occupant = d;
        return true;
    }

    public void Vacate()
    {
        isOccupied = false;
        occupant = null;
    }

    // (�ɼ�) ���� ��ó�� ������ ������ �ڵ����� �����Ϸ��� trigger �ݹ��� �����ص� ��.
}
