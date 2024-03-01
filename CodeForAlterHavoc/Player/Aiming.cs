using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class Aiming : MonoBehaviour
{
    [HideInInspector] public float _RotationDirection;
    [HideInInspector] public Vector2 _Direction;
    public GameObject _AimingPoint;
    public void GetAim(Vector3 RawDirection)
    {
        Vector3 direction = new Vector3(RawDirection.x, RawDirection.y, 0) - transform.position;
        _RotationDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _RotationDirection - 90); //just rotate this obj

        _Direction = _AimingPoint.transform.position - transform.position;
    }
}
