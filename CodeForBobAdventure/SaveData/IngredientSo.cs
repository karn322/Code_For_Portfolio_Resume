using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientName { None, Pork, Crab, Egg, Milk, WhipCream, Butter, Cocao, CacaoPowder, Flour, Lemon, Blueberry }
public enum FinalDished { None, ScrambledEggs, BlueBarryCake, LemonCake, Croissant, CrispyPorkBelly, PorkOnStick, ChocolateTaiyaki, WhippedCreamTaiyaki }

[CreateAssetMenu]
public class IngredientSo : ScriptableObject
{
	[SerializeField] private int[] _NormalIngredient = new int[Enum.GetValues(typeof(IngredientName)).Length];
	[SerializeField] private int[] _FinalDished = new int[Enum.GetValues(typeof(FinalDished)).Length];

	[SerializeField] private FinalDished[] _QuestDished = new FinalDished[3];
	[SerializeField] private int[] _DishedID = new int[3];

	[SerializeField] private int _ReQuestLimit = 3;

    public void AddIngredientToBackpack(IngredientName name, int value)
	{
        _NormalIngredient[(int)name] += value;
	}

	public bool RemoveIngredientToBackpack(IngredientName name, int value)
	{
		if (_NormalIngredient[(int)name] <= 0)
		{
			return false;
		}

		_NormalIngredient[(int)name] -= value;

		if (_NormalIngredient[(int)name] < 0)
		{
			_NormalIngredient[(int)name] += value;
			return false;
		}

		return true;
	}

	public int ShowHowManyIngredient(int i)
	{
		return _NormalIngredient[i];
    }

	public void ResetInventory()
	{
		for(int i = 0; i < _NormalIngredient.Length; i++)
		{
			_NormalIngredient[i] = 0;
		}
		for (int i = 0; i < _FinalDished.Length; i++)
		{
			_FinalDished[i] = 0;
		}
		for (int i = 0; i < _QuestDished.Length; i++)
		{
			_QuestDished[i] = FinalDished.None;
		}
		for (int i = 0; i < _DishedID.Length; i++)
		{
			_DishedID[i] = 0;
		}
		_ReQuestLimit = 3;
	}

	public bool HaveIngredient(int i)
	{
		if (_NormalIngredient[i] <= 0)
		{
			return false;
		}
		else
		{
            return true;
        }
	}

	public void AddFinalDish(FinalDished finalDished, int value)
	{
		_FinalDished[(int)finalDished] += value;

    }

	public bool RemoveFinalDish(FinalDished finalDished, int value)
	{

        if (_FinalDished[(int)finalDished] <= 0)
        {
            return false;
        }

        _FinalDished[(int)finalDished] -= value;

        if (_FinalDished[(int)finalDished] < 0)
        {
            _FinalDished[(int)finalDished] += value;
            return false;
        }

		return true;
    }

	public int ShowHowManyFinalDish(int i)
	{
		return _FinalDished[i];
	}

    public int GetDishID(int i)
    {
        return _DishedID[i];
    }

    public bool HaveFinalDish(int i)
	{
		if (_FinalDished[i] <= 0)
		{
			return false;
		}
		else 
		{ 
			return true; 
		}
	}

	public void SetQuestDished(FinalDished[] disheds, int[] iD)
	{
        for (int i = 0; i < disheds.Length; i++)
		{
			_QuestDished[i] = disheds[i];
			_DishedID[i] = iD[i];
        }
	}

	public FinalDished[] GetQuestDished()
	{
		return _QuestDished;
    }

	

	public bool IsQuestEmpty()
	{
		if (_QuestDished[0] == FinalDished.None)
			return true;
		else
			return false;
	}

	public int GetReQuestLimit()
    {
		return _ReQuestLimit;
    }

	public void UsedReQuest()
    {
		_ReQuestLimit--;
    }
}
