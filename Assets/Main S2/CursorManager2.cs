using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager2 : MonoBehaviour
{
    public Texture2D cursorDefault;   // 기본 화살표
    public Texture2D cursorHandOpen;  // 손 펴짐 (Hover)
    public Texture2D cursorHandClosed; // 손 쥠 (드래그 중)
    public Vector2 hotSpot = new Vector2(16, 16); // 커서 중심점 조절
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
