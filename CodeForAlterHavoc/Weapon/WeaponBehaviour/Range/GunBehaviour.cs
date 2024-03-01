using UnityEngine;

public class GunBehaviour : ProjectileWeaponBehaviour
{    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        transform.position += _Direction * _CurrentSpeed * _PlayerStats.CurrentProjectileSpeed * Time.deltaTime;
    }
}
