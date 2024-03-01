using UnityEngine;

public class LanceController : WeaponController
{
    SpriteRenderer _SpriteRenderer;
    [SerializeField] float _WeaponSize;
    protected override void Start()
    {
        base.Start();
        _SpriteRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    protected override void Attack()
    {
        _CurrentCooldown = _WeaponData.CooldownDuration;
        GameObject weapon = _Pool.GetObject(_WeaponData.Prefab);
        weapon.transform.parent = _Player.transform;

        if (_SpriteRenderer.flipX)
        {
            weapon.transform.rotation = Quaternion.Euler(0, 0, 180);
            weapon.transform.position = transform.position + new Vector3(-1, 0, 0);
        }
        else
        {
            weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
            weapon.transform.position = transform.position + new Vector3(1, 0, 0);
        }

        MeleeWithKnockbackBehaviour weaponBehaviour = weapon.GetComponent<MeleeWithKnockbackBehaviour>();
        weaponBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);
        weaponBehaviour.SetScale(_WeaponSize);
        weaponBehaviour.ResetMark();
    }
}
