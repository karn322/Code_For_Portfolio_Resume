using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGravitation : MonoBehaviour
{
    [SerializeField] private float _Gravity = -9.8f;
    [SerializeField] private float _Time = 50f;
    Vector3 _GravityDirection;
    Vector3 _ObjectDirection;

    public void Attract(Transform Object)
    {
        _GravityDirection = (Object.position - transform.position).normalized;

        _ObjectDirection = Object.up;

        Object.GetComponent<Rigidbody>().AddForce(_GravityDirection * _Gravity);

        Quaternion _ObjectRotation = Quaternion.FromToRotation(_ObjectDirection, _GravityDirection) * Object.rotation;

        Object.rotation = Quaternion.Slerp(Object.rotation, _ObjectRotation, _Time * Time.deltaTime);
    }
}
