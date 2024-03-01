using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private IngredientSo _IngredientSo;
    [SerializeField] private IngredientName _IngredientName;
    [SerializeField] private FinalDished _DishedName;
    [SerializeField] private Text _Amount;

    private Image _Image;
    Color _GrayColor;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#5B5B5B", out _GrayColor);

        _Image = GetComponent<Image>();
        Reload();
    }

    public void Reload()
    {
        _Amount.text = $"{_IngredientSo.ShowHowManyIngredient((int)_IngredientName)}";
        if (!_IngredientSo.HaveIngredient((int)_IngredientName))
        {
            _Image.color = _GrayColor;
        }
        else
        {
            _Image.color = Color.white;
        }
    }

    public string GetName()
    {
        if (_IngredientName == IngredientName.None)
        {
            return _DishedName.ToString();
        }
        else if (_DishedName == FinalDished.None)
        {
            return _IngredientName.ToString();
        }
        else
        {
            return null;
        }
    }

    public bool _IsIngredient()
    {
        if (_IngredientName != IngredientName.None)
        {
            return true;
        }
        else
        return false;
    }
}
