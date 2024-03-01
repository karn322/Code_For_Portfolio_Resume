using System.Collections.Generic;
using UnityEngine;

public class MeleeWithStunBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> _EnemyMark;
    [HideInInspector] public bool _IsStun;
    float _EffectTime = 0.5f;

    protected override void Start()
    {
        base.Start();
        _EnemyMark = new List<GameObject>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !_EnemyMark.Contains(collision.gameObject))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamange());

            if (_IsStun && collision.GetComponent<EnemyMoveToPlayer>())
            {
                EnemyMoveToPlayer enemyMovement = collision.GetComponent<EnemyMoveToPlayer>();
                enemyMovement._IsStun = true;
                enemyMovement._EffectTime = _EffectTime;
            }

            _EnemyMark.Add(collision.gameObject);
        }
        else if (collision.CompareTag("Prop") && !_EnemyMark.Contains(collision.gameObject))
        {
            if (collision.gameObject.TryGetComponent(out BreakableProp breakable))
            {
                breakable.TakeDamage(GetCurrentDamange());
                _EnemyMark.Add(collision.gameObject);
            }
        }
    }
    public void ResetMark()
    {
        if (_EnemyMark == null)
            return;
        _EnemyMark.Clear();
    }
}
