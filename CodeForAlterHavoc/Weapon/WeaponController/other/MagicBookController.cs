using UnityEngine;

public class MagicBookController : WeaponController
{
    protected override void Start()
    {
        base.Start();
        _CurrentCooldown = 0;
    }

    protected override void Attack()
    {
        _CurrentCooldown = _WeaponData.CooldownDuration;
        GameObject weapon = _Pool.GetObject(_WeaponData.Prefab);
        weapon.transform.position = transform.position; //set to player pos
        weapon.transform.parent = transform; //set parent

        MagicBookBehaviour[] weaponBehaviour = weapon.GetComponentsInChildren<MagicBookBehaviour>();

        for (int i = 0; i < weaponBehaviour.Length; i++)
        {
            weaponBehaviour[i].SetStats(_WeaponData, _DestroyAfterSeconds);
            weaponBehaviour[i].ResetMark();
        }
    }   
}
