using System.Collections;
using UnityEngine;

public class GunMultiBullet : WeaponController
{
    [SerializeField] int _BulletAmount;
    [SerializeField] float _BulletDelay;
    EffectSoundOnCondition _Sound;
    protected override void Start()
    {
        base.Start();
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
            GameObject Bullet = _Pool.GetObject(_WeaponData.Prefab);
            Bullet.transform.position = transform.position; //set to player pos

            GunBehaviour bulletBehavoiur = Bullet.GetComponent<GunBehaviour>();
            bulletBehavoiur.SetStats(_WeaponData, _DestroyAfterSeconds);

            if (GameManager._Instance._IsAiming)
            {
                bulletBehavoiur.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerAim._Direction);
            }
            else
            {
                bulletBehavoiur.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerMovement._LastMoveDirection); //set fire dir
            }

            if (_Sound != null)
                _Sound.PlaySoundEffect();

            yield return new WaitForSeconds(_BulletDelay);
        }
    }
}
