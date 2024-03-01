using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    private float _PlayerSpeed = 5f;
    private float _PlayerJumpHight = 10f;
    private int _JumpTime;

    [SerializeField] private LayerMask _Layer;
    [SerializeField] private BoxCollider2D _FloorHit;

    private SpriteRenderer _PlayerSprite;
    [SerializeField] private GameObject _BowL;
    [SerializeField] private GameObject _BowR;

    private Vector3 _StartPosition;
    private bool _HaveKey;
    [SerializeField] private int _HowManyKey;
    private int _Key;

    [SerializeField] private TMP_Text _AppleText;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _PlayerSprite = GetComponent<SpriteRenderer>();
        _StartPosition = transform.position;
        _HaveKey = false;
        _JumpTime = 0;
        UpdateScore();
    }


    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector3(horizontalMove * (_PlayerSpeed), _rb.velocity.y, 0);

        if (horizontalMove > 0)
        {
            _PlayerSprite.flipX = false;
            _BowL.SetActive(false);
            _BowR.SetActive(true);
        }
        if (horizontalMove < 0)
        {
            _PlayerSprite.flipX = true;
            _BowR.SetActive(false);
            _BowL.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _JumpTime < 1)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _PlayerJumpHight, 0);
            _JumpTime += 1;
        }

        if (IsGrounded())
        {
            _JumpTime = 0;
        }

        if (_Key >= _HowManyKey && !_HaveKey)
        {
            _HaveKey = true;
        }
    }

    public void Checkpoint(Vector3 position)
    {
        _StartPosition = position;
    }

    public void TakeDamage()
    {
        transform.position = _StartPosition;
    }

    public void GetKey()
    {
        _Key  += 1;
        UpdateScore();
    }

    public bool CheckKey()
    {
        return _HaveKey;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_FloorHit.bounds.center, Vector2.down, _FloorHit.bounds.extents.y + 0.02f, _Layer);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateScore()
    {
        _AppleText.text = $"{_Key} / {_HowManyKey}";
    }
}
