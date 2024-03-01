using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    CharacterScriptableObject _Charactor;
    PlayerStats _PlayerStats;
    InventoryController _InventoryController;
    bool _IsUsedSkill;
    [SerializeField] float _Cooldown;
    bool _IsEffect;
    [SerializeField] float _EffectOff;
    [SerializeField] Animator _SkillEffect;

    private void Start()
    {
        _PlayerStats = GetComponent<PlayerStats>();
        _Charactor = _PlayerStats._CharacterData;
        _InventoryController = FindObjectOfType<InventoryController>();
    }

    private void Update()
    {
        if(_IsUsedSkill)
        {
            _Cooldown -= Time.deltaTime;
            
            if(_IsEffect)
            {
                _EffectOff -= Time.deltaTime;
                if(_EffectOff <= 0)
                {
                    _IsEffect = false; 
                    ResetExtraStats();
                }
            }

            if (_Cooldown <= 0)
            {
                _IsUsedSkill = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _IsUsedSkill = true;
            _IsEffect = true;
            _Cooldown = _Charactor.SkillCooldown;
            _EffectOff = _Charactor.SkillEffectTimeOut;
            switch (_Charactor.CharactorID)
            {
                case CharactorID.Ace:
                    StartCoroutine(_PlayerStats.UsedSkill());
                    AceSkill();
                    break;
                case CharactorID.Lily:
                    Debug.Log("Lily");
                    LilySkill();
                    break;
            }
        }
    }

    private void AceSkill()
    {
        _InventoryController._AttackBoost += _Charactor.SkillDamageBoost;
    }

    private void LilySkill()
    {
        _InventoryController._SpeedBoost += _Charactor.SkillSpeedMoveBoost;
        _PlayerStats.CurrentAttackSpeed -= _Charactor.SkillSpeedAttackBoost * 0.01f;
    }

    private void ResetExtraStats()
    {
        switch (_Charactor.CharactorID)
        {
            case CharactorID.Ace:
                _InventoryController._AttackBoost -= _Charactor.SkillDamageBoost;
                break;
            case CharactorID.Lily:
                _InventoryController._SpeedBoost -= _Charactor.SkillSpeedMoveBoost;
                _PlayerStats.CurrentAttackSpeed += _Charactor.SkillSpeedAttackBoost * 0.01f;
                break;
        }
    }

    public void AttackAllEnemy()
    {
        if(_Charactor.CharactorID != CharactorID.Ace)
        {
            return;
        }

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        int Damage = _Charactor.SkillDamage;

        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].GetComponent<EnemyStats>().TakeDamage(Damage);
        }
    }

    public void AttackAnimation()
    {
        if (_Charactor.CharactorID != CharactorID.Ace)
        {
            return;
        }

        _SkillEffect.SetTrigger("Skill");
    }
}
