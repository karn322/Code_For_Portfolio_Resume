using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmeraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _Target;

    private Vector3 _Offset = new Vector3(0, 0, -10);

    private void LateUpdate()
    {
        transform.position = _Target.transform.position + _Offset;
    }
}
