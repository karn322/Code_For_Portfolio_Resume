using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWall : MonoBehaviour
{
    [SerializeField] private GameObject _Wall;
    private Animator _Anim;
    private Transform _PlayerPosition;
    private KeySystem _KeySystem;
    private bool _HaveWall;
    private bool _HaveKey;
    private float _PlayerDistance;

    private void Awake()
    {
        _PlayerPosition = GameObject.Find("Player").GetComponent<Transform>();
        _KeySystem = GameObject.Find("KeySystem").GetComponent<KeySystem>();
        _Anim = GetComponent<Animator>();
        _HaveWall = true;
        _HaveKey = false;
    }

    void Update()
    {
        _PlayerDistance = (transform.position - _PlayerPosition.position).magnitude;
        _HaveKey = _KeySystem.CheckKey();
        if (Input.GetKeyDown(KeyCode.E) && _PlayerDistance <= 1.5f){
            _Anim.SetTrigger("Click");
        }


        if (Input.GetKeyDown(KeyCode.E) && _HaveKey && _HaveWall && _PlayerDistance <= 1.5f)
        {            
            Destroy(_Wall);
            _HaveWall = false;
            _HaveKey = false;
            _KeySystem.TakeKey();
        }
    }
}
