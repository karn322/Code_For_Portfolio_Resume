using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController _Controller;
    public GameObject _TargetMap;

    void Start()
    {
        _Controller = FindObjectOfType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _Controller._CurrentChunk = _TargetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if ( _Controller._CurrentChunk == _TargetMap)
                _Controller._CurrentChunk = null;
        }
    }
}
