using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMouseCurser : MonoBehaviour
{
    [SerializeField] Texture2D _NormalCurser;
    [SerializeField] Texture2D _AimingCurser;
    CursorMode _CursorMode = CursorMode.Auto;

    private void Start()
    {
        ChangeMouseToNormal();
    }

    public void ChangeMouseToNormal()
    {
        Cursor.SetCursor(_NormalCurser, Vector2.zero, _CursorMode);
    }

    public void ChangeMouseToAim()
    {
        Cursor.SetCursor(_AimingCurser, Vector2.zero, _CursorMode);
    }



    public void ChangeMouseCurserIcon(Texture2D Icon)
    {
        Cursor.SetCursor(Icon, Vector2.zero, _CursorMode);
    }
}
