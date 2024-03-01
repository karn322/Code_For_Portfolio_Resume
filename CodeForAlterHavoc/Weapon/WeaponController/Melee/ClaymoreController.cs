using UnityEngine;

public class ClaymoreController : WeaponController
{
    [SerializeField] bool _IsKnockBack;
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

        MeleeWithKnockbackBehaviour weaponBehaviour = weapon.GetComponent<MeleeWithKnockbackBehaviour>();
        weaponBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);
        weaponBehaviour._IsKnockback = _IsKnockBack;
        weaponBehaviour.ResetMark();
        weapon.GetComponent<EffectSoundOnCondition>().PlaySoundEffect();
    }
}
