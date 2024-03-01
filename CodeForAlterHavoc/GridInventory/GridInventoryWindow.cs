using System;
using UnityEngine;
using UnityEngine.UI;

public class GridInventoryWindow : MonoBehaviour
{
    public const int _GridSize = 75;

    RectTransform _RectTransform;
    [SerializeField] RectTransform _ItemDisplay;
    GridLayoutGroup _LayoutGroup;
    [SerializeField] GameObject _GridDisplayPrefab;

    [Header("Grid Size")]
    public int _GridWidth;
    public int _GridHeight;

    public GameObject[,] _GridSlots;
    public GridItem[,] _ItemOnGrid;
    public GemType[,] _GemType;

    private void Awake()
    {
        _RectTransform = GetComponent<RectTransform>();
        _LayoutGroup = GetComponent<GridLayoutGroup>();
        _GridSlots = new GameObject[_GridWidth, _GridHeight];
        _ItemOnGrid = new GridItem[_GridWidth, _GridHeight];
        _GemType = new GemType[_GridWidth, _GridHeight];
        InstantGridSize();
    }

    private void InstantGridSize()
    {
        Vector2 gridWindowSize = new Vector2(_GridWidth * _GridSize, _GridHeight * _GridSize);
        Vector2 gridDisplaySize = new Vector2(_GridSize, _GridSize);

        _RectTransform.sizeDelta = gridWindowSize;
        _LayoutGroup.cellSize = gridDisplaySize;
        
        // in inspecter x is width y is height
        // in this code x is height y is width
        for(int y = 0; y < _GridHeight; y++)
        {
            for(int x = 0; x < _GridWidth; x++)
            {
                _GridSlots[x, y] = Instantiate(_GridDisplayPrefab);
                _GridSlots[x, y].name = name + y + "," + x;
                _GridSlots[x, y].transform.SetParent(transform);
                _GemType[x, y] = GemType.None;
            }
        }
    }

    public Nullable<Vector2Int> FindEmptySpace(GridItem item)
    {
        int width = _GridWidth - item._ItemData._Width + 1;
        int height = _GridHeight - item._ItemData._Height + 1;
        Vector2Int pos;
        for(int y = 0; y < height; y++)
        {
            for( int x = 0; x < width; x++)
            {
                pos = new Vector2Int(x, y);
                if (CheckAvalableSpace(pos, item))
                {
                    return pos;
                }
            }
        }

        return null;
    }

    private bool CheckAvalableSpace(Vector2Int Pos, GridItem item)
    {
        for (int y = 0; y < item._ItemData._Height; y++)
        {
            for (int x = 0; x < item._ItemData._Width; x++)
            {
                if (_ItemOnGrid[Pos.x + x, Pos.y + y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void PlaceitemOnPos(GridItem item, Vector2Int pos)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(_GridSlots[pos.x, pos.y].transform);
        Vector2 itemPos = new Vector2(item._ItemData._Width * _GridSize / 2, - item._ItemData._Height * _GridSize / 2);
        rectTransform.localPosition = itemPos;

        for (int y = 0; y < item._ItemData._Height; y++)
        {
            for(int x = 0;x < item._ItemData._Width; x++)
            {
                _ItemOnGrid[pos.x + x, pos.y + y] = item;
                _GemType[pos.x + x, pos.y + y] = item._ItemData._GemType;
            }
        }

        item.GetComponent<Image>().enabled = true;
        item._PosX = pos.x;
        item._PosY = pos.y;
        rectTransform.SetParent(_ItemDisplay);
    }

    public GridItem PickUpItem(Vector2Int pos)
    {
        GridItem item = _ItemOnGrid[pos.x, pos.y];

        if(item == null)
        {
            return null;
        }

        ClearGridItem(item);
        return item;
    }

    private void ClearGridItem(GridItem item)
    {
        item.GetComponent<Image>().enabled = false;
        for (int y = 0; y < item._ItemData._Height; y++)
        {
            for(int x = 0; x < item._ItemData._Width; x++)
            {
                _ItemOnGrid[item._PosX + x, item._PosY + y] = null;
                _GemType[item._PosX + x, item._PosY + y] = GemType.None;
            }
        }
    }
    
    public bool BoundaryCheck(int x, int y, int height, int width)
    {
        if (!PositionCheck(x, y))
            return false;
        y += height - 1;
        x += width - 1;
        if (!PositionCheck(x, y))
            return false;

        return true;
    }

    private bool PositionCheck(int x, int y)
    {
        if (y < 0 || x < 0 || y >= _GridHeight || x >= _GridWidth)
            return false;

        return true;
    }

    private bool OvarlapCheck(GridItem item, Vector2Int pos, ref GridItem overlap)
    {
        for(int y = 0; y < item._ItemData._Height; y++)
        {
            for (int x = 0;x < item._ItemData._Width; x++)
            {
                if (_ItemOnGrid[pos.x + x, pos.y + y] != null)
                {
                    if(overlap == null)
                    {
                        overlap = _ItemOnGrid[pos.x + x, pos.y + y];
                    }
                    else
                    {
                        if(overlap != _ItemOnGrid[pos.x + x, pos.y + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public bool PlaceItem(GridItem item, Vector2Int pos, ref GridItem overlap)
    {
        if (!BoundaryCheck(pos.x, pos.y, item._ItemData._Height, item._ItemData._Width))
        {
            return false;
        }

        if(!OvarlapCheck(item,pos,ref overlap))
        {
            overlap = null;
            return false;
        }

        if(overlap != null)
        {
            ClearGridItem(overlap);
        }

        PlaceitemOnPos(item, pos);

        return true;
    }

    public GemType[,] GetGemtypeOnGrid()
    {
        return _GemType;
    }

    public GridItem GetFirstGridItem(bool check)
    {
        GridItem item;

        for (int y = 0; y < _GridHeight; y++)
        {
            for (int x = 0; x < _GridWidth; x++)
            {
                item = _ItemOnGrid[x, y];                
                if (item != null)
                {
                    if (!check)
                    {
                        ClearGridItem(item);
                    }                    
                    return item;
                }
            }
        }

        return null;
    }

    public GridItem GetFirstGem(GemType type)
    {
        GridItem item;

        for (int y = 0; y < _GridHeight; y++)
        {
            for (int x = 0; x < _GridWidth; x++)
            {
                item = _ItemOnGrid[x, y];
                
                if(item == null)
                {
                    continue;
                }

                if (item._ItemData._GemType == type)
                {
                    ClearGridItem(item);
                    return item;
                }
            }
        }

        return null;
    }
}
