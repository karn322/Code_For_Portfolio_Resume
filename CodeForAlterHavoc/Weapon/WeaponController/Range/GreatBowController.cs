using UnityEngine;

public class GreatBowController : WeaponController
{
    [SerializeField] GameObject _OffsetY;
    [SerializeField] GameObject _OffsetX;
    EffectSoundOnCondition _Sound;
    protected override void Start()
    {
        base.Start();
        _Sound = GetComponent<EffectSoundOnCondition>();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject[] Bullet = new GameObject[13];

        transform.rotation = Quaternion.Euler(0, 0, _PlayerAim._RotationDirection);
        Vector3 offsetY = _OffsetY.transform.position - transform.position;
        Vector3 offsetX = _OffsetX.transform.position - transform.position;

        Vector3 X = offsetX * 3;
        for (int i = 0; i < 13; i++)
        {
            Bullet[i] = _Pool.GetObject(_WeaponData.Prefab);
            if(i == 0)
            {
                Bullet[i].transform.position = transform.position + X;
                X -= offsetX;
            }
            else if(i % 2 == 1)
            {
                Bullet[i].transform.localPosition = transform.position + (offsetY * i / 2) + X;
            }
            else if(i % 2 == 0)
            {
                Bullet[i].transform.localPosition = transform.position - (offsetY * i / 2) + X;
                X -= offsetX;
            }
        }

        if (GameManager._Instance._IsAiming)
        {
            for (int i = 0; i < Bullet.Length; i++)
            {
                GunBehaviour bulletBehavoiur = Bullet[i].GetComponent<GunBehaviour>();
                bulletBehavoiur.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerAim._Direction);
                bulletBehavoiur.SetStats(_WeaponData, _DestroyAfterSeconds);
            }
        }
        else
        {
            for (int i = 0; i < Bullet.Length; i++)
            {
                GunBehaviour bulletBehavoiur = Bullet[i].GetComponent<GunBehaviour>(); //set fire dir
                bulletBehavoiur.DirectionCheckerAiming(_PlayerAim._RotationDirection, _PlayerMovement._LastMoveDirection);
                bulletBehavoiur.SetStats(_WeaponData, _DestroyAfterSeconds);
            }
        }

        _Sound.PlaySoundEffect();
    }
}
