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
    Vector3 dragOffset;
    float fixedY;
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
        if (!isPlaced && cursorManager != null)
            cursorManager.SetHandOpen();
    }

    void OnMouseExit()
    {
        if (!isPlaced && cursorManager != null)
            cursorManager.SetDefaultCursor();
    }

    void OnMouseDown()
    {
        if (isPlaced) return;

        dragging = true;

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
        if (!dragging || isPlaced) return;

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

        if (!dragging || isPlaced) return;
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

    // ✨ 업데이트된 WrongFeedbackAndReturn 코루틴:
    // - feedbackAngle을 사용하여 -45도에서 +45도 사이로 회전하도록 제한합니다.
    // - Quaternion.Slerp를 사용하여 부드러운 회전 보간 및 원래 각도 복귀를 수행합니다.
    IEnumerator WrongFeedbackAndReturn()
    {
        // 대상 피드백 오브젝트 (없으면 자기 자신)
        // 이 오브젝트의 피벗(Pivot) 위치가 회전축이 됩니다.
        Transform target = feedbackObject != null ? feedbackObject : this.transform;

        // 애니메이션 시작 시점의 '로컬' 회전을 저장합니다.
        // 이 회전값이 애니메이션의 '기준값'이 되며, 360도 도는 것을 방지합니다.
        Quaternion start = target.localRotation;

        // 로컬 회전을 기준으로 좌우 feedbackAngle만큼의 목표 회전값을 계산합니다.
        // (0, -45, 0) 회전 * 현재 로컬 회전 => start 회전에서 로컬 Y축을 기준으로 -45도 더 회전
        Quaternion left = Quaternion.Euler(0f, -feedbackAngle, 0f) * start;
        // (0, 45, 0) 회전 * 현재 로컬 회전 => start 회전에서 로컬 Y축을 기준으로 +45도 더 회전
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

        // 마지막에 원래 자리 (월드 위치/회전)로 복귀
        yield return StartCoroutine(ReturnToOriginalRoutine());
    }

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