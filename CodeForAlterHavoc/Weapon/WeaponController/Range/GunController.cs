using UnityEngine;

public class GunController : WeaponController
{
    [SerializeField] WeaponScriptableObject _BlastDamage;
    EffectSoundOnCondition _Sound;
    protected override void Start()
    {
        base.Start();
        _Sound = GetComponent<EffectSoundOnCondition>();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject Bullet = _Pool.GetObject(_WeaponData.Prefab);
        Bullet.transform.position = transform.position; //set to player pos

        GunBehaviour bulletBehavoiur = Bullet.GetComponent<GunBehaviour>();
        bulletBehavoiur.SetStats(_WeaponData, _DestroyAfterSeconds);

        if (GameManager._Instance._IsAiming)
        {
            bulletBehavoiur.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerAim._Direction);
        }
        else
        {
            bulletBehavoiur.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerMovement._LastMoveDirection); //set fire dir
        }

        if(_BlastDamage != null)
            bulletBehavoiur.HaveBlastDamage(_BlastDamage);

        if (_Sound != null)
            _Sound.PlaySoundEffect();
    }
}
