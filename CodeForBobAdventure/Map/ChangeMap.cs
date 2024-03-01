using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMap : MonoBehaviour
{
    [SerializeField] private string _SceneName;
    private PauseUnPause _PauseUnPause;
    private Animator _Anim;
    private BoxCollider2D _BoxCollider2D;
    private void Start()
    {
        _Anim = GetComponent<Animator>();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
        _PauseUnPause = GameObject.Find("PauseMenu").GetComponent<PauseUnPause>();
    }

    private void Update()
    {
        if (_PauseUnPause.IsPause())
        {
            _BoxCollider2D.enabled = false;
        }
        else
        {
            _BoxCollider2D.enabled = true;
        }
    }

    private void OnMouseEnter()
    {
        _Anim.SetTrigger("OnMouseEnter");
    }

    private void OnMouseExit()
    {
        _Anim.SetTrigger("OnMouseExit");
    }

    private void OnMouseDown()
    {
        changeScene.LoadNextScene(_SceneName);
    }
}
