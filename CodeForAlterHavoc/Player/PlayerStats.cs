using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject _CharacterData;

    //Current stats
    float _CurrentHealth;
    float _CurrentMoveSpeed;
    float _CurrentAttackSpeed;
    float _CurrentProjectileSpeed;
    float _CurrentMagnetRange;
    float _CurrentShield;
    float _MaxHealth;

    #region Current Stats Properties
    public float CurrentHealth 
    { 
        get { return _CurrentHealth; } 
        set 
        { 
            if(_CurrentHealth != value) 
                _CurrentHealth = value;
        } 
    }

    public float CurrentShield
    {
        get { return _CurrentShield; }
        set
        {
            if(_CurrentShield != value)
                _CurrentShield = value;
        }
    }
    public float CurrentMoveSpeed
    {
        get { return _CurrentMoveSpeed; }
        set
        {
            if ( _CurrentMoveSpeed != value)
                _CurrentMoveSpeed = value;
        }
    }
    public float CurrentAttackSpeed
    {
        get { return _CurrentAttackSpeed; }
        set
        {
            if(_CurrentAttackSpeed != value)
                _CurrentAttackSpeed = value;
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return _CurrentProjectileSpeed; }
        set
        {
            if (_CurrentProjectileSpeed != value)
                _CurrentProjectileSpeed = value;
        }
    }
    public float CurrentMagnetRange
    {
        get { return _CurrentMagnetRange; }
        set
        {
            if (_CurrentMagnetRange != value)
                _CurrentMagnetRange = value;
        }
    }
    #endregion

    //CurrentBoost
    [HideInInspector] public float _CurrentExpBoost = 1;
    [HideInInspector] public bool _IsShieldActivate = false;
    bool _IsShield = false;
    float _ShieldCooldownTime;
    
    [HideInInspector] public bool _LastChance = false;

    [HideInInspector] public float _MagnetRangeBoost;
    [HideInInspector] public float _HealthRegenBoost;
    float _HealthRegenTime;

    [HideInInspector] public float _MaxHealthBoost;
    float _LastHPBoost;
    [HideInInspector] public float _SpeedBoost;
    float _LastSpeedBoost;
    [HideInInspector] public float _AttackBoost;
    float _LastAttackBoost;
    InventoryController _InventoryController;

    //Experience and level
    [Header("Experience/Level")]
    public float _Experience = 0;
    public int _Level = 1;
    public int _ExperienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int _StartLevel;
        public int _EndLevel;
        public int _ExperienceCapIncrease;
    }

    public List<LevelRange> _LevelRanges;

    //I-Frames
    [Header("I-Frames")]
    public float _InvincibilityDuration;
    float _InvincibilityTimer;
    bool _IsInvincible;

    WeaponManager _WeaponManager;

    [Header("UI")]
    [SerializeField] Image _HealthBar;
    [SerializeField] Image _ShieldBar;
    [SerializeField] Image _ShieldBG;
    [SerializeField] Image _ExpBar;
    [SerializeField] TMP_Text _LevelText;

    Animator _Animator;
    SpriteRenderer _SpriteRender;

    [SerializeField] GameObject _DamagePopUp;
    EffectSoundOnCondition _EffectSoundOnCondition;
    [SerializeField] AnimationClip _DeadAnimationClip;

    [SerializeField] AnimationClip _ReSpawnAnimationClip;
    float _ReSpawnTime;

    [SerializeField] Animator _LevelUp;
    [SerializeField] AnimationClip _LevelUpClip;

    [SerializeField] CameraMovement _Camera;

    ObjectPool _Pool;
    PlayerSkill _Skill;

    private void Awake()
    {
        _CharacterData = CharacterSelector.GetData();
        CharacterSelector._Instance.DestroySingleton();

        _WeaponManager = GetComponent<WeaponManager>();
        CurrentHealth = _CharacterData.MaxHealth;
        CurrentShield = _CharacterData.Shield;
        CurrentMoveSpeed = _CharacterData.MoveSpeed;
        CurrentAttackSpeed = _CharacterData.AttackSpeed;
        CurrentProjectileSpeed = _CharacterData.ProjectileSpeed;
        CurrentMagnetRange = _CharacterData.MagnetRange;
        _MaxHealth = _CharacterData.MaxHealth;

        //spawn starting weapon
        SpawnWeapon(_CharacterData.StartingWeapon);

        _Animator = GetComponent<Animator>();
        _Animator.runtimeAnimatorController = _CharacterData.RuntimeAnimatorController;
        _SpriteRender = GetComponent<SpriteRenderer>();
        _EffectSoundOnCondition = GetComponent<EffectSoundOnCondition>();
        _Skill = GetComponent<PlayerSkill>();
        _InventoryController = FindObjectOfType<InventoryController>();
        _Pool = FindObjectOfType<ObjectPool>();
    }

    private void Start()
    {
        _ExperienceCap = _LevelRanges[0]._ExperienceCapIncrease;

        GameManager._Instance._CurrentMoveSpeedBoostDisplay.text = "Move Speed Boost : " + _SpeedBoost;
        GameManager._Instance._CurrentAtkBoostDisplay.text = "Attack Boost : " + _AttackBoost;

        GameManager._Instance.AssignChosenCharacterUI(_CharacterData);
        UpdateHealthBar();
        UpdateExp();
        UpdateLevelText(); 
    }

    private void Update()
    {
        if (_InvincibilityTimer > 0)
        {
            _InvincibilityTimer -= Time.deltaTime;
        }
        else if (_IsInvincible)
        {
            _IsInvincible = false;
        }
        GetShield();
        Recover();
        if(_LastHPBoost != _MaxHealthBoost)
        {            
            _LastHPBoost = _MaxHealthBoost;
            UpdateHealthBar();
        }
        if(_LastSpeedBoost != _SpeedBoost)
        {
            _LastSpeedBoost = _SpeedBoost;
            float speedBoostDisplay = _SpeedBoost * 50f;
            GameManager._Instance._CurrentMoveSpeedBoostDisplay.text = "Move Speed Boost : " + speedBoostDisplay + "%";
        }
        if(_LastAttackBoost != _AttackBoost)
        {
            _LastAttackBoost = _AttackBoost;
            GameManager._Instance._CurrentAtkBoostDisplay.text = "Attack Boost : " + _AttackBoost;
        }

        if (_ReSpawnTime > 0)
        {
            _ReSpawnTime -= Time.deltaTime;

            if (_ReSpawnTime <= 0)
            {
                GameManager._Instance.StopTime = false;
                GameManager._Instance._StopAiming = false;
            }
        }
    }
    #region Level and Exp
    public void IncreaseExperience(int amount)
    {
        _Experience += amount * _CurrentExpBoost;

        LevelUpChecker();
        UpdateExp();
    }

    void LevelUpChecker()
    {
        if(_Experience >= _ExperienceCap)
        {
            _Level++;
            _Experience -= _ExperienceCap;

            int experienceCapIncrease = 0;
            foreach(LevelRange range in _LevelRanges)
            {
                if (_Level >= range._StartLevel && _Level <= range._EndLevel)
                {
                    experienceCapIncrease = range._ExperienceCapIncrease;
                    break;
                }
            }
            _ExperienceCap += experienceCapIncrease;

            UpdateLevelText();

            StartCoroutine(LevelUpEffect());
        }
    }

    void UpdateExp()
    {
        _ExpBar.fillAmount = (float) _Experience / _ExperienceCap;
    }

    void UpdateLevelText()
    {
        GameManager._Instance.AssignLevelReach(_Level);
        _LevelText.text = "LV " + _Level.ToString();
    }

    #endregion

    #region Damage and Heal and LastChance
    public void TakeDamage(float damage)
    {
        if (_IsInvincible)
            return;

        _InvincibilityTimer = _InvincibilityDuration;
        _IsInvincible = true;

        _Camera._StartShake = true;

        if (_IsShieldActivate)
        {
            float overDamage = 0;

            if (_IsShield)
            {
                if (CurrentShield < damage)
                {
                    overDamage = damage - CurrentShield;
                }

                CurrentShield -= damage;

                if (overDamage > 0)
                {
                    CurrentShield = 0;
                    _IsShield = false;
                    CurrentHealth -= overDamage;
                }
            }
            else
            {
                CurrentHealth -= damage;
            }

        }
        else
        {
            CurrentHealth -= damage;
        }

        if (CurrentHealth > 0)
        {
            TakeDamageAnimation();
        }
        else
        {
            Kill();
        }

        GameObject damagePop = _Pool.GetObject(_DamagePopUp);
        damagePop.transform.position = transform.position;
        string damageText = damage.ToString();
        damagePop.GetComponent<TextMeshPro>().text = damageText;
        damagePop.GetComponent<TextMeshPro>().color = Color.red;

        UpdateShieldBar();
        UpdateHealthBar();
        _EffectSoundOnCondition.PlaySoundEffect();
    }

    void UpdateHealthBar()
    {
        _MaxHealth = _CharacterData.MaxHealth + _MaxHealthBoost;
        _HealthBar.fillAmount = CurrentHealth / _MaxHealth;
        GameManager._Instance._CurrentHealthDisplay.text = "Health : " + _CurrentHealth + " / " + _MaxHealth;
    }

    public void Kill()
    {
        if (_LastChance)
        {
            CurrentHealth = (_CharacterData.MaxHealth + _MaxHealthBoost) / 2; // get 50% max health
            _InventoryController.UsedLastChanceGem();
            UsingLastChance();
        }
        else
        {
            if (!GameManager._Instance._IsGameOver)
            {
                _InvincibilityTimer = 3;
                _Animator.SetTrigger("Dead");

                GameManager._Instance.GameOver(_DeadAnimationClip.length);
                GameManager._Instance.AssignWeaponUI(_WeaponManager._WeaponsSlot);
                GameManager._Instance._StopAiming = true;
                GameManager._Instance.StopTime = true;

                for (int i = 0; i < GameManager._Instance._Inventory.Length; i++)
                {
                    _InventoryController.DisplayGemOnDead(i);
                }
                _InventoryController.SetGemOnLastSave();

                for (int i = 0; i < _WeaponManager._WeaponsSlot.Count; i++)
                {
                    if (_WeaponManager._WeaponsSlot[i] == null)
                        break;
                    _WeaponManager._WeaponsSlot[i]._PlayerIsDead = true;
                }
            }
        }
    }

    private void UsingLastChance()
    {
        _ReSpawnTime = _ReSpawnAnimationClip.length;
        _InvincibilityTimer += _ReSpawnAnimationClip.length;
        RespawnAnimation();

        GameManager._Instance.StopTime = true;
        GameManager._Instance._StopAiming = true;

        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < allEnemy.Length; i++)
        {
            if (allEnemy[i].TryGetComponent<EnemyMoveToPlayer>(out EnemyMoveToPlayer enemyMove))
            {
                enemyMove._EffectTime = _ReSpawnAnimationClip.length / 4;
            }
        }
    }

    public void RestoreHealth(float health)
    {
        CurrentHealth += health;
        
        if(CurrentHealth >= _CharacterData.MaxHealth + _MaxHealthBoost)
        {
            CurrentHealth = _CharacterData.MaxHealth + _MaxHealthBoost;
        }

        UpdateHealthBar();
    }

    void Recover()
    {
        if(CurrentHealth < _CharacterData.MaxHealth + _MaxHealthBoost)
        {
            if (_HealthRegenBoost <= 0)
                return;

            if (CurrentHealth >= _CharacterData.MaxHealth + _MaxHealthBoost)
            {
                CurrentHealth = _CharacterData.MaxHealth + _MaxHealthBoost;
                return;
            }

            _HealthRegenTime -= Time.deltaTime;

            if (_HealthRegenTime <= 0)
            {
                _HealthRegenTime++; // 1 second
                CurrentHealth += _HealthRegenBoost;
            }

            
        }
                
        UpdateHealthBar();
    }

    void GetShield()
    {
        if(_IsShieldActivate)
        {
            _ShieldBar.enabled = true;
            _ShieldBG.enabled = true;

            if (!_IsShield)
            {
                _ShieldCooldownTime -= Time.deltaTime;
            }

            if (_ShieldCooldownTime <= 0)
            {
                _ShieldCooldownTime = _CharacterData.ShieldCooldown;
                CurrentShield = _CharacterData.Shield;
                _IsShield = true;
            }

        }
        else
        {
            _ShieldBar.enabled = false;
            _ShieldBG.enabled = false;
           // CurrentShield = 0;
        }

        UpdateShieldBar();
    }

    void UpdateShieldBar()
    {
        _ShieldBar.fillAmount = CurrentShield / _CharacterData.Shield;
        if (_IsShieldActivate)
        {
            GameManager._Instance._CurrentShield.text = "Shield : " + CurrentShield + " / " + _CharacterData.Shield;
        }
        else
        {
            GameManager._Instance._CurrentShield.text = "Shield : " + CurrentShield + " / 0";
        }        
    }
    #endregion

    #region animation
    public void ChangeAnimation(bool idle, bool IsLeft)
    {
        _Animator.SetBool("Walk", idle);
        if(IsLeft)
        {
            _SpriteRender.flipX = true;
        }
        else
        {
            _SpriteRender.flipX = false;
        }
    }

    private void TakeDamageAnimation()
    {
        _Animator.SetTrigger("Hurt");
    }

    private void RespawnAnimation()
    {
        _Animator.SetTrigger("Respawn");
    }

    public IEnumerator UsedSkill()
    {
        GameManager._Instance.StopTime = true;
        _Animator.SetBool("Skill", true);
        _Skill.AttackAnimation();
        yield return new WaitForSeconds(_CharacterData.SkillEffectTime.length / 2);
        _Skill.AttackAllEnemy();
        yield return new WaitForSeconds(_CharacterData.SkillEffectTime.length / 2);

        GameManager._Instance.StopTime = false;
    }

    IEnumerator LevelUpEffect()
    {
        GameManager._Instance.StopTime = true;
        Time.timeScale = 0;
        _LevelUp.SetTrigger("LevelUp");
        yield return WaitForRealSeconds(_LevelUpClip.length);

        GameManager._Instance.StartLevelUp();
        GameManager._Instance.StopTime = false;
    }

    IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
    #endregion

    public void SpawnWeapon(GameObject weapon)
    {
        for (int i = 0; i < _WeaponManager._WeaponsSlot.Count; i++)
        {
            if (_WeaponManager._WeaponsSlot[i] == null)
            {
                GameObject spawnWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
                spawnWeapon.transform.SetParent(transform);
                _WeaponManager.AddWeapon(i, spawnWeapon.GetComponent<WeaponController>());
                break;
            }
        }
    }
}
