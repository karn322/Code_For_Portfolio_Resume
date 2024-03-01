using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, ICollectable
{
    [HideInInspector] public PlayerStats _PlayerStats;
    [HideInInspector] public bool _InPlayerMagnetRange = false;
    [HideInInspector] public InventoryController _Inventory;

    protected ObjectPool _Pool;

    private void Start()
    {
        _PlayerStats = FindObjectOfType<PlayerStats>();
        _Inventory = FindObjectOfType<InventoryController>();
        _Pool = FindObjectOfType<ObjectPool>();
    }

    public void InMagnetRange()
    {
        _InPlayerMagnetRange = true;
    }
}
