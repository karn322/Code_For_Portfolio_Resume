using System.Collections;
using UnityEngine;

public class GunRandomMultiBulletController : WeaponController
{
    Aiming _Aiming;
    [SerializeField] int _BulletAmount;
    [SerializeField] float _BulletDelay;
    EffectSoundOnCondition _Sound;
    protected override void Start()
    {
        base.Start();
        _Aiming = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Aiming>();
        _Sound = GetComponent<EffectSoundOnCondition>();
    }

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(FireNextBullet());
    }

    private IEnumerator FireNextBullet()
    {
        for (int i = 0; i < _BulletAmount; i++)
        {            
            GameObject[] _Target = GameObject.FindGameObjectsWithTag("Enemy");

            if (_Target.Length == 0)
            {
                yield break;
            }

            _Aiming.GetAim(_Target[Random.Range(0, _Target.Length)].transform.position);

            GameObject Bullet = _Pool.GetObject(_WeaponData.Prefab);
            Bullet.transform.position = transform.position; //set to player pos

            GunBehaviour bulletBehavoiur = Bullet.GetComponent<GunBehaviour>();
            bulletBehavoiur.SetStats(_WeaponData, _DestroyAfterSeconds);

            bulletBehavoiur.DirectionCheckerAiming(_Aiming._RotationDirection, _Aiming._Direction);

            if (_Sound != null)
                _Sound.PlaySoundEffect();

            yield return new WaitForSeconds(_BulletDelay);
        }
    }
}
