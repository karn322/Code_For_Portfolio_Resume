using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemySctiptableObject _EnemyData;

    [HideInInspector] public float _CurrentMoveSpeed;
    [HideInInspector] public float _CurrentHealth;
    [HideInInspector] public float _CurrentDamage;

    public float _DespawnDistance = 20f;
    Transform _Player;
    
    Animator _Animator;

    public GameObject _DamagePopUp;

    EffectSoundOnCondition _EffectSoundOnCondition;
    [SerializeField] WaveStyle _IsWave;
    [SerializeField] float _WaveTime;
    float _TimeLeft;
    bool _IsDrop;

    float _RangeAttackCooldown;
    EnemySpawner _Spawner;

    ObjectPool _Pool;
    DropRateManager _DropRateManager;
    Transform _EnemyTransform;
    BoxCollider2D _BoxCollider;

    [HideInInspector] public bool _IsDead;
    EnemyMoveToPlayer _EnemyMoveToPlayer;

    private void Awake()
    {
        _Player = FindObjectOfType<PlayerStats>().transform;
        _Spawner = FindObjectOfType<EnemySpawner>();
        _Pool = FindObjectOfType<ObjectPool>();
        _Animator = GetComponent<Animator>();
        _EffectSoundOnCondition = GetComponent<EffectSoundOnCondition>();
        _DropRateManager = GetComponent<DropRateManager>();
        _EnemyTransform = GetComponent<Transform>();
        _BoxCollider = GetComponent<BoxCollider2D>();
        _EnemyMoveToPlayer = GetComponent<EnemyMoveToPlayer>();
    }

    private void Start()
    {
        for (int i = 0; i < _EnemyData.DropPrefab.Length; i++)
        {
            if (_EnemyData.DropPrefab[i]._ItemPrefab.TryGetComponent<ExperienceGem>(out ExperienceGem experience))
            {
                experience._ExpAmount = _EnemyData.ExpAmount;
            }
        }
    }
        

    private void Update()
    {
        if (!GameManager._Instance.StopTime)
        {
            if (_IsWave != WaveStyle.None)
            {
                _TimeLeft -= Time.deltaTime;
                if (_TimeLeft <= 0)
                {
                    Kill();
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, _Player.transform.position) > _DespawnDistance)
                {
                    ReturnEnemy();
                }
            }

            if (_EnemyData.HaveRangeAttack)
            {
                _RangeAttackCooldown -= Time.deltaTime;
                if(_RangeAttackCooldown <= 0)
                {
                    StartRangeAttack();
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _CurrentHealth -= damage;
        _Animator.SetTrigger("Hurt");

        GameObject damagePop = _Pool.GetObject(_DamagePopUp);
        damagePop.transform.position = transform.position;
        string damageText = damage.ToString();
        damagePop.GetComponent<TextMeshPro>().text = damageText;
        damagePop.GetComponent<TextMeshPro>().color = Color.yellow;

        if (_CurrentHealth <= 0)
        {
            foreach (var para in _Animator.parameters)
            {
                if(para.name == "Dead")
                {
                    _Animator.SetTrigger("Dead");
                }
            }
            _IsDrop = true;
            Kill();
        }

        _EffectSoundOnCondition.PlaySoundEffect();
    }

    public void Kill()
    {
        if (!_IsDrop)
        {
            _DropRateManager._DontDrop = true;
        }
        if (_IsDrop)
        {
            AddMonsterKill();
            _DropRateManager._DontDrop = false;
        }

        _DropRateManager.DropItem();

        if (_IsWave == WaveStyle.None)
        {
            _Spawner.OnEnemyKilled();
        }
        _IsDead = true;
        _Pool.ReturnGameObject(gameObject);
    }

    private void AddMonsterKill()
    {
        GameManager._Instance._EnemyKillCount++;
        switch (_EnemyData.EnemyType)
        {
            case EnemyType.BossA:
                GameManager._Instance._Boss[0]++;
                break;
            case EnemyType.BossB:
                GameManager._Instance._Boss[1]++;
                break;
            case EnemyType.BossC:
                GameManager._Instance._Boss[2]++;
                break;
            default:
                GameManager._Instance._Monster[(int)_EnemyData.EnemyType]++;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(_CurrentDamage);
        }
    }

    private void ReturnEnemy()
    {
        int rand = Random.Range(0, _Spawner._RelativeSpawnPoints.Count);
        transform.position = _Player.transform.position + _Spawner._RelativeSpawnPoints[rand].position;
    }

    public void SlowDown(bool IsOn)
    {
        if (IsOn)
        {
            if(_CurrentMoveSpeed == _EnemyData.MoveSpeed)
            {
                _CurrentMoveSpeed -= _CurrentMoveSpeed * 15 / 100;                
            }
        }
        else
        {
            if (_CurrentMoveSpeed >= _EnemyData.MoveSpeed)
            {
                return;
            }

            _CurrentMoveSpeed = _EnemyData.MoveSpeed;
        }
    }

    void StartRangeAttack()
    {
        _RangeAttackCooldown = _EnemyData.RangeAttackCooldown;
        StartCoroutine(FireNextBullet());
    }

    private IEnumerator FireNextBullet()
    {
        for (int i = 0; i < _EnemyData.BulletPerAttack; i++)
        {
            GameObject Bullet = _Pool.GetObject(_EnemyData.BulletPrefab);
            Bullet.transform.position = transform.position; //set to player pos
            EnemyBullet enemyBullet = Bullet.GetComponent<EnemyBullet>();
            enemyBullet._CurrentDamage = _EnemyData.BulletDamage;
            enemyBullet._CurrentSpeed = _EnemyData.BulletSpeed;
            enemyBullet._Direction = _Player.position - transform.position;
            enemyBullet._DestroyAfterSeconds = 5;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GetStats(EnemySctiptableObject enemy)
    {
        _EnemyData = enemy;
        _CurrentMoveSpeed = _EnemyData.MoveSpeed;
        _CurrentHealth = _EnemyData.MaxHealth;
        _CurrentDamage = _EnemyData.Damage;

        _Animator.runtimeAnimatorController = _EnemyData.RuntimeAnimatorController;
        _EnemyTransform.localScale = new Vector3(_EnemyData.Scale, _EnemyData.Scale, 1);
        _BoxCollider.offset = new Vector2(_EnemyData.OffsetX,_EnemyData.OffsetY);
        _BoxCollider.size = new Vector2(_EnemyData.SizeX, _EnemyData.SizeY);

        _DropRateManager._Drops.Clear();
        
        for (int i = 0; i < _EnemyData.DropPrefab.Length; i++)
        {
            _DropRateManager._Drops.Add(_EnemyData.DropPrefab[i]);
        }

        _IsDead = false;

        if (_IsWave != WaveStyle.None)
        {
            _TimeLeft = _WaveTime;
        }

        if(_EnemyMoveToPlayer != null)
        {
            _EnemyMoveToPlayer._IsKnockbackImmune = _EnemyData.KnockbackImmune;
            _EnemyMoveToPlayer._IsStunImmune = _EnemyData.StunImmune;
        }
    }
}
