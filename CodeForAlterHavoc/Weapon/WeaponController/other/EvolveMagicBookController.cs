using UnityEngine;

public class EvolveMagicBookController : WeaponController
{
    protected override void Start()
    {
        _CurrentCooldown = _WeaponData.CooldownDuration;
        _CurrentCooldown = 0;
        GameObject weapon = Instantiate(_WeaponData.Prefab);
        weapon.transform.position = transform.position; //set to player pos
        weapon.transform.parent = transform; //set parent

        MagicBookBehaviour[] weaponBehaviour = weapon.GetComponentsInChildren<MagicBookBehaviour>();

        for (int i = 0; i < weaponBehaviour.Length; i++)
        {
            weaponBehaviour[i].SetStats(_WeaponData, _DestroyAfterSeconds);
            weaponBehaviour[i]._IsDestroyAfterDone = false;
        }
    }
}
