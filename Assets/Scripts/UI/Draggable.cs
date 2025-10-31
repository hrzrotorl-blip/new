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
        // reset cursor to default when releasing
        if (cursorManager != null)
            cursorManager.SetDefaultCursor();

        if (!dragging || isPlaced) return;
        dragging = false;
        col.enabled = true;

        // find nearest DropSlot
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
            // snap to slot
            StartCoroutine(SnapToSlotRoutine(best));
        }
        else
        {
            // return to original
            StartCoroutine(ReturnToOriginalRoutine());
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
