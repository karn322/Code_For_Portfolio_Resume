using UnityEngine;

public class EnemyMoveToPlayer : MonoBehaviour
{
    EnemyStats _EnemyStats;
    SpriteRenderer _SpriteRenderer;
    Transform _PlayerTarget;

    [HideInInspector] public bool _IsKnockback; 
    [HideInInspector] public bool _IsStun;
    [HideInInspector] public float _EffectTime;

    [HideInInspector] public bool _IsKnockbackImmune;
    [HideInInspector] public bool _IsStunImmune;

    bool _MoveToPlayer = true;

    void Start()
    {
        _EnemyStats = GetComponent<EnemyStats>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _PlayerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (GameManager._Instance.StopTime)
        {
            if (_EffectTime > 0)
            {
                _EffectTime -= Time.deltaTime;

                Vector2 direction = transform.position + (transform.position - _PlayerTarget.position);
                transform.position = Vector2.MoveTowards(transform.position, direction, _EnemyStats._CurrentMoveSpeed * 3 * Time.deltaTime);
            }
            return;
        }

        if (_IsKnockback && !_IsKnockbackImmune)
        {
            Vector2 direction = transform.position + (transform.position - _PlayerTarget.position);
            transform.position = Vector2.MoveTowards(transform.position, direction, _EnemyStats._CurrentMoveSpeed * 3 * Time.deltaTime);

            _EffectTime -= Time.deltaTime;
            if (_EffectTime <= 0)
            {
                _IsKnockback = false;
            }
        }
        else if (_IsStun && !_IsStunImmune)
        {
            _EffectTime -= Time.deltaTime;
            if (_EffectTime <= 0)
            {
                _IsStun = false;
            }
        }
        else
        {
            if (_EnemyStats._EnemyData.HaveRangeAttack) 
            {
                float distance = Vector3.Distance(transform.position, _PlayerTarget.position);

                if (distance > _EnemyStats._EnemyData.DistanceFromPlayer && _MoveToPlayer)
                    transform.position = Vector2.MoveTowards(transform.position, _PlayerTarget.position, _EnemyStats._CurrentMoveSpeed * Time.deltaTime);

                if (distance < _EnemyStats._EnemyData.DistanceFromPlayer - 2)
                    _MoveToPlayer = false;

                if (!_MoveToPlayer)
                {
                    Vector2 direction = transform.position + (transform.position - _PlayerTarget.position);
                    transform.position = Vector2.MoveTowards(transform.position, direction, _EnemyStats._CurrentMoveSpeed * Time.deltaTime);

                    if (distance > _EnemyStats._EnemyData.DistanceFromPlayer + 0.1f)
                        _MoveToPlayer = true;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _PlayerTarget.position, _EnemyStats._CurrentMoveSpeed * Time.deltaTime);
            }
        }

        if (_MoveToPlayer)
        {
            if (transform.position.x > _PlayerTarget.position.x)
            {
                _SpriteRenderer.flipX = true;
            }

            if (transform.position.x < _PlayerTarget.position.x)
            {
                _SpriteRenderer.flipX = false;
            }
        }
        else
        {
            if (transform.position.x > _PlayerTarget.position.x)
            {
                _SpriteRenderer.flipX = false;
            }

            if (transform.position.x < _PlayerTarget.position.x)
            {
                _SpriteRenderer.flipX = true;
            }
        }

        
    }
}
