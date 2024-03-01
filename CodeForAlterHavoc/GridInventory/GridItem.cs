using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour
{
    public GridItemData _ItemData;

    public int _PosX;
    public int _PosY;

    public void SetData(GridItemData data)
    {
        _ItemData = data;
        GetComponent<Image>().sprite = data._Icon;
        Vector2 size = new Vector2(data._Width * GridInventoryWindow._GridSize, data._Height * GridInventoryWindow._GridSize);
        GetComponent<RectTransform>().sizeDelta = size;
    }

    public void DeleteThisItem()
    {
        Destroy(gameObject);
    }
}
