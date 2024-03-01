using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookPlayer : MonoBehaviour
{
    [SerializeField] private float _MouseSensitivity = 100f;
    private Transform _PlayerBody;
    private float _XRotation = 0f;
    private PauseMenu _PauseMenu;
    private bool _IsPause;

    private void Awake()
    {
        _PauseMenu = GameObject.Find("HUD").GetComponent<PauseMenu>();
        _PlayerBody = GameObject.Find("Player").GetComponent<Transform>();
        _IsPause = false;
    }

    void Update()
    {
        _IsPause = _PauseMenu.IsPause();

        if (_IsPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (!_IsPause)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            float mouseX = Input.GetAxis("Mouse X") * _MouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _MouseSensitivity * Time.deltaTime;

            _XRotation -= mouseY;
            _XRotation = Mathf.Clamp(_XRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_XRotation, 0f, 0f);
            _PlayerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
