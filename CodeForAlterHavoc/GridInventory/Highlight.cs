using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    GridItemFollowMouse _FollowMouse;
    RectTransform _Highlighter;
    [SerializeField] RectTransform _HighlighterDisplay;
    GridInventoryWindow _InventoryWindow;
    InventoryController _InventoryController;

    private void Start()
    {
        _FollowMouse = FindObjectOfType<GridItemFollowMouse>();
        _InventoryController = FindObjectOfType<InventoryController>();
        _Highlighter = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(_FollowMouse.GetPostionOnGrid() == null || !_InventoryController._CanPickUp)
        {
            IsHighlightDisplay(false);
            return;
        }

        _InventoryWindow = _FollowMouse._SelectedGrid;
        Vector2Int pos = (Vector2Int)_FollowMouse.GetPostionOnGrid();

        ChangeHighlightSize(pos);

        if (_InventoryController._SelectedItem != null)
        {
            GridItem item = _InventoryController._SelectedItem;

            if(_InventoryWindow.BoundaryCheck(pos.x, pos.y, item._ItemData._Height, item._ItemData._Width))
            {
                ChangeHighlightPosition(item, pos);
            }
        }
    }

    private void IsHighlightDisplay(bool on)
    {
        GetComponent<Image>().enabled = on;
    }

    private void ChangeHighlightSize(Vector2Int pos)
    {
        if(_InventoryWindow._ItemOnGrid[pos.x, pos.y] != null)
        {
            GridItem item = _InventoryWindow._ItemOnGrid[pos.x, pos.y];

            Vector2 size = new Vector2(item._ItemData._Width * GridInventoryWindow._GridSize, item._ItemData._Height * GridInventoryWindow._GridSize);
            _Highlighter.sizeDelta = size;

            ChangeHighlightPosition(item);
        }        
    }

    private void ChangeHighlightPosition(GridItem item)
    {
        RectTransform rectTransform = _InventoryWindow._GridSlots[item._PosX, item._PosY].GetComponent<RectTransform>();

        _Highlighter.transform.position = rectTransform.position + new Vector3(item._ItemData._Width * GridInventoryWindow._GridSize / 2, -item._ItemData._Height * GridInventoryWindow._GridSize / 2, 0);
        _Highlighter.SetParent(_HighlighterDisplay);
        _Highlighter.SetAsFirstSibling();

        IsHighlightDisplay(true);
    }

    public void ChangeHighlightPosition(GridItem item, Vector2Int pos)
    {
        RectTransform rectTransform = _InventoryWindow._GridSlots[pos.x, pos.y].GetComponent<RectTransform>();

        _Highlighter.transform.position = rectTransform.position + new Vector3(item._ItemData._Width * GridInventoryWindow._GridSize / 2, -item._ItemData._Height * GridInventoryWindow._GridSize / 2, 0);
        _Highlighter.SetParent(_HighlighterDisplay);
        _Highlighter.SetAsFirstSibling();

        IsHighlightDisplay(true);
    }
}
