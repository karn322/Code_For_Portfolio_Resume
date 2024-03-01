using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _PlayerPosition;
    private Vector3 _RotationSpeed;
    private Vector3 _Speed;
    private float _PlayerDistance;
    private Vector3 _DefaultPosition;
    private bool _Throwed;
    private bool _Freezed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _PlayerPosition = GameObject.Find("Player").GetComponent<Transform>();
        _Throwed = false;
        _Freezed = false;
        _DefaultPosition = transform.position;
    }

    void Update()
    {
        _PlayerDistance = (transform.position - _PlayerPosition.position).magnitude;

        if (Input.GetKeyDown(KeyCode.Q) && _PlayerDistance <= 1.1f && !_Throwed)
        {
            ThrowingBall();
        }

        if (_Throwed)
        {
            _rb.AddForce(Vector3.Cross(_rb.angularVelocity, _rb.velocity));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = _DefaultPosition;
            _Throwed = false;            
            _Freezed = true;
        }

        if (_Freezed)
        {
            _rb.constraints = RigidbodyConstraints.None;
            _Freezed = false;
        }
    }

    private void ThrowingBall()
    {
        _rb.angularVelocity = _RotationSpeed;
        _rb.velocity = _Speed;
        _Throwed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Can"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = _DefaultPosition;
            _Throwed = false;
            _Freezed = true;
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = _DefaultPosition;
            _Throwed = false;
            _Freezed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OutOfMap"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = _DefaultPosition;
            _Freezed = true;
        }
    }

    public void SetRotationSpeedA()
    {
        _RotationSpeed = new Vector3(0,0.25f,0);
        _Speed = new Vector3(10,5.5f,2);
    }

    public void SetRotationSpeedB()
    {
        _RotationSpeed = new Vector3(0,-0.25f,0);
        _Speed = new Vector3(10,5.5f,-2);
    }

    public void SetRotationSpeedC()
    {
        _RotationSpeed = new Vector3(0,0,0);
        _Speed = new Vector3(10, 5.5f, 0);
    }
}
