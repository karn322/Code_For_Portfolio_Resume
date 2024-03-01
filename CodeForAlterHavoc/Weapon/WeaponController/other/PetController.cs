using System.Collections.Generic;
using UnityEngine;

public class PetController : WeaponController
{
    [SerializeField] GameObject _Target;
    Rigidbody2D _Rigidbody;
    bool _EnemyMark;
    [SerializeField] GameObject _AfterHitEffect;
    SpriteRenderer _SpriteRenderer;
    EffectSoundOnCondition _EffectSoundOnCondition;
    [SerializeField] float _Distance;

    protected override void Start()
    {
        base.Start();

        _Rigidbody = GetComponent<Rigidbody2D>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _EffectSoundOnCondition = GetComponent<EffectSoundOnCondition>();
        transform.parent = null;
        Attack();
    }

    protected override void Update()
    {
        base.Update();
        if (_Target == null)
        {
            return;
        }

        Vector2 forceDirection = (_Target.transform.position - transform.position).normalized;

        if (_EnemyMark)
        {
            _Rigidbody.velocity = forceDirection * 0.25f;
        }
        else
        {
            _Rigidbody.velocity = forceDirection * _WeaponData.Speed;
        }

        if (forceDirection.x > 0)
        {
            _SpriteRenderer.flipX = false;
        }

        if (forceDirection.x < 0)
        {
            _SpriteRenderer.flipX = true;
        }
    }

    protected override void Attack()
    {
        _CurrentCooldown = _WeaponData.CooldownDuration;

        _EnemyMark = false;

        if(_Target != null)
        {
            if (!_Target.GetComponent<EnemyStats>()._IsDead)
            {
                return;
            }
        }

        GameObject[] enemyPos = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemyPos.Length == 0)
        {
            return;
        }
        
        List<GameObject> Target = new List<GameObject>();
        for (int i = 0; i < enemyPos.Length; i++)
        {
            float dis = Vector2.Distance(_Player.transform.position, enemyPos[i].transform.position);
            if (_Distance >= dis)
            {               
                Target.Add(enemyPos[i]);
            }
        }

        if(Target.Count == 0)
        {
            return;
        }

        _Target = Target[Random.Range(0, Target.Count)];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _Target && !_EnemyMark)
        {
            GameObject Effect = _Pool.GetObject(_AfterHitEffect);
            Effect.transform.position = collision.transform.position;
            _EnemyMark = true;
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(_WeaponData.Damage + FindObjectOfType<PlayerStats>()._AttackBoost);
            _EffectSoundOnCondition.PlaySoundEffect();
        }
    }
}
