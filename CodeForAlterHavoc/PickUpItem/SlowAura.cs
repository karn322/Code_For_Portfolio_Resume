using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAura : MonoBehaviour
{
    public bool _IsOn;

    private void Update()
    {
        if (_IsOn)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyStats enemy))
        {
            if(_IsOn)
            {
                enemy.SlowDown(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyStats enemy))
        {
            enemy.SlowDown(false);
        }
    }
}
