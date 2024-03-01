using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGravity : MonoBehaviour
{
    private PlayerMovement _Player;
    private Transform _PlayerPosition;
    private float _Distance;
    private bool _StarGravity;
    private Animator _Anim;

    private void Awake()
    {
        _Player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _PlayerPosition = GameObject.Find("Player").GetComponent<Transform>();
        _Anim = GetComponent<Animator>();
        _StarGravity = false;
    }

    void Update()
    {
        _Distance = (transform.position - _PlayerPosition.position).magnitude;

        if (Input.GetKeyDown(KeyCode.E) && _Distance <= 1.5f)
        {
            _Anim.SetTrigger("Click");
            _StarGravity = _Player.IsOnStar();
            if (!_StarGravity)
            {
                _Player.ChangeGravityTrue();
                _StarGravity = true;
            }
            else
            {
                _Player.ChangeGravityFalse();
                _StarGravity = false;
            }
        }
    }
}
