using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DropRateManager;

public enum EnemyType 
{
    SkillBat,
    Slardar,
    SnakeWarrior,
    Slime,
    FlyingDemon,
    DyingWood,
    SkeletonAxeman,
    SkeletonSoilder,
    InfectedSkeleton,
    BossA,
    BossB,
    BossC
}

[CreateAssetMenu(fileName ="EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemySctiptableObject : ScriptableObject
{
    [SerializeField] EnemyType _EnemyType;
    public EnemyType EnemyType { get => _EnemyType; private set => _EnemyType = value; }
    [SerializeField] float _MoveSpeed;
    public float MoveSpeed { get => _MoveSpeed; private set => _MoveSpeed = value; }
    [SerializeField] float _MaxHealth;
    public float MaxHealth { get => _MaxHealth; private set => _MaxHealth = value; }
    [SerializeField] float _Damage;
    public float Damage { get => _Damage; private set => _Damage = value; }

    [SerializeField] RuntimeAnimatorController _RuntimeAnimatorController;
    public RuntimeAnimatorController RuntimeAnimatorController { get => _RuntimeAnimatorController; private set => _RuntimeAnimatorController = value; }
    [SerializeField] int _ExpAmount;
    public int ExpAmount { get => _ExpAmount; private set => _ExpAmount = value; }
    [SerializeField] Drops[] _DropPrefab;
    public Drops[] DropPrefab { get => _DropPrefab; private set => _DropPrefab = value; }

    [Header("Immune")]
    [SerializeField] bool _KnockbackImmune;
    public bool KnockbackImmune { get => _KnockbackImmune; private set => _KnockbackImmune = value; }
    [SerializeField] bool _StunImmune;
    public bool StunImmune { get => _StunImmune; private set => _StunImmune = value; }

    [Header("Collider")]
    [SerializeField] float _Scale;
    public float Scale { get => _Scale; private set => _Scale = value; }
    [SerializeField] float _OffetX;
    public float OffsetX { get => _OffetX; private set => _OffetX = value;}
    [SerializeField] float _OffetY;
    public float OffsetY { get => _OffetY; private set => _OffetY = value;}
    [SerializeField] float _SizeX;
    public float SizeX { get => _SizeX; private set => _SizeX = value; }
    [SerializeField] float _SizeY;
    public float SizeY { get => _SizeY; private set => _SizeY = value; }

    [Header("Can Fire Bullet")]
    [SerializeField] float _DistanceFromPlayer;
    public float DistanceFromPlayer { get => _DistanceFromPlayer; private set => _DistanceFromPlayer = value; }
    [SerializeField] bool _HaveRangeAttack;
    public bool HaveRangeAttack { get => _HaveRangeAttack; private set => _HaveRangeAttack = value; }
    [SerializeField] GameObject _BulletPrefab;
    public GameObject BulletPrefab { get => _BulletPrefab; private set => _BulletPrefab = value; }
    [SerializeField] float _BulletDamage;
    public float BulletDamage { get => _BulletDamage; private set => _BulletDamage = value; }
    [SerializeField] float _BulletSpeed;
    public float BulletSpeed { get => _BulletSpeed; private set => _BulletSpeed = value; }
    [SerializeField] float _BulletPerAttack;
    public float BulletPerAttack { get => _BulletPerAttack; private set => _BulletPerAttack = value; }
    [SerializeField] float _RangeAttackCooldown;
    public float RangeAttackCooldown { get => _RangeAttackCooldown; private  set => _RangeAttackCooldown = value; }
}
