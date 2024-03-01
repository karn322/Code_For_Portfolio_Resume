using UnityEngine;

// Base Script of projectile behaviours [placed on projectile weapon prefab]
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 _Direction;
    public float _DestroyAfterSeconds;

    protected PlayerStats _PlayerStats;

    //current stats
    protected float _CurrentDamage;
    protected float _CurrentSpeed;
    protected bool _IsPiercing;

    public GameObject _AfterHitEffect;

    [SerializeField] bool _Stun;
    [SerializeField] float _EffectTime;

    protected ObjectPool _Pool;
    public bool _IsOn;

    [HideInInspector] WeaponScriptableObject _BlastDamage;

    protected virtual void Start()
    {
        _PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        _Pool = FindObjectOfType<ObjectPool>();
    }

    protected virtual void Update()
    {
        if (_IsOn)
        {
            _DestroyAfterSeconds -= Time.deltaTime;
            if (_DestroyAfterSeconds <= 0)
            {
                _IsOn = false;
                _Pool.ReturnGameObject(gameObject);
            }
        }
    }

    public void DirectionCheckerAiming(float RotateDir, Vector2 Dir)
    {
        transform.rotation = Quaternion.Euler(0,0, RotateDir);
        _Direction = Dir;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamange()); //current for multiply damage
            if (!_IsPiercing)
            {
                PlayEffect();
            }

            if(_Stun && collision.GetComponent<EnemyMoveToPlayer>())
            {
                EnemyMoveToPlayer enemyMovement = collision.GetComponent<EnemyMoveToPlayer>();
                enemyMovement._IsStun = true;
                enemyMovement._EffectTime = _EffectTime;
            }
        }
        else if (collision.CompareTag("Prop"))
        {
            if(collision.gameObject.TryGetComponent(out BreakableProp breakable))
            {
                breakable.TakeDamage(GetCurrentDamange());
                if (!_IsPiercing)
                {
                    PlayEffect();
                }
            }
        }
    }

    public float GetCurrentDamange()
    {
        float damage = _CurrentDamage + FindObjectOfType<PlayerStats>()._AttackBoost;
        return damage;
    }

    public void PlayEffect()
    {
        if (_AfterHitEffect != null)
        {
            GameObject Effect = _Pool.GetObject(_AfterHitEffect);
            Effect.transform.position = transform.position;
            Effect.transform.rotation = transform.rotation;
            if(Effect.TryGetComponent<EffectSoundOnCondition>(out EffectSoundOnCondition sound))
            {
                sound.PlaySoundEffect();
            }

            if(Effect.TryGetComponent<MeleeBehaviour>(out MeleeBehaviour melee))
            {
                melee.SetStats(_BlastDamage, 1);
                melee.ResetMark();
            }
        }
        _Pool.ReturnGameObject(gameObject);
    }

    public void SetStats(WeaponScriptableObject weaponData, float After)
    {
        _CurrentDamage = weaponData.Damage;
        _CurrentSpeed = weaponData.Speed;
        _IsPiercing = weaponData.Pirecing;
        _DestroyAfterSeconds = After;
        _IsOn = true;
    }

    public void HaveBlastDamage(WeaponScriptableObject blast)
    {
        _BlastDamage = blast;
    }
}
