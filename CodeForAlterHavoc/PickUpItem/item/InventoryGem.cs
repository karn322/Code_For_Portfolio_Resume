using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGem : PickUp
{
    [SerializeField] GridItemID GemType;

    private void OnTriggerEnter2D(Collider2D collision) // item enter player collider
    {
        if (collision.CompareTag("Player"))
        {
            _Inventory.InstantiateSelectedItemToGrid(GemType);
            GetComponent<EffectSoundOnCondition>().PlaySoundEffect(); 
            GameManager._Instance._AllGemCollect[(int)GemType]++;
            _Pool.ReturnGameObject(gameObject);
        }
    }
}
