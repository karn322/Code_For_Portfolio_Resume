using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Vector2 _MoveDirection;

    [HideInInspector] public float lastHorizontalVector;
    [HideInInspector] public float lastVerticalVector;
    [HideInInspector] public Vector2 _LastMoveDirection;

    //Referances
    Rigidbody2D _Rigidbody;
    PlayerStats _PlayerStats;

    void Start()
    {
        _PlayerStats = GetComponent<PlayerStats>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        _LastMoveDirection = new Vector2(1f, 0f); //if not when start weapon not fire
    }

    void Update()
    {
        GetDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetDirection()
    {
        if (GameManager._Instance.StopTime || GameManager._Instance._StopAiming)
        {
            return;
        }
        if (GameManager._Instance._IsGameOver)
            return;

        float X = Input.GetAxisRaw("Horizontal");
        float Y = Input.GetAxisRaw("Vertical");
        _MoveDirection = new Vector2(X, Y).normalized;
        
        if (_MoveDirection.y != 0f)
        {
            lastVerticalVector = _MoveDirection.y;
            _LastMoveDirection = new Vector2(0f, lastVerticalVector);
        }

        if (_MoveDirection.x != 0f)
        {
            lastHorizontalVector = _MoveDirection.x;
            _LastMoveDirection = new Vector2(lastHorizontalVector, 0f);
        }

        if (_MoveDirection.x != 0f && _MoveDirection.y != 0f)
        {
            lastHorizontalVector = _MoveDirection.x;
            lastVerticalVector = _MoveDirection.y;
            _LastMoveDirection = new Vector2(lastHorizontalVector, lastVerticalVector);
        }

        bool isIdle = false;
        bool isLeft = false;

        if( _MoveDirection != Vector2.zero) // check move or not
        {
            isIdle = true;
        }

        if( _LastMoveDirection.x < 0) // check go left or not
        {
            isLeft = true;
        }

        _PlayerStats.ChangeAnimation(isIdle, isLeft);
    }

    private void Move()
    {
        if (GameManager._Instance.StopTime)
        {
            _Rigidbody.velocity = Vector2.zero;
            return;
        }
        if (GameManager._Instance._IsGameOver)
            return;

        _Rigidbody.velocity = new Vector2(_MoveDirection.x * (_PlayerStats.CurrentMoveSpeed + _PlayerStats._SpeedBoost), _MoveDirection.y * (_PlayerStats.CurrentMoveSpeed + _PlayerStats._SpeedBoost));
    }
}
