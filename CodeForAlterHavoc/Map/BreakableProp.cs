using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakableProp : MonoBehaviour
{
    [SerializeField] float _StartHealth;
    float _MaxHp;
    float _CurrentHp;
    [SerializeField] GameObject _DamagePopUp;
    [SerializeField] bool _IsHealthUp;
    [SerializeField] int _AddHealthPerMinutes;
    int _Time;

    Sprite _Normal;
    [SerializeField] Sprite _BreakSprite;
    SpriteRenderer _SpriteRenderer;
    bool _IsDead;

    [SerializeField] Image _ShowHp;
    [SerializeField] float OpacityDownTime;

    ObjectPool _Pool;
    DropRateManager _DropRateManager;
    [SerializeField] int ExpAmount;
    [SerializeField] int ExpUpPerMin;

    private void Start()
    {
        _Pool = FindObjectOfType<ObjectPool>();
        _SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _DropRateManager = GetComponent<DropRateManager>();
        if (GameManager._Instance != null)
        {
            _MaxHp = _StartHealth + ((int)GameManager._Instance._StopwatchTime / 60 * _AddHealthPerMinutes);
            _CurrentHp = _MaxHp;
            _Normal = _SpriteRenderer.sprite;

            for (int i = 0; i < _DropRateManager._Drops.Count; i++)
            {
                if (_DropRateManager._Drops[i]._ItemPrefab.TryGetComponent<ExperienceGem>(out ExperienceGem gem))
                {
                    gem._ExpAmount = ExpAmount + ((int)GameManager._Instance._StopwatchTime / 60 * ExpUpPerMin);
                }
            }
        }
    }

    private void Update()
    {
        if (_IsHealthUp && _Time != (int)GameManager._Instance._StopwatchTime / 60)
        {
            _Time = (int)GameManager._Instance._StopwatchTime / 60;
            _MaxHp += (_Time * _AddHealthPerMinutes);
            _CurrentHp += (_Time * _AddHealthPerMinutes);
        }

        if (_IsDead)
        {
            OpacityDown();
        }
    }

    public void TakeDamage(float damage)
    {
        _CurrentHp -= damage;

        if (_ShowHp != null)
        {
            ShowHp();
        }

        if (_CurrentHp <= 0)
        {
            _IsDead = true;
            
            if (_BreakSprite != null)
            {
                _SpriteRenderer.sprite = _BreakSprite;
            }

            return;
        }

        GameObject damagePop = _Pool.GetObject(_DamagePopUp);
        string damageText = damage.ToString();
        damagePop.GetComponent<TextMeshPro>().text = damageText;
        damagePop.GetComponent<TextMeshPro>().color = Color.white;
        damagePop.transform.position = transform.position;
    }

    void OpacityDown()
    {
        if (_SpriteRenderer.color == new Color(1, 1, 1, 0))
        {
            _DropRateManager.DropItem();
            _Pool.ReturnGameObject(gameObject);
        }
        _SpriteRenderer.color -= new Color(0,0,0, OpacityDownTime);
    }

    void ShowHp()
    {
        _ShowHp.enabled = true;
        _ShowHp.fillAmount = _CurrentHp / _MaxHp;
    }

    public void Respawn()
    {
        if (!_IsDead)
            return;

        _IsDead = false;
        _SpriteRenderer.sprite = _Normal;
        _SpriteRenderer.color = Color.white;

        _MaxHp = _StartHealth + ((int)GameManager._Instance._StopwatchTime / 60 * _AddHealthPerMinutes);
        _CurrentHp = _MaxHp;

        for (int i = 0; i < _DropRateManager._Drops.Count; i++)
        {
            if (_DropRateManager._Drops[i]._ItemPrefab.TryGetComponent<ExperienceGem>(out ExperienceGem gem))
            {
                gem._ExpAmount = ExpAmount + ((int)GameManager._Instance._StopwatchTime / 60 * ExpUpPerMin);
            }
        }
    }
}
