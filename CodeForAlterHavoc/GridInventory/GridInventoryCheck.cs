using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InventoryArrangeEffect
{
    None,
    ExpBoost,
    Regen,
    MagnetRange,
    Shield,
    LastChange,
    HealingUp,
    SlowAura
}

public enum GemType
{
    None,
    Blue,
    Green,
    Red,
    White
}

public class GridInventoryCheck : MonoBehaviour
{
    public GridItemArrange[] _ItemArrange;
    GridInventoryWindow _GridWindow;
    InventoryController _InventoryController;
    [SerializeField] GemType[] _GemType;
    public InventoryArrangeEffect _Effect = InventoryArrangeEffect.None;
    InventoryArrangeEffect _LastEffect;
    PlayerStats _PlayerStats;
    [SerializeField] SlowAura _SlowAura;
    [SerializeField] int _GridID;

    [Header("Buff Rate")]
    [SerializeField] float _ExpBoost = 0.2f;
    [SerializeField] float _HealthRegenBoost = 1;
    [SerializeField] float _MagnetRangeBoost = 2;

    int _AttackBoost;
    int _HealthBoost;
    int _SpeedBoost;
    int _AllBoost;

    int _LastAttackBoost;
    int _LastHealthBoost;
    int _LastSpeedBoost;

    void Start()
    {
        _PlayerStats = FindObjectOfType<PlayerStats>();
        _InventoryController = FindObjectOfType<InventoryController>();
        _GridWindow = GetComponent<GridInventoryWindow>();
    }

    void Update()
    {
        GetGemType();
        CheckArrangeItem();

        if(_LastEffect != _Effect)
        {
            DeActivateEffect();
            _LastEffect = _Effect;
            ActivateEffect();
        }

        CheckGemTypeEffect();

        #region add stats
        //attack change
        if (_LastAttackBoost != _AttackBoost + _AllBoost)
        {            
            _InventoryController._AttackBoost -= _LastAttackBoost;
            _LastAttackBoost = _AttackBoost + _AllBoost;
            _InventoryController._AttackBoost += _LastAttackBoost;
        }
        
        if(_LastHealthBoost != _HealthBoost + _AllBoost)
        {
            _InventoryController._MaxHealthBoost -= _LastHealthBoost;
            _LastHealthBoost = _HealthBoost + _AllBoost;
            _InventoryController._MaxHealthBoost += _LastHealthBoost;
        }

        if (_LastSpeedBoost != _SpeedBoost + _AllBoost)
        {
            _InventoryController._SpeedBoost -= _LastSpeedBoost;
            _LastSpeedBoost = _SpeedBoost + _AllBoost;
            _InventoryController._SpeedBoost += _LastSpeedBoost; 
        }
        #endregion
    }

    void CheckArrangeItem()
    {
        bool[] isThisArray = new bool[_GemType.Length];

        for (int x = 0; x < _ItemArrange.Length; x++)
        {
            for (int i = 0; i < _GemType.Length; i++)
            {
                if (_GemType[i] == GemType.White)
                {
                    isThisArray[i] = true;
                    continue;
                }

                if (_ItemArrange[x]._GemType[i] == GemType.None)
                {
                    isThisArray[i] = true;
                    continue;
                }

                if (_GemType[i] != _ItemArrange[x]._GemType[i])                    
                {
                    isThisArray[i] = false;
                }
                else
                {
                    isThisArray[i] = true;
                }
            }

            if (isThisArray.All(b => b))
            {
                _Effect = _ItemArrange[x]._Effect;
                return;
            }
        }

        _Effect = InventoryArrangeEffect.None;
    }

    void ActivateEffect()
    {
        switch (_Effect)
        {
            case InventoryArrangeEffect.ExpBoost:
                _InventoryController._ExpBoost += _ExpBoost;
                break;

            case InventoryArrangeEffect.Regen:
                _InventoryController._HealthRegenBoost += _HealthRegenBoost;
                break;

            case InventoryArrangeEffect.MagnetRange:
                _InventoryController._MagnetRangeBoost += _MagnetRangeBoost;
                break;

            case InventoryArrangeEffect.Shield:
                _PlayerStats._IsShieldActivate = true;
                break;

            case InventoryArrangeEffect.LastChange:
                _InventoryController.GetLastChance(_GridID);
                break;

            case InventoryArrangeEffect.HealingUp:
                _InventoryController.GetItemRegenUp(_GridID);
                break;

            case InventoryArrangeEffect.SlowAura:
                _SlowAura._IsOn = true;
                break;
                
            default: 
                break;
        }

        _InventoryController.DisplayEffectIcon(_GridID, _Effect);
    }

