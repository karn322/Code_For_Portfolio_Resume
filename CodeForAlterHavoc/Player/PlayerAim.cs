using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Camera _MainCamera;
    Vector3 _MousePosition;
    [HideInInspector] public float _RotationDirection;
    [HideInInspector] public Vector2 _Direction;
    public GameObject _AimingPoint;

    PlayerMovement _PM;
    private void Start()
    {
        _MainCamera = Camera.main;
        _PM = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (GameManager._Instance._StopAiming)
        {
            return;
        }

        if(!GameManager._Instance._IsAiming)
        {
            Vector3 dir = new Vector3(_PM._LastMoveDirection.x + _PM.transform.position.x, _PM._LastMoveDirection.y + _PM.transform.position.y , 0) ;
            RotateThisGameoblect(dir);
        }
        else
        {
            _MousePosition = _MainCamera.ScreenToWorldPoint(Input.mousePosition);
            RotateThisGameoblect(_MousePosition);

            _Direction = (_AimingPoint.transform.position - transform.position); //fire direction 
        }
    }

    private void RotateThisGameoblect(Vector3 RawDirection)
    {
        Vector3 direction = new Vector3(RawDirection.x, RawDirection.y, 0) - transform.position;
        _RotationDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _RotationDirection - 90); //just rotate this obj
    }
}
