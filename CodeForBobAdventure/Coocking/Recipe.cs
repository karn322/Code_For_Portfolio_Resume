using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public FinalDished _DishedName;
    public IngredientName _IngredientName;

    public IngredientName[] _IngredientNames = new IngredientName[4];

    public string GetIngredientName(int i)
    {
        return _IngredientNames[i].ToString();
    }

    public FinalDished GetFinalDishName()
    {
        return _DishedName;
    }

    public IngredientName GetFinalIngredientName()
    {
        return _IngredientName;
    }

    public bool IsFinalIngredient()
    {
        if (_DishedName == FinalDished.None)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }
}

