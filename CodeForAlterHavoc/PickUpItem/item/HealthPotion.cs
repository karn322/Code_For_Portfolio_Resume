using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : PickUp
{
    [SerializeField] int _HealAmount;

    private void OnTriggerEnter2D(Collider2D collision) // item enter player collider
    {
        if (collision.CompareTag("Player"))
        {
            InventoryController inventoryController = FindObjectOfType<InventoryController>();
           
            if (inventoryController._IsHasItemRegenUp)
            {
                _PlayerStats.RestoreHealth(_HealAmount * 2);
            }
            else
            {
                _PlayerStats.RestoreHealth(_HealAmount);
            }

            GetComponent<EffectSoundOnCondition>().PlaySoundEffect();
            GameManager._Instance._AllPotionCollect++;
            _Pool.ReturnGameObject(gameObject);
        }
    }

}
