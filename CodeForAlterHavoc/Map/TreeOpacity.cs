using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOpacity : MonoBehaviour
{
    [SerializeField] Vector3 pos;
    [SerializeField] GameObject Object;
    [SerializeField] bool _NotOpa;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Object.transform.position -= pos;

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "FG";
            sprite.sortingOrder = 30;

            if (_NotOpa)
                return;

            
            sprite.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Object.transform.position += pos;

            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "MG";
            sprite.sortingOrder = 0;

            if (_NotOpa)
                return;

            
            sprite.color = Color.white;
        }
    }
}
