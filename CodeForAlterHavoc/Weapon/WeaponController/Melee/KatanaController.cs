using UnityEngine;

public class KatanaController : WeaponController
{
    [SerializeField] float _SwordSize;
    [SerializeField] float _Distance;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject[] enemyPos = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemyPos.Length == 0)
        {
            return;
        }

        float LastDistance = 9999;
        Vector3 position = Vector3.zero;

        for (int i = 0; i < enemyPos.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemyPos[i].transform.position);
            if (LastDistance >= distance)
            {
                LastDistance = distance;
                position = enemyPos[i].transform.position;
            }
        }

        if (LastDistance >= _Distance)
        {
            return;
        }

        GameObject weapon = _Pool.GetObject(_WeaponData.Prefab);
        weapon.transform.position = position;

        MeleeMultiHitBehaviour weaponBehaviour = weapon.GetComponent<MeleeMultiHitBehaviour>();
        weaponBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);
        weaponBehaviour.SetScale(_SwordSize);
        weaponBehaviour.ResetMark();
    }
}
