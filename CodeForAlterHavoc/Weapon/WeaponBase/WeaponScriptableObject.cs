using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponID
{
    None,

    Sword,
    Rapier,
    LongSword,
    Claymore,
    Katana,
    FlyingDagger,

    FireArm,
    Rifle,
    Bow,
    CrossBow,
    SlingShot,
    FlareGun,

    Aura,
    Pet,
    Book,
    Staff,
    Axe,
    Lance,

    Excaliber,
    MeteoriteClaymore,
    MagicKatana,

    BetterRifle,
    GreatBow,
    GrenadeLauncher,

    AuraPet,
    SuperBook,
    MagicAxe
}

[CreateAssetMenu(fileName ="WeaponScriptableObject",menuName ="ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] GameObject _Prefab;
    public GameObject Prefab { get => _Prefab; private set => _Prefab = value; }
    #region Stats
    //base stats for weapons
    [SerializeField] float _Damage;
    public float Damage { get => _Damage; private set => _Damage = value; }
    [SerializeField] float _Speed;
    public float Speed { get => _Speed; private set => _Speed = value; }
    [SerializeField] float _CooldownDuration;
    public float CooldownDuration { get => _CooldownDuration; private set => _CooldownDuration = value; }
    [SerializeField] bool _Pireceing;
    public bool Pirecing { get => _Pireceing; private set => _Pireceing = value;}
    [SerializeField] int _Level;
    public int Level { get => _Level; private set => _Level = value; }
    [SerializeField] GameObject _NextLevelPrefab;
    public GameObject NextLevelPrefab { get => _NextLevelPrefab; private set => _NextLevelPrefab = value; }
    #endregion

    [SerializeField] string _Name;
    public string Name { get => _Name; private set => _Name = value; }
    [SerializeField] string _Description;
    public string Description { get => _Description; private set => _Description = value; }
    
    [SerializeField] Sprite _Icon;
    public Sprite Icon { get => _Icon; private set => _Icon = value; }
    [SerializeField] WeaponID _WeaponTypeID;
    public WeaponID WeaponTypeID { get => _WeaponTypeID; private set => _WeaponTypeID = value; }
}
