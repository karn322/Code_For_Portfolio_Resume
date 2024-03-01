using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMouse : MonoBehaviour
{
    private bool _Show;

    private void Start()
    {
        _Show = false;
    }
    void Update()
    {   
        if (!_Show)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _Show = true;
        }        
    }
}
