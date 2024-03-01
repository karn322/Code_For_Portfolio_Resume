using System.Collections;
using UnityEngine;

public class AxeController : WeaponController
{
    Aiming _Aiming;
    [SerializeField] int _BulletAmount;
    [SerializeField] float _BulletDelay;
    protected override void Start()
    {
        base.Start();
        _Aiming = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Aiming>();
    }

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(FireNextAxe());
    }

    private IEnumerator FireNextAxe()
    {
        for (int i = 0; i < _BulletAmount; i++)
        {
            GameObject[] enemyPos = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemyPos.Length == 0)
            {
                yield break;
            }

            float LastDistance = 9999;
            Vector3 position = Vector3.zero;

            for (int j = 0; j < enemyPos.Length; j++)
            {
                float distance = Vector3.Distance(transform.position, enemyPos[j].transform.position);
                if (LastDistance >= distance)
                {
                    LastDistance = distance;
                    position = enemyPos[j].transform.position;
                }
            }

            _Aiming.GetAim(position);

            GameObject Axe = _Pool.GetObject(_WeaponData.Prefab);
            Axe.transform.position = transform.position; 

            if (position.x < 0)
            {
                Axe.GetComponent<SpriteRenderer>().flipX = true;
            }

            GunBehaviour AxeBehaviour = Axe.GetComponent<GunBehaviour>();
            AxeBehaviour.DirectionCheckerAiming(_Aiming._RotationDirection, _Aiming._Direction);
            AxeBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);

            Axe.GetComponent<EffectSoundOnCondition>().PlaySoundEffect();

            yield return new WaitForSeconds(_BulletDelay);
        }
    }
}
