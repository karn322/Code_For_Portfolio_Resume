using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quest : MonoBehaviour
{
    public QuestRecipeImage[] _QuestRecipeImage;

    public int _DishedNeed;
    private int _HowManyRecipe = 3;

    private int[] _DishedID;

    private FinalDished[] _WhatRecipe;
    private int AllRecipe;

    [SerializeField] private Text _ReQuestText;

    [SerializeField] private IngredientSo _IngredientSo;

    private void Awake()
    {
        _DishedID = new int[_HowManyRecipe];
        _WhatRecipe = new FinalDished[_HowManyRecipe];

        AllRecipe = Enum.GetValues(typeof(FinalDished)).Length;

        if (_IngredientSo.IsQuestEmpty())
            RandomDished();
        else
            _WhatRecipe = _IngredientSo.GetQuestDished();
        for (int i = 0; i < _DishedID.Length; i++)
        {
            _DishedID[i] = _IngredientSo.GetDishID(i);
        }

        _ReQuestText.text = "Get New Quest ( " + _IngredientSo.GetReQuestLimit() + " / 3 )";

        ShowRecipe();
    }

    public void ChangeQuest()
    {
        if (_IngredientSo.GetReQuestLimit() <= 0)
            return;
        for (int i = 0; i < _DishedID.Length; i++)
        {
            _DishedID[i] = 0;
        }
        RandomDished();
        ShowRecipe(); // Random Quest Test
        _IngredientSo.UsedReQuest();
        _ReQuestText.text = "Get New Quest ( "+ _IngredientSo.GetReQuestLimit() + " / 3 )";
    }

    private void RandomDished()
    {
        int currentDished = _DishedNeed;
        for (int i = 0; i < _HowManyRecipe; i++) //random dished number
        {
            if (currentDished <= 0)
            {
                break;
            }

            _DishedID[i] += Random.Range(1, currentDished);
            currentDished -= _DishedID[i];
            

            if (i == _HowManyRecipe - 1 && currentDished > 0)
            {
                _DishedID[i] += currentDished;
            }
        }

        // random dished
        int[] Num = RandomNotRepeat(1, AllRecipe, _HowManyRecipe);
        for (int i = 0; i < Num.Length; i++)
        {
            _WhatRecipe[i] = (FinalDished)Num[i];
        }

        _IngredientSo.SetQuestDished(_WhatRecipe,_DishedID);
    }

    // show recipe
    public void ShowRecipe()
    {
        Recipe currentRecipe;
        for (int i = 0; i < _WhatRecipe.Length; i++)
        {
            string WhatToFind = _WhatRecipe[i].ToString() + "Recipe";
            for (int j = 0; j < _QuestRecipeImage[i].Result.Length; j++)
            {
                _QuestRecipeImage[i].Result[j].enabled = true;
            }
            _QuestRecipeImage[i].HowMany.enabled = true;
            currentRecipe = GameObject.Find(WhatToFind).GetComponent<Recipe>();
            for (int j = 0; j < _QuestRecipeImage[i].PlueIcon.Length; j++)
            {
                _QuestRecipeImage[i].PlueIcon[j].enabled = true;
            }
            _QuestRecipeImage[i].EqualIcon.enabled = true;

            for (int j = 0; j < currentRecipe._IngredientNames.Length; j++)
            {
                _QuestRecipeImage[i].Ingredient[j].enabled = true;
                _QuestRecipeImage[i].Ingredient[j].sprite = Resources.Load<Sprite>("Sprite/Ingredient/" + currentRecipe._IngredientNames[j].ToString());
                if (currentRecipe._IngredientNames[j] == IngredientName.None)
                {
                    _QuestRecipeImage[i].Ingredient[j].enabled = false;

                    if (j >= 1)
                    {
                        _QuestRecipeImage[i].PlueIcon[j - 1].enabled = false;
                    }
                }
            }

            if (_WhatRecipe[i] != FinalDished.None)
                _QuestRecipeImage[i].Result[0].sprite = Resources.Load<Sprite>("Sprite/FinalDish/" + _WhatRecipe[i].ToString());
            
            _QuestRecipeImage[i].HowMany.text = $" {_IngredientSo.ShowHowManyFinalDish((int)_WhatRecipe[i])} / {_DishedID[i]} ";

            if (_DishedID[i] == 0)
            {
                for (int j = 0; j < _QuestRecipeImage[i].Ingredient.Length; j++)
                {
                    _QuestRecipeImage[i].Ingredient[j].enabled = false;
                    
                }
                for (int j = 0; j < _QuestRecipeImage[i].PlueIcon.Length; j++)
                {
                    _QuestRecipeImage[i].PlueIcon[j].enabled = false;
                }
                _QuestRecipeImage[i].EqualIcon.enabled = false;
                _QuestRecipeImage[i].HowMany.enabled = false;
                for (int j = 0; j < _QuestRecipeImage[i].Result.Length; j++)
                {
                    _QuestRecipeImage[i].Result[j].enabled = false;
                }
            }
        }
    }

    public void ShowHowMuchFinalDishLift()
    {
        for (int i = 0; i < _HowManyRecipe; i++)
        {
            _QuestRecipeImage[i].HowMany.text = $" {_IngredientSo.ShowHowManyFinalDish((int)_WhatRecipe[i])} / {_IngredientSo.GetDishID(i)} ";
        }
    }

    public void IsQuestFinish()
    {
        int completed = 0;

        for (int i = 0; i < _HowManyRecipe; i++)
        {
            if (_IngredientSo.ShowHowManyFinalDish((int)_WhatRecipe[i]) >= _IngredientSo.GetDishID(i))
            {
                completed++;
            }
        }

        if (completed == _HowManyRecipe)
        {
            changeScene.LoadNextScene("FinishGame");
        }

    }

    private int[] RandomNotRepeat(int min, int max, int HowManyTime)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < HowManyTime; i++)
        {
            int rand = Random.Range(min, max);
            for (int j = 0; j < list.Count; j++)
            {
                while(rand == list[j])
                {
                    rand = Random.Range(min, max);
                }
            }

            list.Add(rand);
        }

        int[] num = new int[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            num[i] = list[i];
        }
        return num;
    }

}

[System.Serializable]
public class QuestRecipeImage
{
    public Image[] Ingredient;
    public Image[] Result;
    public Text HowMany;
    public Text[] PlueIcon;
    public Text EqualIcon;
}