    void DeActivateEffect()
    {
        switch (_LastEffect)
        {
            case InventoryArrangeEffect.ExpBoost:
                _InventoryController._ExpBoost -= _ExpBoost;
                break;

            case InventoryArrangeEffect.Regen:
                _InventoryController._HealthRegenBoost -= _HealthRegenBoost;
                break;

            case InventoryArrangeEffect.MagnetRange:
                _InventoryController._MagnetRangeBoost -= _MagnetRangeBoost;
                break;

            case InventoryArrangeEffect.Shield:
                _PlayerStats._IsShieldActivate = false;
                break;

            case InventoryArrangeEffect.LastChange:
                _InventoryController.LostLastChance(_GridID);
                break;

            case InventoryArrangeEffect.HealingUp:
                _InventoryController.LostItemRegenUp(_GridID);
                break;

            case InventoryArrangeEffect.SlowAura:
                _SlowAura._IsOn = false;
                break;

            default:
                break;
        }
    }

    void GetGemType()
    {
        GemType[,] types = _GridWindow.GetGemtypeOnGrid();

        int width = types.GetLength(0);
        int height = types.GetLength(1);

        GemType[] gems = new GemType[width * height];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                gems[(y * width) + x] = types[x, y];
            }
        }
        _GemType = gems;
    }

    public void ClearLastChance()
    {
        _InventoryController.ClearAllItem(_GridWindow);
    }

    void CheckGemTypeEffect()
    {
        int blue = 0;
        int green = 0;
        int red = 0;
        int white = 0;

        for (int i = 0; i < _GemType.Length; i++)
        {
            if (_GemType[i] == GemType.Blue)    
                blue++;

            if (_GemType[i] == GemType.Green) 
                green++;

            if (_GemType[i] == GemType.Red) 
                red++;

            if (_GemType[i] == GemType.White)
                white++;
        }

        // 3x3 grid
        if(_GridWindow._GridWidth == 3  &&  _GridWindow._GridHeight == 3)
        {

            GemType[] gem = new GemType[3];
            // check width
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gem[j] = _GemType[i + (j * 3)];
                }

                if(gem.All(b => b == GemType.White))
                {
                    white += 2;
                } 
                else if (gem.All(b => b == GemType.Blue || b == GemType.White))
                {
                    blue += 2;
                }
                else if (gem.All(b => b == GemType.Green || b == GemType.White))
                {
                    green += 2;
                }
                else if (gem.All(b => b == GemType.Red || b == GemType.White))
                {
                    red += 2;
                }
            }

            //check height
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gem[j] = _GemType[(i * 3) + j];
                }

                if (gem.All(b => b == GemType.White))
                {
                    white += 2;
                }
                else if (gem.All(b => b == GemType.Blue || b == GemType.White))
                {
                    blue += 2;
                }
                else if (gem.All(b => b == GemType.Green || b == GemType.White))
                {
                    green += 2;
                }
                else if (gem.All(b => b == GemType.Red || b == GemType.White))
                {
                    red += 2;
                }
            }

            //check diagonal
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(i == 1)
                    {
                        gem[j] = _GemType[(2 * j) + 2];
                    }
                    else
                    {
                        gem[j] = _GemType[j + (j * 3)];
                    }
                }

                if (gem.All(b => b == GemType.White))
                {
                    white += 2;
                }
                else if (gem.All(b => b == GemType.Blue || b == GemType.White))
                {
                    blue += 2;
                }
                else if (gem.All(b => b == GemType.Green || b == GemType.White))
                {
                    green += 2;
                }
                else if (gem.All(b => b == GemType.Red || b == GemType.White))
                {
                    red += 2;
                }
            }

            //check diamond
            gem = new GemType[4];

            gem[0] = _GemType[1];
            gem[1] = _GemType[3];
            gem[2] = _GemType[5];
            gem[3] = _GemType[7];

            if (gem.All(b => b == GemType.White))
            {
                white += 3;
            }
            else if (gem.All(b => b == GemType.Blue || b == GemType.White))
            {
                blue += 3;
            }
            else if (gem.All(b => b == GemType.Green || b == GemType.White))
            {
                green += 3;
            }
            else if (gem.All(b => b == GemType.Red || b == GemType.White))
            {
                red += 3;
            }
        }

        _SpeedBoost = blue;
        _HealthBoost = green;
        _AttackBoost = red;
        _AllBoost = white;
    }
}
