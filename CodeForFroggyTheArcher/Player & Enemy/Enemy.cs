using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _EnemySprite;
    private bool _FacingRight = false;
    private Rigidbody2D _rb;
    private float _DirectionX = -1;
    private float _EnemySpeed = 5;
    private bool _HitIllusionWall;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(_DirectionX * _EnemySpeed, _rb.velocity.y);

        if (_HitIllusionWall)
        {
            _DirectionX *= -1;
            _FacingRight = !_FacingRight;
            _HitIllusionWall = false;
        }

        if (_FacingRight)
        {
            _EnemySprite.flipX = false;
        }
        if (!_FacingRight)
        {
            _EnemySprite.flipX = true;
        }
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }

    public void HitWall()
    {
        _HitIllusionWall = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.TakeDamage();
        }
    }
}
