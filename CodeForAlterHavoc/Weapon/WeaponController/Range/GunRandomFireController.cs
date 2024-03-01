using UnityEngine;

public class GunRandomFireController : WeaponController
{
    Aiming _Aiming;
    EffectSoundOnCondition _Sound;
    protected override void Start()
    {
        base.Start();
        _Aiming = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Aiming>();
        _Sound = GetComponent<EffectSoundOnCondition>();
    }

    protected override void Attack()
    {
        base.Attack();
        
        GameObject[] _Target = GameObject.FindGameObjectsWithTag("Enemy");

        if (_Target.Length == 0)
        {
            return;               
        }

        _Aiming.GetAim(_Target[Random.Range(0, _Target.Length)].transform.position);

        GameObject Bullet = Instantiate(_WeaponData.Prefab);
        Bullet.transform.position = transform.position; //set to player pos

        GunBehaviour bulletBehavoiur = Bullet.GetComponent<GunBehaviour>();
        bulletBehavoiur.SetStats(_WeaponData, _DestroyAfterSeconds);

        bulletBehavoiur.DirectionCheckerAiming(_Aiming._RotationDirection, _Aiming._Direction);

        if (_Sound != null)
            _Sound.PlaySoundEffect();
    }
}
