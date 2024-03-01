using UnityEngine;

public class SwordController : WeaponController
{
    [SerializeField] bool _IsStun;
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

        MeleeWithStunBehaviour weaponBehaviour = weapon.GetComponent<MeleeWithStunBehaviour>();
        weaponBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);
        weaponBehaviour._IsStun = _IsStun;
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
