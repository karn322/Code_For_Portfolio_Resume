using System.Collections.Generic;
using UnityEngine;

public class MeleeMultiHitBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> _EnemyMark;
    public bool _IsClearList;
    PlayerStats _PlayerStats;
    SpriteRenderer _SpriteRenderer;

    protected override void Start()
    {
        base.Start();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _EnemyMark = new List<GameObject>();
        _PlayerStats = FindObjectOfType<PlayerStats>();
    }


    protected override void Update()
    {
        base.Update();

        if( _IsClearList)
        {
            _EnemyMark.Clear();
            _IsClearList = false;
        }

        if(_PlayerStats.transform.position.x > transform.position.x)
        {
            _SpriteRenderer.flipX = true;
        }
        
        if(_PlayerStats.transform.position.x < transform.position.x)
        {
            _SpriteRenderer.flipX = false;
        }
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
