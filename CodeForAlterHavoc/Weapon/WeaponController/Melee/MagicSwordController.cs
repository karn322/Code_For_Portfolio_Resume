using UnityEngine;

public class MagicSwordController : WeaponController
{
    Aiming _Aiming;
    protected override void Start()
    {
        base.Start();
        _Aiming = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Aiming>();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject[] enemyPos = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemyPos.Length == 0)
        {
            return;
        }

        _Aiming.GetAim(enemyPos[Random.Range(0, enemyPos.Length)].transform.position);

        GameObject weapon = _Pool.GetObject(_WeaponData.Prefab);
        weapon.transform.position = transform.position; //set to player pos

        MagicSwordBehaviour weaponBehaviour = weapon.GetComponent<MagicSwordBehaviour>();
        weaponBehaviour.DirectionCheckerAiming(_Aiming._RotationDirection, _Aiming._Direction);
        weaponBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);
        weaponBehaviour.ResetMark();
    }
}
