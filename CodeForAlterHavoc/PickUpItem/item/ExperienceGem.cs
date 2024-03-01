using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExpType
{
    Blue,
    Green,
    Perple
}

public class ExperienceGem : PickUp
{
    public int _ExpAmount;
    public ExpType _Type;

    private void OnTriggerEnter2D(Collider2D collision) // item enter player collider
    {
        if (collision.CompareTag("Player"))
        {
            _PlayerStats.IncreaseExperience(_ExpAmount);
            GetComponent<EffectSoundOnCondition>().PlaySoundEffect();
            GameManager._Instance._AllExpOrbCollect[(int)_Type]++;
            _Pool.ReturnGameObject(gameObject);
        }
    }
}
