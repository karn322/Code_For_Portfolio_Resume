using System;
using UnityEngine;
using UnityEngine.UI;

public class GridItemFollowMouse : MonoBehaviour
{
    GridInventoryWindow[] _InventoryWindow;
    RectTransform _RectTransform;
     
    public GridInventoryWindow _SelectedGrid;
    int _PosY;
    int _PosX;
    Vector3 _Mouseoffset;

    private void Awake()
    {
        _InventoryWindow = FindObjectsOfType<GridInventoryWindow>();
        _RectTransform = GetComponent<RectTransform>();
        _RectTransform.sizeDelta = new Vector2(GridInventoryWindow._GridSize, GridInventoryWindow._GridSize);
        ShowSelectedItem(false);
    }

    void Update()
    {
        if(GameManager._Instance._CurrentState == GameManager.GameState.UsingInventory)
        {
            transform.position = Input.mousePosition + _Mouseoffset;
            GetGrid();
        }
    }

    public void GetGrid()
    {
        Vector3 mousePos = Input.mousePosition;
        float lastTarget = 9999f;
        int gridIndex = 0;

        for (int i = 0; i < _InventoryWindow.Length; i++)
        {
            for (int x = 0; x < _InventoryWindow[i]._GridSlots.GetLength(0); x++)
            {
                for (int y = 0; y < _InventoryWindow[i]._GridSlots.GetLength(1); y++)
                {
                    Vector3 Target = _InventoryWindow[i]._GridSlots[x, y].transform.position + new Vector3(GridInventoryWindow._GridSize / 2, -GridInventoryWindow._GridSize / 2, 0);
                    float target = Vector3.Distance(Target, mousePos);
                    if (target < lastTarget)
                    {
                        lastTarget = target;
                        gridIndex = i;
                        _PosX = x;
                        _PosY = y;
                    }
                }
            }
        }

        if (lastTarget < GridInventoryWindow._GridSize / 2) //check click on grid tile position
        {
            _SelectedGrid = _InventoryWindow[gridIndex];
        }
        else
        {
            _SelectedGrid = null;
        }
    }

    public void ShowSelectedItem(bool show)
    {
        GetComponent<Image>().enabled = show;
    }

    public void ChangeIcon(GridItem item)
    {
        GetComponent<Image>().sprite = item._ItemData._Icon;
        Vector2 size = new Vector2(item._ItemData._Width * GridInventoryWindow._GridSize, item._ItemData._Height * GridInventoryWindow._GridSize);
        GetComponent<RectTransform>().sizeDelta = size;
        _Mouseoffset = new Vector2(item._ItemData._Width * GridInventoryWindow._GridSize / 4, -item._ItemData._Height * GridInventoryWindow._GridSize / 4);
    }

    public Nullable<Vector2Int> GetPostionOnGrid()
    {
        if (_SelectedGrid != null)
        {
            return new Vector2Int(_PosX, _PosY);
        }
        return null;
    }

}
