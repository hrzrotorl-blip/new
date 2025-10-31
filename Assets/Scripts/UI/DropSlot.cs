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

    public string successMessage = "홍동백서 : 빨간것은 동쪽, 하얀것은 서쪽";
    private UIManager uiManager;

    void Start()
    {
        // 씬에서 UIManager를 자동으로 찾아 연결 (안전장치)
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
    }

    // 마우스로 이 오브젝트(슬롯)를 클릭했을 때 호출됨
    void OnMouseDown()
    {
        // 이것이 "상호작용" 부분입니다.
        // 만약 슬롯이 점유되어 있고 (isOccupied == true)
        // UIManager가 연결되어 있다면
        if (isOccupied && uiManager != null)
        {
            // UIManager에게 메시지 표시를 요청
            uiManager.ShowMessage(successMessage);
        }
    }
    // --- ---

    // (Reset, GetSnapPosition, GetSnapRotation, Occupy, Vacate 함수는 그대로 둡니다)
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

    // (옵션) 슬롯 근처에 들어오는 물건을 자동으로 감지하려면 trigger 콜백을 구현해도 됨.
}
