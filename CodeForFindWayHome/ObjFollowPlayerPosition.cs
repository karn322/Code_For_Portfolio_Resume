using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFollowPlayerPosition : MonoBehaviour
{
    private Transform _PlayerPosition;

    private void Awake()
    {
        _PlayerPosition = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.LookAt(_PlayerPosition);
        transform.Rotate(0, 180, 0);
    }
}
