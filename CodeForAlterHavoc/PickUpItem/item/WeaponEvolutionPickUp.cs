using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponEvolutionPickUp : MonoBehaviour
{
    WeaponManager _WeaponManager;
    ObjectPool _Pool;

    private void Start()
    {
        _WeaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        _Pool = FindObjectOfType<ObjectPool>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenChest();
            GameManager._Instance._AllWeaponChestCollect++;
            _Pool.ReturnGameObject(gameObject);
        }
    }

    public void OpenChest()
    {
        GameManager._Instance.StartGetChest();

        List<EvolutionWeaponScriptableObject> toEvolve = _WeaponManager.GetPossibleEvolutions();
        if(toEvolve.Count > 0)
        {
            EvolutionWeaponScriptableObject Weapon = toEvolve[Random.Range(0, _WeaponManager.GetPossibleEvolutions().Count)];
            _WeaponManager.EvolveWeapon(Weapon);
            AssigneEvoWeapon(Weapon);
        }
        else
        {
            if (!_WeaponManager.RandomLevelUpWeapon())
            {
                _WeaponManager.RandomOtherOption();
            }
        }
    }

    private void AssigneEvoWeapon(EvolutionWeaponScriptableObject weapon)
    {
        GameManager._Instance._Merge.SetActive(true);
        GameManager._Instance._ChestItemName.text = weapon._EvolvedWeaponData.Name;
        GameManager._Instance._ChestItemDescription.text = weapon._EvolvedWeaponData.Description;

        GameManager._Instance._ChestItemImageL.sprite = weapon._BaseWeapon.Icon;
        GameManager._Instance._ChestItemImageMid.sprite = weapon._CatalystWeapon.Icon;
        GameManager._Instance._ChestItemImageR.sprite = weapon._EvolvedWeaponData.Icon;
    }
}
