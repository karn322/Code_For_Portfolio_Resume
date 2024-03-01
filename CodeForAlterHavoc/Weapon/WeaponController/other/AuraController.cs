using UnityEngine;

public class AuraController : WeaponController
{
    AuraBehaviour _AuraBehaviour;
    [SerializeField] float AuraScale;
    protected override void Start()
    {
        GameObject auraEffect = Instantiate(_WeaponData.Prefab);
        auraEffect.transform.position = transform.position; //set to player pos
        auraEffect.transform.parent = transform; //set parent
        _AuraBehaviour = auraEffect.GetComponent<AuraBehaviour>();
        _AuraBehaviour.SetStats(_WeaponData, _DestroyAfterSeconds);
        _AuraBehaviour._IsDestroyAfterDone = false;
        _AuraBehaviour.SetScale(AuraScale);
        _AuraBehaviour.ClearMark();
    }

    protected override void Attack()
    {
        _CurrentCooldown = _WeaponData.CooldownDuration;
        _AuraBehaviour.ClearMark();
    }

    private void OnDestroy()
    {
        Destroy(_AuraBehaviour.gameObject);
    }
}
