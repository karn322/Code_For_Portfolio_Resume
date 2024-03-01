using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Reader;

public enum OtherItemName
{
    Healing,
    AtkUp,
    SpeedUp,
    HpUp,
}

public class WeaponManager : MonoBehaviour
{
    public List<WeaponController> _WeaponsSlot = new List<WeaponController>(6);
    [SerializeField] int[] _WeaponLevels = new int[6];

    public List<Image> _WeaponUiSlot = new List<Image>();
    [SerializeField] List<TMP_Text> _LevelText = new List<TMP_Text>();

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int _WeaponUpgradeIndex;
        public GameObject _InitialWeapon;
        public WeaponScriptableObject _WeaponData;
    }

    [System.Serializable]
    public class OtherItemOption
    {
        public OtherItemName _ItemName;
        public int _Amount;
        public Sprite _Icon;
        public string _Name;
        public string _Description;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text _UpgradedNameDisplay;
        public TMP_Text _UpdradeDescriptonDisplay;
        public Image _UpgradeIcon;
        public Button _UpgradeButton;
    }

    [SerializeField] List<WeaponUpgrade> _MeleeWeaponUpgradeOption = new List<WeaponUpgrade>();
    [SerializeField] List<WeaponUpgrade> _RangeWeaponUpgradeOption = new List<WeaponUpgrade>();
    [SerializeField] List<OtherItemOption> _OtherOption = new List<OtherItemOption>();
    [SerializeField] List<UpgradeUI> _UpgradeUIOption = new List<UpgradeUI>();
    [SerializeField] GameObject _HighlightUIOption;
    [SerializeField] GameObject _DoneButton;
    PlayerStats _Player;

    [SerializeField] List<EvolutionWeaponScriptableObject> _weaponEvolutionOption = new List<EvolutionWeaponScriptableObject>();

    bool _LevelUpWeapon;
    int _LevelUpSlot;
    int _LevelUpWeaponIndex;

    bool _SpawnNewWeapon;
    GameObject _NewWeaponPrefab;

    bool _OtherItem;
    OtherItemOption _OtherItemID;
    [SerializeField] InventoryController _InventoryController; //add stats

    private void Start()
    {
        _Player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex,  WeaponController weapon)
    {
        _WeaponsSlot[slotIndex] = weapon;
        CheckWeaponUi();
    }

    private void CheckWeaponUi()
    {
        for (int i = 0; i < _WeaponsSlot.Count; i++)
        {
            if (_WeaponsSlot[i] == null)
            {
                _WeaponUiSlot[i].enabled = false;
                _LevelText[i].text = "";
            }
            else
            {
                _WeaponUiSlot[i].enabled = true;
                _WeaponUiSlot[i].sprite = _WeaponsSlot[i]._WeaponData.Icon;
                _WeaponLevels[i] = _WeaponsSlot[i]._WeaponData.Level;
                _LevelText[i].text = _WeaponLevels[i].ToString();
            }
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if(_WeaponsSlot.Count > slotIndex)
        {
            WeaponController weapon = _WeaponsSlot[slotIndex];
            if (!weapon._WeaponData.NextLevelPrefab)
            {
                return; //max level
            }
            GameObject upgradedWeapon = Instantiate(weapon._WeaponData.NextLevelPrefab,transform.position,Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            upgradedWeapon.transform.position = weapon.transform.position;
            if(upgradedWeapon.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.flipX = weapon.GetComponent<SpriteRenderer>().flipX;
            }
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            _WeaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>()._WeaponData.Level;

            if (_Player._CharacterData.IsMelee)
            {
                _MeleeWeaponUpgradeOption[upgradeIndex]._WeaponData = upgradedWeapon.GetComponent<WeaponController>()._WeaponData;
            }
            else
            {
                _RangeWeaponUpgradeOption[upgradeIndex]._WeaponData = upgradedWeapon.GetComponent<WeaponController>()._WeaponData;
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> upgrades;
        if (_Player._CharacterData.IsMelee)
        {
            upgrades = new List<WeaponUpgrade>(_MeleeWeaponUpgradeOption);
        }
        else
        {
            upgrades = new List<WeaponUpgrade>(_RangeWeaponUpgradeOption);
        }
        List<OtherItemOption> availableOtherItem = new List<OtherItemOption>(_OtherOption);

        List<WeaponUpgrade> removeUpgrade = new List<WeaponUpgrade>();

        for (int i = 0; i < upgrades.Count; i++) //get all posible upgrade
        {
            if (!upgrades[i]._WeaponData.NextLevelPrefab)
            {
                removeUpgrade.Add(upgrades[i]);
            }
        }

        for (int i = 0; i < removeUpgrade.Count; i++)
        {
            upgrades.Remove(removeUpgrade[i]);
        }

        List<WeaponUpgrade> availableUpgradeWeapon = new List<WeaponUpgrade>();

        if (_WeaponsSlot[_WeaponsSlot.Count-1] != null) //full slot
        {
            for (int i = 0; i < _WeaponsSlot.Count; i++)
            {
                for (int j = 0; j < upgrades.Count; j++)
                {
                    if (upgrades[j]._WeaponData == _WeaponsSlot[i]._WeaponData)
                    {
                        availableUpgradeWeapon.Add(upgrades[j]);
                    }
                }
            }
        }
        else
        {
            availableUpgradeWeapon = new List<WeaponUpgrade>(upgrades);
        }

        foreach (var upgradeOption in _UpgradeUIOption)
        {
            if (availableUpgradeWeapon.Count == 0 && availableOtherItem.Count == 0)
            {
                return;
            }

            int upgradeType;
            #region check upgradeAble list
            
            if (availableUpgradeWeapon.Count == 0)
            {
                upgradeType = 1;
            }
            else if (availableUpgradeWeapon.Count > 0)
            {
                upgradeType = 0;
            }
            else
            {
                upgradeType = 1;
            }
            #endregion

            if (upgradeType == 0) //weapon
            {
                EnableUpgradeUI(upgradeOption);

                WeaponUpgrade chosenWeaponUpgrade = availableUpgradeWeapon[Random.Range(0, availableUpgradeWeapon.Count)];

                availableUpgradeWeapon.Remove(chosenWeaponUpgrade);

                if(chosenWeaponUpgrade != null)
                {
                    bool newWeapon = false;
                    for (int i = 0; i < _WeaponsSlot.Count;  i++)
                    {
                        if (_WeaponsSlot[i] != null && _WeaponsSlot[i]._WeaponData == chosenWeaponUpgrade._WeaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                upgradeOption._UpgradeButton.onClick.AddListener(() => SetLevelUpWeapon(i,chosenWeaponUpgrade._WeaponUpgradeIndex));
                                upgradeOption._UpdradeDescriptonDisplay.text = chosenWeaponUpgrade._WeaponData.NextLevelPrefab.GetComponent<WeaponController>()._WeaponData.Description;
                                upgradeOption._UpgradedNameDisplay.text = chosenWeaponUpgrade._WeaponData.NextLevelPrefab.GetComponent<WeaponController>()._WeaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon)
                    {
                        upgradeOption._UpgradeButton.onClick.AddListener(() => SetGetNewWeapon(chosenWeaponUpgrade._InitialWeapon));
                        upgradeOption._UpdradeDescriptonDisplay.text = chosenWeaponUpgrade._WeaponData.Description;
                        upgradeOption._UpgradedNameDisplay.text = chosenWeaponUpgrade._WeaponData.Name;
                    }

                    upgradeOption._UpgradeButton.onClick.AddListener(() => HighlightSelectedButton(upgradeOption));
                    upgradeOption._UpgradeIcon.sprite = chosenWeaponUpgrade._WeaponData.Icon;
                }
            }
            else if (upgradeType == 1) //other item
            {
                EnableUpgradeUI(upgradeOption);

                OtherItemOption chosenOtherItem = availableOtherItem[Random.Range(0, availableOtherItem.Count)];

                availableOtherItem.Remove(chosenOtherItem);

                upgradeOption._UpgradeButton.onClick.AddListener(() => SetGetOtherOption(chosenOtherItem));

                upgradeOption._UpgradeButton.onClick.AddListener(() => HighlightSelectedButton(upgradeOption));
                upgradeOption._UpgradeIcon.sprite = chosenOtherItem._Icon;
                upgradeOption._UpgradedNameDisplay.text = chosenOtherItem._Name;
                upgradeOption._UpdradeDescriptonDisplay.text = chosenOtherItem._Description;
            }
        }

        _DoneButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        _DoneButton.GetComponent<Button>().enabled = false;
    }

    void SetLevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        _LevelUpWeapon = true;
        _SpawnNewWeapon = false;

        _LevelUpSlot = slotIndex;
        _LevelUpWeaponIndex = upgradeIndex;
    }

    void SetGetNewWeapon(GameObject weaponPrefab)
    {
        _SpawnNewWeapon = true;
        _LevelUpWeapon = false;

        _NewWeaponPrefab = weaponPrefab;
    }

    void SetGetOtherOption(OtherItemOption option)
    {
        _OtherItemID = option;
        _OtherItem = true;
    }

    public void DoneLevelUp() //used on UI button
    {
        if (_LevelUpWeapon)
        {
            LevelUpWeapon(_LevelUpSlot, _LevelUpWeaponIndex);
            _LevelUpWeapon = false;
        }

        if (_SpawnNewWeapon)
        {
            _Player.SpawnWeapon(_NewWeaponPrefab);
            _SpawnNewWeapon = false;
        }

        if (_OtherItem)
        {
            switch (_OtherItemID._ItemName)
            {
                case OtherItemName.Healing:
                    _Player.RestoreHealth(_OtherItemID._Amount);
                    break;

                case OtherItemName.HpUp:
                    _InventoryController._MaxHealthBoost += _OtherItemID._Amount;
                    _Player.RestoreHealth(_OtherItemID._Amount);
                    break;

                case OtherItemName.AtkUp:
                    _InventoryController._AttackBoost += _OtherItemID._Amount;
                    break;

                case OtherItemName.SpeedUp:
                    _InventoryController._SpeedBoost += _OtherItemID._Amount;
                    break;

                default: 
                    break;
            }
            _OtherItem = false;
        }

        _HighlightUIOption.SetActive(false);

        if (GameManager._Instance != null && GameManager._Instance._ChosenUpgrade)
        {
            GameManager._Instance.EndLevelUp();
        }
    }

    void RemoveUpgradeOption()
    {
        foreach (var upgradeOption in _UpgradeUIOption)
        {
            upgradeOption._UpgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrade() // call by gamemanager
    {
        RemoveUpgradeOption();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui._UpgradedNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui._UpgradedNameDisplay.transform.parent.gameObject.SetActive(true);
    }

    void HighlightSelectedButton(UpgradeUI ui)
    {
        _HighlightUIOption.SetActive(true);
        Transform UiPos = ui._UpgradedNameDisplay.transform.parent;
        _HighlightUIOption.transform.SetParent(UiPos);
        _HighlightUIOption.transform.position = UiPos.position;

        _DoneButton.GetComponent<Image>().color = Color.white;
        _DoneButton.GetComponent<Button>().enabled = true;

        //ui._UpgradeButton.onClick.AddListener(() => DoneLevelUp());
    }

    public bool RandomLevelUpWeapon()
    {
        List<WeaponUpgrade> availableUpgradeWeapon = GetAllUpgradeWeapon();

        if (availableUpgradeWeapon.Count == 0)
            return false;

        WeaponUpgrade weapon = availableUpgradeWeapon[Random.Range(0, availableUpgradeWeapon.Count)];
        
        for (int i = 0; i < _WeaponsSlot.Count; i++)
        {
            if (_WeaponsSlot[i] == null)
                break;

            if (_WeaponsSlot[i]._WeaponData == weapon._WeaponData)
            {
                LevelUpWeapon(i, weapon._WeaponUpgradeIndex);
                break;
            }
        }
        
        AssigneItem(weapon._WeaponData.Name, weapon._WeaponData.Description, weapon._WeaponData.Icon);

        return true;
    }

    private void AssigneItem(string Name, string Description, Sprite Image)
    {
        GameManager._Instance._Merge.SetActive(false);
        GameManager._Instance._ChestItemName.text = Name;
        GameManager._Instance._ChestItemDescription.text = Description;
        GameManager._Instance._ChestItemImageMid.sprite = Image;
    }

    private List<WeaponUpgrade> GetAllUpgradeWeapon()
    {
        List<WeaponUpgrade> upgrades;
        if (_Player._CharacterData.IsMelee)
        {
            upgrades = new List<WeaponUpgrade>(_MeleeWeaponUpgradeOption);
        }
        else
        {
            upgrades = new List<WeaponUpgrade>(_RangeWeaponUpgradeOption);
        }
        List<WeaponUpgrade> removeUpgrade = new List<WeaponUpgrade>();
        for (int i = 0; i < upgrades.Count; i++) //get all posible upgrade
        {
            if (!upgrades[i]._WeaponData.NextLevelPrefab)
            {
                removeUpgrade.Add(upgrades[i]);
            }
        }

        for (int i = 0; i < removeUpgrade.Count; i++)
        {
            upgrades.Remove(removeUpgrade[i]);
        }

        List<WeaponUpgrade> availableUpgradeWeapon = new List<WeaponUpgrade>();

        for (int i = 0; i < _WeaponsSlot.Count; i++)
        {
            if (_WeaponsSlot[i] == null)
                break;

            for (int j = 0; j < upgrades.Count; j++)
            {
                if (upgrades[j]._WeaponData == _WeaponsSlot[i]._WeaponData)
                {
                    availableUpgradeWeapon.Add(upgrades[j]);
                }
            }
        }

        return availableUpgradeWeapon;
    }

    public void RandomOtherOption()
    {
        _OtherItem = true;
        List<OtherItemOption> availableOtherItem = new List<OtherItemOption>(_OtherOption);
        OtherItemOption item = availableOtherItem[Random.Range(0, availableOtherItem.Count)];
        _OtherItemID = item;

        AssigneItem(item._Name, item._Description, item._Icon);

        DoneLevelUp();
    }

    public List<EvolutionWeaponScriptableObject> GetPossibleEvolutions()
    {
        ///WeaponController = Wslot
        ///WeaponScriptableObject = evoweapon

        List<EvolutionWeaponScriptableObject> Possible = new List<EvolutionWeaponScriptableObject>();
        for (int i = 0; i < _weaponEvolutionOption.Count; i++)
        {
            for (int A = 0; A < _WeaponsSlot.Count; A++)
            {
                if (_WeaponsSlot[A] == null)
                {
                    break;
                }
                for (int B = 0; B < _WeaponsSlot.Count; B++)
                {
                    if (_WeaponsSlot[B] == null)
                    {
                        break;
                    }
                    if (_WeaponsSlot[A]._WeaponData == _weaponEvolutionOption[i]._BaseWeapon && _WeaponsSlot[B]._WeaponData == _weaponEvolutionOption[i]._CatalystWeapon)
                    {
                        Possible.Add(_weaponEvolutionOption[i]);
                    }
                }
            }
        }

        return Possible;
    }

    public void EvolveWeapon(EvolutionWeaponScriptableObject evolution)
    {
        List<WeaponController> removeweapon = new List<WeaponController>();
        for (int i = 0; i < _WeaponsSlot.Count; i++)
        {
            if (_WeaponsSlot[i] == null)
            {
                break;
            }
            if (_WeaponsSlot[i]._WeaponData == evolution._BaseWeapon || _WeaponsSlot[i]._WeaponData == evolution._CatalystWeapon)
            {
                removeweapon.Add(_WeaponsSlot[i]);
                /*
                WeaponController weapon = _WeaponsSlot[i];
                _WeaponsSlot[i] = null;
                Destroy(weapon.gameObject);
                */
            }
        }

        for (int i = 0; i < removeweapon.Count; i++)
        {
            Destroy(removeweapon[i].gameObject);
            _WeaponsSlot.Remove(removeweapon[i]);
            _WeaponsSlot.Add(null);
        }

        _Player.SpawnWeapon(evolution._EvolvedWeaponController);
    }
}
