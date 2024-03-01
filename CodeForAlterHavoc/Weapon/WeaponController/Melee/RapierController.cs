using UnityEngine;

public class RapierController : WeaponController
{    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject weapon = _Pool.GetObject(_WeaponData.Prefab);
        weapon.transform.position = transform.position; //set to player pos
        weapon.transform.parent = _Player.transform; //set parent

        MeleeBehaviour weaponBehaviour = weapon.GetComponent<MeleeBehaviour>();
        weaponBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);
        weaponBehaviour.ResetMark();

        if (GameManager._Instance._IsAiming)
        {
            weaponBehaviour.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerAim._Direction);
        }
        else
        {
            weaponBehaviour.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerMovement._LastMoveDirection);
        }

        weapon.GetComponent<EffectSoundOnCondition>().PlaySoundEffect();
    }
    
}
