using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveTo
{
    None,
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
}

public class EnemyMove : MonoBehaviour
{
    EnemyStats _EnemyStats;
    Transform _PlayerTarget;
    SpriteRenderer _SpriteRenderer;
    [HideInInspector] public bool _IsKnockback;
    [HideInInspector] public bool _IsStun;
    [HideInInspector] public float _EffectTime;

    Vector2 _Direction;
    public MoveTo _MoveTo;

    void Start()
    {
        _EnemyStats = GetComponent<EnemyStats>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _PlayerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        switch (_MoveTo)
        {
            case MoveTo.None:
                _Direction = Vector2.zero;
                break;
            case MoveTo.MoveUp:
                _Direction = Vector2.up;
                break;
            case MoveTo.MoveDown:
                _Direction = Vector2.down;
                break;
            case MoveTo.MoveLeft:
                _Direction = Vector2.left;
                break;
            case MoveTo.MoveRight:
                _Direction = Vector2.right;
                break;
        }

        switch (_Direction.x)
        {
            case 0:
                if (transform.position.x > _PlayerTarget.position.x)
                {
                    _SpriteRenderer.flipX = true;
                }
                if (transform.position.x < _PlayerTarget.position.x)
                {
                    _SpriteRenderer.flipX = false;
                }
                break;

            case 1:
                _SpriteRenderer.flipX = false;
                break;

            case -1:
                _SpriteRenderer.flipX = true;
                break;
        }

        if (_IsKnockback)
        {
            Vector2 direction = transform.position + (transform.position - _PlayerTarget.position);
            transform.position = Vector2.MoveTowards(transform.position, direction, _EnemyStats._CurrentMoveSpeed * 3 * Time.deltaTime);

            _EffectTime -= Time.deltaTime;
            if (_EffectTime <= 0)
            {
                _IsKnockback = false;
            }
        }
        else if (_IsStun)
        {
            _EffectTime -= Time.deltaTime;
            if (_EffectTime <= 0)
            {
                _IsStun = false;
            }
        }
        else
        {
            if (_Direction == Vector2.zero)
                return;

            transform.position = Vector2.MoveTowards(transform.position, _Direction + (Vector2)transform.position, _EnemyStats._CurrentMoveSpeed * Time.deltaTime);
        }
    }
}
