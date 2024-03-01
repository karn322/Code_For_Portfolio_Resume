using UnityEngine;
// base script for melee behaviour
public class MeleeWeaponBehaviour : MonoBehaviour
{
    protected float _CurrentDamage;
    protected float _CurrentSpeed;
    protected float _DestroyAfterSeconds;

    protected Vector3 _Direction;
    protected ObjectPool _Pool;
    public bool _IsDestroyAfterDone;

    protected virtual void Start()
    {
        _Pool = FindObjectOfType<ObjectPool>();
    }

    protected virtual void Update()
    {
        if (_IsDestroyAfterDone)
        {
            _DestroyAfterSeconds -= Time.deltaTime;
            if(_DestroyAfterSeconds <= 0)
            {
                _IsDestroyAfterDone = false;
                _Pool.ReturnGameObject(gameObject);
            }
        }
    }

    public void DirectionCheckerAiming(float RotateDir, Vector3 Dir)
    {
        transform.rotation = Quaternion.Euler(0, 0, RotateDir);
        _Direction = new Vector3(Dir.x, Dir.y, 0);
    }

    public float GetCurrentDamange()
    {
        float damage = _CurrentDamage + FindObjectOfType<PlayerStats>()._AttackBoost;
        return damage;
    }

    public void SetStats(WeaponScriptableObject weaponData, float After)
    {
        _CurrentDamage = weaponData.Damage;
        _CurrentSpeed = weaponData.Speed;
        _DestroyAfterSeconds = After;
        _IsDestroyAfterDone = true;
    }

    public void SetScale(float num)
    {
        transform.localScale = new Vector3(num, num, 1);
    }
}
