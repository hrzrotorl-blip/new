using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropSlot : MonoBehaviour
{
    public int id = 0;                // 슬롯 아이디 (0~10)
    public Transform snapPoint;       // Inspector에서 할당 (자식 SnapPoint)

    // [수정 요구 사항 1] PuzzleManager 필드 추가
    [Header("Manager Link")]
    [Tooltip("이 슬롯이 속해있는 PuzzleManager (인스펙터에서 수동 할당)")]
    public PuzzleManager myManager;

    [HideInInspector] public bool isOccupied = false; //
    Draggable occupant; //

    void Reset() //
    {
        // Collider는 trigger로 설정 권장
        var col = GetComponent<Collider>(); //
        if (col != null) col.isTrigger = true; //
    }

    public Vector3 GetSnapPosition() //
    {
        if (snapPoint != null) return snapPoint.position; //
        return transform.position; //
    }

    public Quaternion GetSnapRotation() //
    {
        if (snapPoint != null) return snapPoint.rotation; //
        return transform.rotation; //
    }

    public bool Occupy(Draggable d) //
    {
        if (isOccupied) return false; //
        isOccupied = true; //
        occupant = d; //
        return true; //
    }

    public void Vacate() //
    {
        isOccupied = false; //
        occupant = null; //
    }

    /// <summary>
    /// 슬롯에 놓인 오브젝트가 정답인지 판별
    /// </summary>
    // [수정 요구 사항 2] IsCorrect() 메서드는 그대로 유지
    public bool IsCorrect() //
    {
        if (!isOccupied || occupant == null) //
            return false; //

        // occupant가 가진 id가 이 슬롯의 id와 일치하면 정답
        return occupant.id == id; //
    }
}