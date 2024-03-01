using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    public string _Name;
    public int _Damage;

    [SerializeField] private int _MaxHP;
    public int _CurrentHP;
    public IngredientName _IngredientName;

    [SerializeField] private Animator _Anim;
    [SerializeField] private SpriteRenderer _DeadSprite;

    private BattleSystem _BattleSystem;
    private int _Index;
    private bool _Isdead;

    public bool _IsRangeType;
    public GameObject _Bullet;
    public Transform _BulletTransform;

    public GameObject _Body;

    private void Start()
    {
        _BattleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        if (_DeadSprite != null)
            _DeadSprite.enabled = false;
        if (_IsRangeType)
            _Bullet.GetComponent<SpriteRenderer>().enabled = false;
        if (_Name == "SleepyPig")
            _Body.SetActive(false);
        if(_Name == "CottonSpider")
        {
            IngredientName inName;
            int i = Random.Range(0, 4);
            switch (i)
            {
                case 0:
                    inName = IngredientName.Cocao;
                    break;
                case 1:
                    inName = IngredientName.Lemon;
                    break;
                case 2:
                    inName = IngredientName.Blueberry;
                    break;
                case 3:
                    inName = IngredientName.Flour;
                    break;
                default:
                    inName = IngredientName.Flour;
                    break;
            }
                
            _IngredientName = inName;
        }
    }

    public int GetMaxHP()
    {
        return _MaxHP;
    }

    public bool TakeDamage(int damage)
    {
        _CurrentHP -= damage;
        if(_CurrentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeHeal(int heal)
    {
        _CurrentHP += heal;
        if (_CurrentHP >= _MaxHP)
        {
            _CurrentHP = _MaxHP;
        }
    }

    public void SetIndex(int i)
    {
        _Index = i;
    }

    private void OnMouseDown()
    {
        if (!_Isdead)
        {
            _BattleSystem.ChangeTarget(_Index);
        }
    }

    public void Dead()
    {
        _Isdead = true;
        _Anim.SetBool("Dead", true);
        if (_DeadSprite != null)
            _DeadSprite.enabled = true;
    }

    public void PlayAttackAnim()
    {
        _Anim.SetTrigger("Attack");
    }

    public void PlayWaitAnim()
    {
        _Anim.SetTrigger("Wait");
    }

    public void PlayIdelAnim()
    {
        _Anim.SetTrigger("Idle");
    }

    public float GetAnimTime(AnimState animState)
    {
        float time = 0;
        AnimationClip[] clip = _Anim.runtimeAnimatorController.animationClips;

        string animName = null;
        switch (animState)
        {
            case AnimState.Attack:
                animName = _Name + "Attack";
                break;

            case AnimState.Idel:
                animName = _Name + "Idel";
                break;

            case AnimState.Wait:
                if (_IsRangeType)
                    animName = _Name + "Bullet";
                else
                    animName = _Name + "Walk";
                break;

            case AnimState.Dead:
                animName = _Name + "Dead";
                break;

            default:
                animName = null;
                break;
        }

        foreach (AnimationClip animationClip in clip)
        {
            if (animationClip.name == animName)
            {
                time = animationClip.length;
            }
        }

        return time;
    }
}
