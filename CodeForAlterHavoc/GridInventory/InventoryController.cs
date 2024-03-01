using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GridItemID
{
    Blue,
    Green,
    Red,
    White
}

public class InventoryController : MonoBehaviour
{  
    GridItemFollowMouse _MousePos;

    [SerializeField] List<GridItemData> _ItemsData;
    [SerializeField] GridInventoryWindow _GenItemGrid;
    GridInventoryWindow _SelectedGrid;

    [HideInInspector] public GridItem _SelectedItem;
    GridItem _OverlapingItem;

    [SerializeField] GameObject _GridItemPrefab;

    PlayerStats _PlayerStats;

    [Header("Buff Rate")]
    public float _ExpBoost = 1;
    public float _HealthRegenBoost = 0;
    public float _MagnetRangeBoost = 1;

    public int _MaxHealthBoost;
    int _LastHealthBoost;
    public int _SpeedBoost;
    int _lastSpeedBoost;
    float _SpeedBoostPercent = 0.01f;
    public int _AttackBoost;
    int _LastAtkBoost;

    [SerializeField] GridInventoryWindow[] _AllCheckGrid;
    bool[] _IsLastChance;
    bool _IsHasLastChance;

    bool[] _IsItemRegenUp;
    [HideInInspector] public bool _IsHasItemRegenUp;

    GridItemArrange _SelectedPattern;
    GridItem _AutoItem;
    [SerializeField] GameObject _PlaceInPatternButton;

    [System.Serializable]
    public class EffectPic
    {
        public InventoryArrangeEffect _Effect;
        public Sprite _Sprite;
    }

    [Header("UI Display")]
    [SerializeField] GameObject[] _ActivateEffectUi;
    [SerializeField] EffectPic[] _EffectSprite;

    [SerializeField] GameObject _AtkUpSprite;
    [SerializeField] GameObject _SpeedUpSprite;
    [SerializeField] GameObject _HpUpSprite;

    [SerializeField] TMP_Text _AtkUpText;
    [SerializeField] TMP_Text _SpeedUpText;
    [SerializeField] TMP_Text _HpUpText;

    [HideInInspector] public bool _CanPickUp = true;

    [SerializeField] Sprite _Red;
    [SerializeField] Sprite _Green;
    [SerializeField] Sprite _Blue;
    [SerializeField] Sprite _White;

    [SerializeField] SaveData _SaveData;
    [SerializeField] bool _Cheating;

    private void Awake()
    {
        _MousePos = FindObjectOfType<GridItemFollowMouse>();
        _IsLastChance = new bool[_AllCheckGrid.Length];
        _IsItemRegenUp = new bool[_AllCheckGrid.Length];
    }

    private void Start()
    {
        _PlayerStats = FindObjectOfType<PlayerStats>();
        _AtkUpSprite.SetActive(false);
        _SpeedUpSprite.SetActive(false);
        _HpUpSprite.SetActive(false);
        for (int i = 0; i < _ActivateEffectUi.Length; i++)
        {
            _ActivateEffectUi[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnleftMouseClick();
        }

        if(GameManager._Instance._CurrentState == GameManager.GameState.UsingInventory)
        {
            if (_SelectedItem != null) // show item icon or not
                _MousePos.ShowSelectedItem(true);
            else
                _MousePos.ShowSelectedItem(false);
        }

        if (_Cheating)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) //key 1
            {
                InstantiateSelectedItemToGrid(GridItemID.Blue);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) //key 2
            {
                InstantiateSelectedItemToGrid(GridItemID.Red);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) //key 3
            {
                InstantiateSelectedItemToGrid(GridItemID.Green);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) //key 4
            {
                InstantiateSelectedItemToGrid(GridItemID.White);
            }
        }

        #region check boost
        if (_ExpBoost != _PlayerStats._CurrentExpBoost)
        {
            _PlayerStats._CurrentExpBoost = _ExpBoost;
        }

        if(_HealthRegenBoost != _PlayerStats._HealthRegenBoost)
        {
            _PlayerStats._HealthRegenBoost = _HealthRegenBoost;
        }

