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
