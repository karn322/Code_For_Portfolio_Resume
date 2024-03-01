using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magneitem : PickUp
{
    private void OnTriggerEnter2D(Collider2D collision) // item enter player collider
    {
        if (collision.CompareTag("Player"))
        {
            GameObject[] item = GameObject.FindGameObjectsWithTag("PickUp");
            for (int i = 0; i < item.Length; i++)
            {
                item[i].GetComponent<PickUp>().InMagnetRange();
            }
            GetComponent<EffectSoundOnCondition>().PlaySoundEffect();
            _Pool.ReturnGameObject(gameObject);
        }
    }
}
