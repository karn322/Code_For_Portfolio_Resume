using UnityEngine;

// base script for all weapon controller

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject _WeaponData;
    protected float _CurrentCooldown;
    public float _DestroyAfterSeconds = 2f;

    [SerializeField] protected GameObject _Player;
    protected PlayerStats _PlayerStats;
    protected PlayerMovement _PlayerMovement;
    protected PlayerAim _PlayerAim;

    [HideInInspector] public bool _PlayerIsDead;
    protected ObjectPool _Pool;


    protected virtual void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _PlayerStats = _Player.GetComponent<PlayerStats>();
        _PlayerMovement = _Player.GetComponent<PlayerMovement>();
        _PlayerAim = FindObjectOfType<PlayerAim>();
        _CurrentCooldown = _WeaponData.CooldownDuration;
        _Pool = FindObjectOfType<ObjectPool>();
    }

    protected virtual void Update()
    {
        if (GameManager._Instance.StopTime)
            return;

        _CurrentCooldown -= Time.deltaTime;
        if (_CurrentCooldown <= 0)
        {
            if (_PlayerIsDead)
                Destroy(gameObject);

            Attack();
        }
    }

    protected virtual void Attack()
    {
        _CurrentCooldown = _WeaponData.CooldownDuration * _PlayerStats.CurrentAttackSpeed;
    }
}
