using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private Text _NameText;
    [SerializeField] private Slider _HPSlider;
    [SerializeField] private Text _HPText;
    private int _MaxHP;

    public void SetHUD(Character character)
    {
        _NameText.text = character._Name;
        _MaxHP = character.GetMaxHP();
        _HPSlider.maxValue = _MaxHP;
        _HPSlider.value = character._CurrentHP;
        _HPText.text = character._CurrentHP + " / " + _MaxHP;
        
    }

    public void SetHP(int hp)
    {
        if (hp <= 0)
        {
            hp = 0;
        }
        _HPSlider.value = hp;
        _HPText.text = hp + " / " + _MaxHP;
    }
}
