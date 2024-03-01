using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    private Item _CurrentItem;
    public Image _CustomCursor;

    public Slot[] _CraftingSlots;

    public List<Item> _ItemList;
    public Recipe[] _Recipe;
    private string[] _Recipes;
    public GameObject _ResultSlot;
    public Transform _BackPackPosition;

    public Transform _ResultTransform;

    [SerializeField] private IngredientSo _Ingredient;
    [SerializeField] private Quest _Quest;
    [SerializeField] private Button _Crafting;

    private void Start()
    {
        _Recipes = new string[_Recipe.Length];
       
        for (int i = 0; i < _Recipe.Length; i++)
        {
            string CurrentRecipeString = "";

            for (int j = 0; j < _ItemList.Count; j++)
            {
                if (_Recipe[i]._IngredientNames[j] != IngredientName.None)
                {
                    CurrentRecipeString += _Recipe[i].GetIngredientName(j);
                }
                else
                {
                    CurrentRecipeString += "null";
                }
            }
            
            _Recipes[i] = CurrentRecipeString;
        }

        _Quest.ShowRecipe();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(_CurrentItem != null)
            {
                _CustomCursor.gameObject.SetActive(false);
                Slot nearestSlot = null;
                float shortestDistance = float.MaxValue;

                foreach(Slot slot in _CraftingSlots)
                {
                    float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);

                    if(dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearestSlot = slot;
                    }
                }

                if (nearestSlot.item != null)
                {
                    Enum.TryParse(nearestSlot.item.GetName(), true, out IngredientName name1);
                    _Ingredient.AddIngredientToBackpack(name1, 1);

                    nearestSlot.item.Reload();
                    _CurrentItem.Reload();
                }

                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = _CurrentItem.GetComponent<Image>().sprite;
                
                nearestSlot.item = _CurrentItem;
                _ItemList[nearestSlot.index] = _CurrentItem;

                _CurrentItem = null;
            }
        }
    }

    public void CheckForCreateRecipe()
    {
        _Crafting.interactable = false;

        string currentRecipeString = "";

        _ResultSlot.transform.position = new Vector3(_ResultTransform.position.x, _ResultTransform.position.y, _ResultTransform.position.z);
        _ResultSlot.transform.localScale = new Vector3(1, 1, 1);

        foreach (Item item in _ItemList)
        {
            if (item != null)
            {
                currentRecipeString += item.GetName();
            }
            else 
            {
                currentRecipeString += "null";               
            }
        }

        for (int i = 0; i < _Recipes.Length; i++)
        {
            if (_Recipes[i] == currentRecipeString)
            {

                if (_Recipe[i].IsFinalIngredient())
                {
                    IngredientName name = _Recipe[i].GetFinalIngredientName();
                    _ResultSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Ingredient/" + name.ToString());
                    _Ingredient.AddIngredientToBackpack(name, 1);
                }
                else
                {
                    FinalDished name = _Recipe[i].GetFinalDishName();
                    _ResultSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/FinalDish/" + name.ToString());
                    _Ingredient.AddFinalDish(name,1);
                }

                StartCoroutine(ResultsGoToBackPack(_Recipe[i]));

                break;
            }
        }

        for (int i = 0; i < _CraftingSlots.Length; i++)
        {
            _CraftingSlots[i].gameObject.SetActive(false);
            _CraftingSlots[i].item = null;
        }

        for (int i = 0; i < _ItemList.Count; i++)
        {
            _ItemList[i] = null;
        }
        
        _Crafting.interactable = true;

        _Quest.ShowHowMuchFinalDishLift();
    }

    public void OnClickSlot(Slot slot)
    {
        Enum.TryParse(_ItemList[slot.index].GetName(), true, out IngredientName ingredientName);
        _Ingredient.AddIngredientToBackpack(ingredientName, 1);
        slot.item.Reload();

        slot.item = null;
        _ItemList[slot.index] = null;
        slot.gameObject.SetActive(false);
    }

    public void OnMouseDownItem(Item item)
    {
        Enum.TryParse(item.GetName(), true, out IngredientName name);
        bool canDrag = _Ingredient.RemoveIngredientToBackpack(name, 1);

        item.Reload();

        if (_CurrentItem == null & canDrag)
        {
            _CurrentItem = item;
            _CustomCursor.gameObject.SetActive(true);
            _CustomCursor.sprite = _CurrentItem.GetComponent<Image>().sprite;
        }

    }


    IEnumerator ResultsGoToBackPack(Recipe recipe)
    {
        SoundManager.Instance.PlayEffect(Sound.SoundEffectName.cook);
        yield return new WaitForSeconds(2.5f);
        SoundManager.Instance.PlayEffect(Sound.SoundEffectName.finishCook);
            
        _ResultSlot.GetComponent<Image>().enabled = true;

        for (float t = 0f; t <= 1; t += 5 * Time.deltaTime)
        {
            _ResultSlot.transform.localScale = Vector3.Lerp(_ResultTransform.localScale, new Vector3(1.5f, 1.5f, 1.5f), t);
            yield return null;
        }

        for (float t = 0f; t <= 1; t += 5 * Time.deltaTime)
        {
            _ResultSlot.transform.localScale = Vector3.Lerp(new Vector3(1.5f, 1.5f, 1.5f), _ResultTransform.localScale, t);
            yield return null;
        }

        Transform transform = _BackPackPosition;
        if (recipe.IsFinalIngredient())
        {
            transform = GameObject.Find(recipe.GetFinalIngredientName().ToString()).GetComponent<Transform>();
        }

        yield return new WaitForSeconds(.5f);

        for (float t = 0f; t <= 1; t += 1 * Time.deltaTime)
        {
            _ResultSlot.transform.position = Vector3.Lerp(_ResultTransform.position, transform.position, t);
            _ResultSlot.transform.localScale = Vector3.Lerp(_ResultTransform.localScale, new Vector3(0.01f,0.01f,0.01f), t);
            yield return null;
        }

        if (recipe.IsFinalIngredient())
        {
            GameObject.Find(recipe.GetFinalIngredientName().ToString()).GetComponent<Item>().Reload();
        }
        else
        {
            _Quest.ShowHowMuchFinalDishLift();
        }

        _ResultSlot.GetComponent<Image>().enabled = false;


        _Quest.IsQuestFinish();
    }
}
