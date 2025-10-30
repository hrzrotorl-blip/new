using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour
{
    public int id = 0;                      // 이 물건의 정체성 (0~10)
    public float snapDistance = 1.0f;       // 슬롯과의 최대 허용 거리 (조정)
    public float returnSpeed = 8f;          // 원래 위치로 돌아갈 때 속도
    public float snapSpeed = 12f;           // 스냅(자석) 속도

    [HideInInspector] public bool isPlaced = false;

    Vector3 originalPosition;
    Quaternion originalRotation;
    Transform originalParent;
    Rigidbody rb;
    Collider col;

    bool dragging = false;
    Vector3 dragOffset;
    float fixedY; // 원하는 높이를 고정하려면 사용

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // 시작 위치 저장
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent;

        // 기본으로 물리 영향 없이 제어 (드래그 시엔 kinematic 유지)
        rb.isKinematic = true;

        // 필요하면 높이 고정 (예: 바닥에서 떨어지지 않게)
        fixedY = transform.position.y;
    }

    void OnMouseDown()
    {
        if (isPlaced) return;

        dragging = true;
        // 마우스와 오브젝트 간의 offset
        Plane plane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            dragOffset = transform.position - hit;
        }

        // 충돌체가 다른 것과 간섭하지 않도록 할 수 있음
        col.enabled = false;
    }

    void OnMouseDrag()
    {
        if (!dragging || isPlaced) return;

        Plane plane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            Vector3 target = hit + dragOffset;
            // 높이 고정
            target.y = fixedY;
            transform.position = target;
        }
    }

    void OnMouseUp()
    {
        if (!dragging || isPlaced) return;
        dragging = false;
        col.enabled = true;

        // 모든 DropSlot을 검사해서 제일 가까운 슬롯을 찾는다
        DropSlot[] slots = FindObjectsOfType<DropSlot>();
        DropSlot best = null;
        float bestDist = float.MaxValue;
        foreach (var s in slots)
        {
            if (s.isOccupied) continue; // 이미 차있으면 건너뛰기
            float d = Vector3.Distance(transform.position, s.GetSnapPosition());
            if (d < bestDist)
            {
                bestDist = d;
                best = s;
            }
        }

        if (best != null && bestDist <= snapDistance && best.id == this.id)
        {
            // 올바른 슬롯에 가까움 -> 스냅
            StartCoroutine(SnapToSlotRoutine(best));
        }
        else
        {
            // 틀렸거나 멀리 있음 -> 원래 자리로 되돌리기
            StartCoroutine(ReturnToOriginalRoutine());
        }
    }

    IEnumerator SnapToSlotRoutine(DropSlot slot)
    {
        // 슬롯으로 부드럽게 이동
        Vector3 targetPos = slot.GetSnapPosition();
        Quaternion targetRot = slot.GetSnapRotation();
        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // 잠금: 슬롯을 점유
        slot.Occupy(this);

        while (t < 1f)
        {
            t += Time.deltaTime * snapSpeed * 0.5f;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        // 고정: 부모를 슬롯으로, 상호작용 비활성화
        transform.SetParent(slot.transform, true);
        isPlaced = true;
        col.enabled = false;
        rb.isKinematic = true;
    }

    IEnumerator ReturnToOriginalRoutine()
    {
        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed * 0.5f;
            transform.position = Vector3.Lerp(startPos, originalPosition, t);
            transform.rotation = Quaternion.Slerp(startRot, originalRotation, t);
            yield return null;
        }
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    // 외부에서 슬롯 점유 취소(옵션)
    public void ResetToOriginal()
    {
        StopAllCoroutines();
        transform.SetParent(originalParent, true);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isPlaced = false;
        col.enabled = true;
    }
}
