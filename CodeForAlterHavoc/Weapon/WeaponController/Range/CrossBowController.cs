using UnityEngine;

public class CrossBowController : WeaponController
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
        GameObject[] Bullet = new GameObject[3];

        transform.rotation = Quaternion.Euler(0, 0, _PlayerAim._RotationDirection);
        Vector3 offsetY = _OffsetY.transform.position - transform.position;
        Vector3 offsetX = _OffsetX.transform.position - transform.position;

        Bullet[0] = _Pool.GetObject(_WeaponData.Prefab);
        Bullet[0].transform.position = transform.position + offsetX; //set to player pos

        Bullet[1] = _Pool.GetObject(_WeaponData.Prefab);
        Bullet[1].transform.localPosition = transform.position + offsetY;

        Bullet[2] = _Pool.GetObject(_WeaponData.Prefab);
        Bullet[2].transform.localPosition = transform.position - offsetY;

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
