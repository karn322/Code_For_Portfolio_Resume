using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharactorID
{
    Ace,
    Lily
}

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField] Sprite _Icon;
    public Sprite Icon { get => _Icon; set => _Icon = value; }
    [SerializeField] Sprite _FullBodyImage;
    public Sprite FullBodyImage { get => _FullBodyImage; set => _FullBodyImage = value; }
    [SerializeField] string _Name;
    public string Name { get => _Name; set => _Name = value; }

    [SerializeField] GameObject _StartingWeapon;
    public GameObject StartingWeapon { get=> _StartingWeapon; set => _StartingWeapon = value; }

    [SerializeField] bool _IsMelee;
    public bool IsMelee { get => _IsMelee; set => _IsMelee = value; }

    #region Character stats
    [SerializeField] float _MaxHealth;
    public float MaxHealth { get => _MaxHealth; private set => _MaxHealth = value; }
    [SerializeField] float _Shield;
    public float Shield { get => _Shield; private set => _Shield = value; }
    [SerializeField] float _ShieldCooldown;
    public float ShieldCooldown { get => _ShieldCooldown; private set => _ShieldCooldown = value;}
    [SerializeField] float _MoveSpeed;
    public float MoveSpeed { get => _MoveSpeed; private set => _MoveSpeed = value; }
    [SerializeField] float _AttackSpeed;
    public float AttackSpeed { get => _AttackSpeed; private set => _AttackSpeed = value; }
    [SerializeField] float _ProjectileSpeed;
    public float ProjectileSpeed { get => _ProjectileSpeed; private set => _ProjectileSpeed = value; }
    [SerializeField] float _MagnetRange;
    public float MagnetRange { get => _MagnetRange; private set => _MagnetRange = value; }
    [SerializeField] RuntimeAnimatorController _RuntimeAnimatorController;
    public RuntimeAnimatorController RuntimeAnimatorController { get => _RuntimeAnimatorController; private set => _RuntimeAnimatorController = value;}

    #endregion
    [SerializeField] CharactorID _CharactorID;
    public CharactorID CharactorID { get => _CharactorID; private set => _CharactorID = value;}
    [SerializeField] int _SkillCooldown;
    public int SkillCooldown { get => _SkillCooldown; private set => _SkillCooldown = value;}
    [SerializeField] int _SkillEffectTimeOut;
    public int SkillEffectTimeOut { get => _SkillEffectTimeOut; private set => _SkillEffectTimeOut = value;}

    [SerializeField] int _SkillDamage;
    public int SkillDamage { get => _SkillDamage; private set => _SkillDamage = value; }
    [SerializeField] int _SkillSpeedMoveBoost;
    public int SkillSpeedMoveBoost { get => _SkillSpeedMoveBoost; private set => _SkillSpeedMoveBoost = value; }
    [SerializeField] int _SkillDamageBoost;
    public int SkillDamageBoost { get => _SkillDamageBoost; private set => _SkillDamageBoost = value;}
    [SerializeField] float _SkillSpeedAttackBoost;
    public float SkillSpeedAttackBoost { get => _SkillSpeedAttackBoost; private set => _SkillSpeedAttackBoost = value; }
    [SerializeField] AnimationClip _SkillEffectTime;
    public AnimationClip SkillEffectTime { get => _SkillEffectTime; private set => _SkillEffectTime = value; }

}
