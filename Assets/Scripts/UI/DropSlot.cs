using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropSlot : MonoBehaviour
{
    public int id = 0;                // 슬롯 아이디 (0~10)
    public Transform snapPoint;       // Inspector에서 할당 (자식 SnapPoint)
    [HideInInspector] public bool isOccupied = false;
    Draggable occupant;

    void Reset()
    {
        // Collider는 trigger로 설정 권장
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

    /// <summary>
    /// 슬롯에 놓인 오브젝트가 정답인지 판별
    /// </summary>
    public bool IsCorrect()
    {
        if (!isOccupied || occupant == null)
            return false;

        // occupant가 가진 id가 이 슬롯의 id와 일치하면 정답
        return occupant.id == id;
    }
}
