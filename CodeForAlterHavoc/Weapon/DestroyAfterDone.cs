using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDone : MonoBehaviour
{
    public bool _IsDone;
    public bool _MoveUp;
    ObjectPool _Pool;

    private void Start()
    {
        _Pool = FindObjectOfType<ObjectPool>();
    }

    void Update()
    {
        if (_IsDone)
        {
            _Pool.ReturnGameObject(gameObject);
            _IsDone = false;
        }
        if( _MoveUp)
        {
            transform.position += Vector3.up * Time.deltaTime;
        }
    }
}
