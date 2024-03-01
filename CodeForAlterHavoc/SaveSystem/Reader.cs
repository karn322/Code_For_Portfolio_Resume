using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reader : MonoBehaviour
{
    [Header("RefData")]
    [SerializeField] SaveData _SaveData;// save ref

    [Header("Display Stats")] 
    [SerializeField] TMP_Text _AllEnemyKill;
    [SerializeField] TMP_Text _AllPlayTime;

    [SerializeField] TMP_Text[] _ExpOrb;
    [SerializeField] TMP_Text[] _Gem;
    [SerializeField] TMP_Text _Potion;
    [SerializeField] TMP_Text _UpgradeChest;

    [SerializeField] TMP_Text[] _MonsterKill;
    [SerializeField] TMP_Text[] _BossKill;

    [Header("Display LastGame")]
    [SerializeField] GameObject[] _DisplayHolder;
    [SerializeField] Image[] _Charactor;
    [SerializeField] TMP_Text[] _PlayTime;
    [SerializeField] TMP_Text[] _EnemyKillLastGame;
    [SerializeField] Weapon[] _WeaponDisplay;
    [SerializeField] Inventory[] _InventoryDisplay;

    [System.Serializable]
    public class Weapon
    {
        public Image[] _WeaponSp;
    }

    [System.Serializable]
    public class Inventory
    {
        public Image[] _GemA;
        public Image[] _GemB;
        public Image[] _GemC;
        public Image[] _GemD;
    }

    [SerializeField] Sprite[] _CharactorSprite;
    [SerializeField] Sprite[] _WeaponSprite;
    [SerializeField] Sprite[] _GemSprite;

    private void Start()
    {
        ReadData();
    }

    public void ReadData()
    {
        _AllEnemyKill.text = _SaveData._AllEnemyKill.ToString();
        int minutes = Mathf.FloorToInt(_SaveData._AllTimePLay / 60);
        int seconds = Mathf.FloorToInt(_SaveData._AllTimePLay % 60);
        _AllPlayTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        for (int i = 0; i < _ExpOrb.Length; i++)
        {
            _ExpOrb[i].text = _SaveData._AllExpOrbCollect[i].ToString();
        }
        for (int i = 0; i < _Gem.Length; i++)
        {
            _Gem[i].text = _SaveData._AllGemCollect[i].ToString();
        }
        _Potion.text = _SaveData._AllPotionCollect.ToString();
        _UpgradeChest.text = _SaveData._AllUpgradeChestCollect.ToString();
        for (int i = 0;i < _MonsterKill.Length; i++)
        {
            _MonsterKill[i].text = _SaveData._MonsterKill[i].ToString();
        }
        for (int i = 0; i < _BossKill.Length; i++)
        {
            _BossKill[i].text  = _SaveData._BossKill[i].ToString();
        }

        for (int i = 0; i < _DisplayHolder.Length; i++)
        {
            if(_SaveData._LastGames.Count <= i)
            {
                _DisplayHolder[i].gameObject.SetActive(false);
                continue;
            }

            _DisplayHolder[i].gameObject.SetActive(true);

            minutes = Mathf.FloorToInt(_SaveData._LastGames[i]._PlayTime / 60);
            seconds = Mathf.FloorToInt(_SaveData._LastGames[i]._PlayTime % 60);

            _PlayTime[i].text = string.Format("{0:00}:{1:00}", minutes, seconds);
            _EnemyKillLastGame[i].text = _SaveData._LastGames[i]._EnemyKill.ToString();
            _Charactor[i].sprite = _CharactorSprite[(int)_SaveData._LastGames[i]._CharactorID];

            for (int j = 0; j < _WeaponDisplay[i]._WeaponSp.Length; j++)
            {
                _WeaponDisplay[i]._WeaponSp[j].sprite = _WeaponSprite[(int)_SaveData._LastGames[i]._WeaponID[j]];
                if(_WeaponDisplay[i]._WeaponSp[j].sprite == null)
                {
                    _WeaponDisplay[i]._WeaponSp[j].enabled = false;
                }
            }

            for (int j = 0; j < _InventoryDisplay[i]._GemA.Length; j++)
            {
                _InventoryDisplay[i]._GemA[j].sprite = _GemSprite[(int)_SaveData._LastGames[i]._InventoryA[j]];
                if (_InventoryDisplay[i]._GemA[j].sprite == null)
                {
                    _InventoryDisplay[i]._GemA[j].enabled = false;
                }

                _InventoryDisplay[i]._GemB[j].sprite = _GemSprite[(int)_SaveData._LastGames[i]._InventoryB[j]];
                if (_InventoryDisplay[i]._GemB[j].sprite == null)
                {
                    _InventoryDisplay[i]._GemB[j].enabled = false;
                }

                _InventoryDisplay[i]._GemC[j].sprite = _GemSprite[(int)_SaveData._LastGames[i]._InventoryC[j]];
                if (_InventoryDisplay[i]._GemC[j].sprite == null)
                {
                    _InventoryDisplay[i]._GemC[j].enabled = false;
                }

                _InventoryDisplay[i]._GemD[j].sprite = _GemSprite[(int)_SaveData._LastGames[i]._InventoryD[j]];
                if (_InventoryDisplay[i]._GemD[j].sprite == null)
                {
                    _InventoryDisplay[i]._GemD[j].enabled = false;
                }
            }
        }
    }

    public void ResetData()
    {
        _SaveData.ClearData();
        ReadData();
    }
}
