using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour
{
    public int id = 0;                      // Object identity (0~10)
    public float snapDistance = 1.0f;       // Maximum distance allowed to snap
    public float returnSpeed = 8f;          // Return speed to original position
    public float snapSpeed = 12f;           // Snap (magnet) speed

    [Header("Feedback (Wrong Answer)")]
    [Tooltip("틀렸을 때 좌우로 회전할 오브젝트. 비워두면 이 오브젝트를 사용합니다.")]
    public Transform feedbackObject;
    [Tooltip("틀렸을 때 재생할 오디오 클립")]
    public AudioClip wrongSound;
    [Tooltip("한쪽 회전 각도 (deg)")]
    public float feedbackAngle = 45f; // 👈 좌우 45도로 기본값 수정
    [Tooltip("회전 애니메이션 각 단계 지속 시간 (초)")]
    public float feedbackStepDuration = 0.12f;

    [HideInInspector] public bool isPlaced = false;

    Vector3 originalPosition;
    Quaternion originalRotation;
    Transform originalParent;
    Rigidbody rb;
    Collider col;

    bool dragging = false;
    // 💡 개선 1a: 실제로 마우스가 움직였는지 확인하는 플래그
    bool isActuallyDragging = false;
    // 💡 개선 2a: 피드백 애니메이션이 실행 중인지 확인하는 플래그
    bool isAnimating = false;

    Vector3 dragOffset;
    float fixedY;
    // 💡 개선 1b: 드래그 시작 시의 월드 위치를 저장하여 실제 이동했는지 확인
    Vector3 dragStartPosition;

    CursorManager2 cursorManager;

    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // find cursor manager
        cursorManager = FindObjectOfType<CursorManager2>();

        // save original position
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent;

        // disable physics during drag
        rb.isKinematic = true;

        // fix height
        fixedY = transform.position.y;

        // ensure AudioSource exists (used for wrongSound)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void OnMouseEnter()
    {
        // 💡 개선 2b: 애니메이션 중에는 커서 변경하지 않음
        if (!isPlaced && !isAnimating && cursorManager != null)
            cursorManager.SetHandOpen();
    }

    void OnMouseExit()
    {
        // 💡 개선 2b: 애니메이션 중에는 커서 변경하지 않음
        if (!isPlaced && !isAnimating && cursorManager != null)
            cursorManager.SetDefaultCursor();
    }

    void OnMouseDown()
    {
        // 💡 개선 2c: 애니메이션 중이거나 이미 배치된 경우 클릭 무시
        if (isPlaced || isAnimating) return;

        dragging = true;
        // 💡 개선 1a: 드래그 시작 시점에는 false로 초기화
        isActuallyDragging = false;
        // 💡 개선 1c: 드래그 시작 위치 저장 (클릭/드래그 판단 기준)
        dragStartPosition = transform.position;

        // change cursor to closed hand
        if (cursorManager != null)
            cursorManager.SetHandClosed();

        // calculate offset between object and mouse position
        Plane plane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            dragOffset = transform.position - hit;
        }

        // temporarily disable collider during drag
        col.enabled = false;
    }

    void OnMouseDrag()
    {
        // 💡 개선 2c: 애니메이션 중이거나 이미 배치된 경우 드래그 무시
        if (!dragging || isPlaced || isAnimating) return;

        // 💡 개선 1d: 드래그 시작 위치에서 미세한 이동이 감지되면 isActuallyDragging = true
        if (Vector3.Distance(transform.position, dragStartPosition) > 0.01f)
        {
            isActuallyDragging = true;
        }

        if (cursorManager != null)
            cursorManager.SetHandClosed();

        Plane plane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            Vector3 target = hit + dragOffset;
            target.y = fixedY;
            transform.position = target;
        }
    }

    void OnMouseUp()
    {
        if (cursorManager != null)
            cursorManager.SetDefaultCursor();

        // 💡 개선 2c: 애니메이션 중이거나 이미 배치된 경우 마우스 떼기 무시
        if (!dragging || isPlaced || isAnimating) return;

        // 💡 개선 1e: 실제로 드래그를 하지 않았다면 (단순 클릭) 로직을 종료하고 상태 초기화합니다.
        if (!isActuallyDragging)
        {
            dragging = false;
            col.enabled = true;
            return;
        }

        // --- 이 이후부터는 실제 드래그를 했을 때만 실행됩니다. ---

        dragging = false;
        col.enabled = true;

        DropSlot[] slots = FindObjectsOfType<DropSlot>();
        DropSlot best = null;
        float bestDist = float.MaxValue;
        foreach (var s in slots)
        {
            if (s.isOccupied) continue;
            float d = Vector3.Distance(transform.position, s.GetSnapPosition());
            if (d < bestDist)
            {
                bestDist = d;
                best = s;
            }
        }

        if (best != null && bestDist <= snapDistance && best.id == this.id)
        {
            StartCoroutine(SnapToSlotRoutine(best));
        }
        else
        {
            // 드래그 실패 시에만 피드백 실행
            StartCoroutine(WrongFeedbackAndReturn());
        }
    }

    IEnumerator SnapToSlotRoutine(DropSlot slot)
    {
        Vector3 targetPos = slot.GetSnapPosition();
        Quaternion targetRot = slot.GetSnapRotation();
        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

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

        transform.SetParent(slot.transform, true);
        isPlaced = true;

        if (slot.myManager != null)
        {
            slot.myManager.CheckCompletionState();
        }

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

    IEnumerator WrongFeedbackAndReturn()
    {
        // 💡 개선 2d: 애니메이션 시작 시 플래그 잠금
        isAnimating = true;

        // 대상 피드백 오브젝트 (없으면 자기 자신)
        Transform target = feedbackObject != null ? feedbackObject : this.transform;

        Quaternion start = target.localRotation;
        Quaternion left = Quaternion.Euler(0f, -feedbackAngle, 0f) * start;
        Quaternion right = Quaternion.Euler(0f, feedbackAngle, 0f) * start;

        // 사운드 재생
        if (wrongSound != null && audioSource != null)
            audioSource.PlayOneShot(wrongSound);

        // 1. 왼쪽으로 회전 (start -> left)
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.001f, feedbackStepDuration);
            target.localRotation = Quaternion.Slerp(start, left, t);
            yield return null;
        }

        // 2. 오른쪽으로 회전 (left -> right)
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.001f, feedbackStepDuration);
            target.localRotation = Quaternion.Slerp(left, right, t);
            yield return null;
        }

        // 3. 다시 왼쪽으로 회전 (right -> left)
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.001f, feedbackStepDuration);
            target.localRotation = Quaternion.Slerp(right, left, t);
            yield return null;
        }

        // 4. 원래 각도로 복귀 (left -> start)
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.001f, feedbackStepDuration);
            target.localRotation = Quaternion.Slerp(left, start, t);
            yield return null;
        }

        // 5. 마지막에 원래 자리 (월드 위치/회전)로 복귀
        yield return StartCoroutine(ReturnToOriginalRoutine());

        // 💡 개선 2d: 애니메이션 종료 후 플래그 해제
        isAnimating = false;
    }

    public void ResetToOriginal()
    {
        StopAllCoroutines();
        // 💡 개선 2d: 리셋 시에도 애니메이션 플래그 해제
        isAnimating = false;

        transform.SetParent(originalParent, true);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isPlaced = false;
        col.enabled = true;
    }
}