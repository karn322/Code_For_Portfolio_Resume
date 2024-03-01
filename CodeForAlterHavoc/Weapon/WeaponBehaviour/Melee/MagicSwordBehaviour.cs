using System.Collections.Generic;
using UnityEngine;

public class MagicSwordBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> _EnemyMark;
    PlayerStats _PlayerStats;    
    [SerializeField] GameObject _AfterHitEffect;
    float _Timer;
    bool _Clear;

    protected override void Start()
    {
        base.Start();
        _EnemyMark = new List<GameObject>();
        _PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    protected override void Update()
    {
        base.Update();
        if (_DestroyAfterSeconds >= _Timer / 2)
        {
            transform.position += _Direction * _CurrentSpeed * _PlayerStats.CurrentProjectileSpeed * Time.deltaTime;
        }
        else
        {
            if(!_Clear)
            {
                _Clear = true;
                _EnemyMark.Clear();
            }
            transform.position -= _Direction * _CurrentSpeed * _PlayerStats.CurrentProjectileSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !_EnemyMark.Contains(collision.gameObject))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamange());

            GameObject Effect = _Pool.GetObject(_AfterHitEffect);
            Effect.transform.position = collision.transform.position;

            GetComponent<EffectSoundOnCondition>().PlaySoundEffect();

            _EnemyMark.Add(collision.gameObject);
        }
        else if (collision.CompareTag("Prop") && !_EnemyMark.Contains(collision.gameObject))
        {
            if (collision.gameObject.TryGetComponent(out BreakableProp breakable))
            {
                breakable.TakeDamage(GetCurrentDamange());

                GameObject Effect = _Pool.GetObject(_AfterHitEffect);
                Effect.transform.position = collision.transform.position;

                GetComponent<EffectSoundOnCondition>().PlaySoundEffect();

                _EnemyMark.Add(collision.gameObject);
            }
        }
    }
    public void ResetMark()
    {
        _Timer = _DestroyAfterSeconds;
        if (_EnemyMark == null)
            return;
        _EnemyMark.Clear();
    }

}
