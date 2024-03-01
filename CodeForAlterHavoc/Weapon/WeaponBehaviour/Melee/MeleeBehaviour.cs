using System.Collections.Generic;
using UnityEngine;

public class MeleeBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> _EnemyMark;

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
