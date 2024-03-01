using System.Collections.Generic;
using UnityEngine;

public class MagicBookBehaviour : MeleeWeaponBehaviour
{
    [HideInInspector] public List<GameObject> _EnemyMark;
    [SerializeField] float _EffectTime;
    [SerializeField] bool _DontDistroy;
    [SerializeField] bool _Return;
    [SerializeField] GameObject _Parent;

    protected override void Start()
    {
        base.Start();
        _EnemyMark = new List<GameObject>();
    }

    protected override void Update()
    {
        if (_IsDestroyAfterDone)
        {
            _DestroyAfterSeconds -= Time.deltaTime;
            if (_DestroyAfterSeconds <= 0)
            {
                _IsDestroyAfterDone = false;
                if (_Return)
                {
                    _Pool.ReturnGameObject(_Parent);
                }                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !_EnemyMark.Contains(collision.gameObject))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamange());

            if (collision.GetComponent<EnemyMoveToPlayer>())
            {
                EnemyMoveToPlayer enemyMovement = collision.GetComponent<EnemyMoveToPlayer>();
                enemyMovement._IsKnockback = true;
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

    public void RotateThis(float speed)
    {
        transform.Rotate(new Vector3(0, 0, -speed) * Time.deltaTime);
    }

    public void ResetMark()
    {
        if (_EnemyMark == null)
            return;
        _EnemyMark.Clear();
    }
}