        if(_MagnetRangeBoost != _PlayerStats._MagnetRangeBoost )
        {
            _PlayerStats._MagnetRangeBoost = _MagnetRangeBoost;
        }            

        if(_MaxHealthBoost != _PlayerStats._MaxHealthBoost)
        {
            _PlayerStats._MaxHealthBoost = _MaxHealthBoost;
        }            

        if(_SpeedBoost * _SpeedBoostPercent != _PlayerStats._SpeedBoost)
        {
            _PlayerStats._SpeedBoost = _SpeedBoost * _SpeedBoostPercent; //add in percentage
        }

        if(_AttackBoost != _PlayerStats._AttackBoost)
        {
            _PlayerStats._AttackBoost = _AttackBoost;
        }

        if(_IsHasLastChance != _PlayerStats._LastChance)
        {
            _PlayerStats._LastChance = _IsHasLastChance;
        }
        #endregion

        DisplayStatsBuff();
    }

    public void InstantiateSelectedItemToGrid(GridItemID ID)
    {
        GridItem item = Instantiate(_GridItemPrefab).GetComponent<GridItem>();

        item.SetData(_ItemsData[(int)ID]);

        Nullable<Vector2Int> posOnGrid = _GenItemGrid.FindEmptySpace(item);
        if (posOnGrid == null)
        {
            Destroy(item.gameObject);
            return;
        }

        _GenItemGrid.PlaceitemOnPos(item, (Vector2Int)posOnGrid);

        GameManager._Instance.CheckTutorial();
    }

    void OnleftMouseClick()
    {
        if (!_CanPickUp)
            return;

        Nullable<Vector2Int> posCheck = _MousePos.GetPostionOnGrid();
        if (posCheck == null)
            return;

        Vector2Int pos = posCheck.Value;

        _SelectedGrid = _MousePos._SelectedGrid;

        if (_SelectedItem != null) // place item
        {
            bool canPlace = _SelectedGrid.PlaceItem(_SelectedItem, pos,ref _OverlapingItem);
            if(canPlace)
            {
                _SelectedItem = null;
                if(_OverlapingItem != null) // switch item between on grid and on mouse
                {
                    _SelectedItem = _OverlapingItem;
                    _OverlapingItem = null;
                    _MousePos.ChangeIcon(_SelectedItem);
                }
            }
        }
        else
        {
            _SelectedItem = _SelectedGrid.PickUpItem(pos);
            if (_SelectedItem != null)
                _MousePos.ChangeIcon(_SelectedItem);
        }
            
    }

    public void ClearAllItem(GridInventoryWindow grid)
    {
        for (int i = 0; i < grid._GridHeight * grid._GridWidth; i++)
        {
            GridItem item = grid.GetFirstGridItem(false);
            if(item == null)
            {
                break;
            }
            item.DeleteThisItem();
        }
    }

    public void ClearSelectedItem()
    {
        if (_SelectedItem != null)
        {
            _SelectedItem.DeleteThisItem();
        }
    }

    public void SelectedGridAndClear(GridInventoryWindow selectedGrid)
    {
        _SelectedGrid = selectedGrid;

        if (_SelectedGrid.GetFirstGridItem(true) != null)
        {
            int loop = _SelectedGrid._GridHeight * _SelectedGrid._GridWidth;

            for (int i = 0; i < loop; i++)
            {
                _AutoItem = _SelectedGrid.GetFirstGridItem(false);

                if (_AutoItem == null)
                {
                    break;
                }

                Nullable<Vector2Int> posOnGrid = _GenItemGrid.FindEmptySpace(_AutoItem);
                _GenItemGrid.PlaceitemOnPos(_AutoItem, (Vector2Int)posOnGrid);
                _AutoItem = null;
            }
        }
    }

    public void RePlaceItemOnGrid()
    {
        GemType[] selectedArrange = _SelectedPattern._GemType;

        for (int i = 0; i < selectedArrange.Length; i++)
        {
            if (selectedArrange[i] == GemType.None)
            {
                continue;
            }

            _AutoItem = _GenItemGrid.GetFirstGem(selectedArrange[i]);

            if (_AutoItem == null)
            {
                continue;
            }

            Vector2Int posOnGrid = new Vector2Int(i % 3, i / 3);
            _SelectedGrid.PlaceitemOnPos(_AutoItem, posOnGrid);
            _AutoItem = null;
        }
        _PlaceInPatternButton.SetActive(false);
    }

    public void SelectedArrangePattern(GridItemArrange itemPattern)
    {
        _SelectedPattern = itemPattern;
        _PlaceInPatternButton.SetActive(true);
    }

    void DisplayStatsBuff()
    {
        if (_LastHealthBoost != _MaxHealthBoost)
        {            
            if(_MaxHealthBoost > 0)
            {
                _HpUpSprite.SetActive(true);
                _HpUpText.text = _MaxHealthBoost.ToString();
            }
            else
            {
                _HpUpSprite.SetActive(false);
                _HpUpText.text = "0";
            }
            _LastHealthBoost = _MaxHealthBoost;
        }

        if (_lastSpeedBoost != _SpeedBoost)
        {            
            if (_SpeedBoost > 0)
            {
                _SpeedUpSprite.SetActive(true);
                _SpeedUpText.text = _SpeedBoost.ToString() + "%";
            }
            else
            {
                _SpeedUpSprite.SetActive(false);
                _SpeedUpText.text = "0%";
            }
            _lastSpeedBoost = _SpeedBoost;
        }

        if (_LastAtkBoost != _AttackBoost)
        {            
            if (_AttackBoost > 0)
            {
                _AtkUpSprite.SetActive(true);
                _AtkUpText.text = _AttackBoost.ToString();
            }
            else
            {
                _AtkUpSprite.SetActive(false);
                _AtkUpText.text = "0";
            }
            _LastAtkBoost = _AttackBoost;
        }
    }

    public void DisplayEffectIcon(int id,InventoryArrangeEffect Effect)
    {
        _ActivateEffectUi[id].GetComponent<Image>().sprite = null;

        switch (Effect)
        {
            case InventoryArrangeEffect.HealingUp:
                CheckIsSingle(id, Effect);
                break;

            case InventoryArrangeEffect.Shield:
                CheckIsSingle(id, Effect);
                break;

            case InventoryArrangeEffect.SlowAura:
                CheckIsSingle(id, Effect);
                break;

            case InventoryArrangeEffect.LastChange:
                _ActivateEffectUi[id].SetActive(true);
                _ActivateEffectUi[id].GetComponent<Image>().sprite = _EffectSprite[GetSpriteID(Effect)]._Sprite;
                break;

            case InventoryArrangeEffect.ExpBoost:
                _ActivateEffectUi[id].SetActive(true);
                _ActivateEffectUi[id].GetComponent<Image>().sprite = _EffectSprite[GetSpriteID(Effect)]._Sprite;
                break;

            case InventoryArrangeEffect.Regen:
                _ActivateEffectUi[id].SetActive(true);
                _ActivateEffectUi[id].GetComponent<Image>().sprite = _EffectSprite[GetSpriteID(Effect)]._Sprite;
                break;

            case InventoryArrangeEffect.MagnetRange:
                _ActivateEffectUi[id].SetActive(true);
                _ActivateEffectUi[id].GetComponent<Image>().sprite = _EffectSprite[GetSpriteID(Effect)]._Sprite;
                break;

            default:
                _ActivateEffectUi[id].SetActive(false);
                break;

        }
    }

    private void CheckIsSingle(int id, InventoryArrangeEffect Effect)
    {
        int num = GetSpriteID(Effect);

        bool Activated = false;
        for (int i = 0; i < _ActivateEffectUi.Length; i++)
        {
            if (_ActivateEffectUi[i].GetComponent<Image>().sprite == _EffectSprite[num]._Sprite)
            {
                Activated = true;
                break;
            }
        }

        if (Activated)
        {
            _ActivateEffectUi[id].SetActive(false);
        }
        else
        {
            _ActivateEffectUi[id].SetActive(true);
            _ActivateEffectUi[id].GetComponent<Image>().sprite = _EffectSprite[num]._Sprite;
        }
    }

    private int GetSpriteID(InventoryArrangeEffect Effect)
    {
        int num = 0;
        for (int i = 0; i < _EffectSprite.Length; i++)
        {
            if (Effect == _EffectSprite[i]._Effect)
            {
                num = i;
                break;
            }
        }

        return num;
    }

    public void GetLastChance(int gridID)
    {
        _IsLastChance[gridID] = true;
        CheckIsLastChance();
    }

    public void LostLastChance(int gridID)
    {
        _IsLastChance[gridID] = false;
        CheckIsLastChance();
    }

    public void UsedLastChanceGem()
    {
        for (int i = 0; i < _IsLastChance.Length; i++)
        {
            if(_IsLastChance[i])
            {
                ClearAllItem(_AllCheckGrid[i]);
                break;
            }
        }
    }

    void CheckIsLastChance()
    {
        for (int i = 0; i < _IsLastChance.Length; i++)
        {
            if (_IsLastChance[i])
            {
                _IsHasLastChance = true;
                break;
            }
            _IsHasLastChance = false;
        }
    }

    public void GetItemRegenUp(int gridID)
    {
        _IsItemRegenUp[gridID] = true;
        UpdateItemRegenUp();
    }

    public void LostItemRegenUp(int gridID)
    {
        _IsItemRegenUp[gridID] = false;
        UpdateItemRegenUp();
    }

    void UpdateItemRegenUp()
    {
        for (int i = 0; i < _IsItemRegenUp.Length; i++)
        {
            if (_IsItemRegenUp[i])
            {
                _IsHasItemRegenUp = true;
                return;
            }
        }
        _IsHasItemRegenUp = false;
    }

    public void IsCanPickUpGem(bool t)
    {
        _CanPickUp = t;
    }

    public void DisplayGemOnDead(int num)
    {
        GemType[] gemTypes = GetGemType(num);
        for (int i = 0; i < gemTypes.Length; i++)
        {
            GameManager._Instance._Inventory[num]._DisplayInventoryImage[i].sprite = GetSpriteFromGemType(gemTypes[i]);

            if (GameManager._Instance._Inventory[num]._DisplayInventoryImage[i].sprite == null)
            {
                GameManager._Instance._Inventory[num]._DisplayInventoryImage[i].enabled = false;
            }
            else
            {
                GameManager._Instance._Inventory[num]._DisplayInventoryImage[i].enabled = true;
            }
        }
    }

    public void SetGemOnLastSave()
    {
        int lastRound = _SaveData._LastGames.Count - 1;
        GemType[] gemTypes = GetGemType(0);
        for (int i = 0; i < gemTypes.Length; i++)
        {
            _SaveData._LastGames[lastRound]._InventoryA[i] = gemTypes[i];
        }
        gemTypes = GetGemType(1);
        for (int i = 0; i < gemTypes.Length; i++)
        {
            _SaveData._LastGames[lastRound]._InventoryB[i] = gemTypes[i];
        }
        gemTypes = GetGemType(2);
        for (int i = 0; i < gemTypes.Length; i++)
        {
            _SaveData._LastGames[lastRound]._InventoryC[i] = gemTypes[i];
        }
        gemTypes = GetGemType(3);
        for (int i = 0; i < gemTypes.Length; i++)
        {
            _SaveData._LastGames[lastRound]._InventoryD[i] = gemTypes[i];
        }
        
    }

    GemType[] GetGemType(int ID)
    {
        GemType[,] types = _AllCheckGrid[ID].GetGemtypeOnGrid();

        int width = types.GetLength(0);
        int height = types.GetLength(1);

        GemType[] gems = new GemType[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gems[(y * width) + x] = types[x, y];
            }
        }
        return gems;
    }

    Sprite GetSpriteFromGemType(GemType type)
    {
        Sprite sprite = null;
        switch (type)
        {
            case GemType.Blue:
                sprite = _Blue;
                break;
            case GemType.Green:
                sprite = _Green;
                break;
            case GemType.Red:
                sprite = _Red;
                break;
            case GemType.White:
                sprite = _White;
                break;
            default:
                break;
        }
        return sprite;
    }
}
