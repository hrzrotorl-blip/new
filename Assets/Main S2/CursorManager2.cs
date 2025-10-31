using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager2 : MonoBehaviour
{
    public Texture2D cursorDefault;   // �⺻ ȭ��ǥ
    public Texture2D cursorHandOpen;  // �� ���� (Hover)
    public Texture2D cursorHandClosed; // �� �� (�巡�� ��)
    public Vector2 hotSpot = new Vector2(16, 16); // Ŀ�� �߽��� ����
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(cursorDefault, hotSpot, cursorMode);
    }

    public void SetHandOpen()
    {
        Cursor.SetCursor(cursorHandOpen, hotSpot, cursorMode);
    }

    public void SetHandClosed()
    {
        Cursor.SetCursor(cursorHandClosed, hotSpot, cursorMode);
    }
}
