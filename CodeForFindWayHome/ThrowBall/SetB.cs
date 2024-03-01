using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetB : MonoBehaviour
{
    [SerializeField] private ThrowBall _ThrowBall;
    private Transform _PlayerPosition;
    private float _Distance;
    private Animator _Anim;

    private void Awake()
    {
        _PlayerPosition = GameObject.Find("Player").GetComponent<Transform>();
        _Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _Distance = (transform.position - _PlayerPosition.position).magnitude;

        if (Input.GetKeyDown(KeyCode.E) && _Distance <= 1.1f)
        {
            _Anim.SetTrigger("Click");
            _ThrowBall.SetRotationSpeedB();
        }
    }
}
