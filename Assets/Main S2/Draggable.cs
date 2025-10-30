using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour
{
    public int id = 0;                      // �� ������ ��ü�� (0~10)
    public float snapDistance = 1.0f;       // ���԰��� �ִ� ��� �Ÿ� (����)
    public float returnSpeed = 8f;          // ���� ��ġ�� ���ư� �� �ӵ�
    public float snapSpeed = 12f;           // ����(�ڼ�) �ӵ�

    [HideInInspector] public bool isPlaced = false;

    Vector3 originalPosition;
    Quaternion originalRotation;
    Transform originalParent;
    Rigidbody rb;
    Collider col;

    bool dragging = false;
    Vector3 dragOffset;
    float fixedY; // ���ϴ� ���̸� �����Ϸ��� ���

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // ���� ��ġ ����
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent;

        // �⺻���� ���� ���� ���� ���� (�巡�� �ÿ� kinematic ����)
        rb.isKinematic = true;

        // �ʿ��ϸ� ���� ���� (��: �ٴڿ��� �������� �ʰ�)
        fixedY = transform.position.y;
    }

    void OnMouseDown()
    {
        if (isPlaced) return;

        dragging = true;
        // ���콺�� ������Ʈ ���� offset
        Plane plane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            dragOffset = transform.position - hit;
        }

        // �浹ü�� �ٸ� �Ͱ� �������� �ʵ��� �� �� ����
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
            // ���� ����
            target.y = fixedY;
            transform.position = target;
        }
    }

    void OnMouseUp()
    {
        if (!dragging || isPlaced) return;
        dragging = false;
        col.enabled = true;

        // ��� DropSlot�� �˻��ؼ� ���� ����� ������ ã�´�
        DropSlot[] slots = FindObjectsOfType<DropSlot>();
        DropSlot best = null;
        float bestDist = float.MaxValue;
        foreach (var s in slots)
        {
            if (s.isOccupied) continue; // �̹� �������� �ǳʶٱ�
            float d = Vector3.Distance(transform.position, s.GetSnapPosition());
            if (d < bestDist)
            {
                bestDist = d;
                best = s;
            }
        }

        if (best != null && bestDist <= snapDistance && best.id == this.id)
        {
            // �ùٸ� ���Կ� ����� -> ����
            StartCoroutine(SnapToSlotRoutine(best));
        }
        else
        {
            // Ʋ�Ȱų� �ָ� ���� -> ���� �ڸ��� �ǵ�����
            StartCoroutine(ReturnToOriginalRoutine());
        }
    }

    IEnumerator SnapToSlotRoutine(DropSlot slot)
    {
        // �������� �ε巴�� �̵�
        Vector3 targetPos = slot.GetSnapPosition();
        Quaternion targetRot = slot.GetSnapRotation();
        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // ���: ������ ����
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

        // ����: �θ� ��������, ��ȣ�ۿ� ��Ȱ��ȭ
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

    // �ܺο��� ���� ���� ���(�ɼ�)
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
