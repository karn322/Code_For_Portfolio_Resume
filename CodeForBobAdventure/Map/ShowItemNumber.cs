using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemNumber : MonoBehaviour
{
    [SerializeField] private IngredientSo _IngredientSo;
    [SerializeField] private Text[] _Text = new Text[Enum.GetValues(typeof(IngredientName)).Length];

    private void Start()
    {
        LoadHowManyItem();
    }

    public void LoadHowManyItem()
    {
        for (int i = 0; i < _Text.Length; i++)
        {
            if (_Text[i] != null)
            _Text[i].text = $"{_IngredientSo.ShowHowManyIngredient(i)}";
        }
    }
}
