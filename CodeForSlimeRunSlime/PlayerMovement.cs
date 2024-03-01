using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool jump = true;
    private int addjump = 0;

    [SerializeField] private int howmanyjump = 2;
    [SerializeField] private float playerSpeed = 3.5f;
    [SerializeField] private float playerjump = 7f;

    private SpriteRenderer sprite;
    private Animator anim;

    private enum MovementState { idle, moving, jumping, falling, jumping2 }
    private MovementState state = 0;

    private int liveCount = 3;
    [SerializeField] private Text LiveText;

    public Vector2 currentCheckpoint;

    public int howManyKey = 0;
    public bool haveKey = false;
    [SerializeField] private Text KeyText;

    public Transform playerPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalMove * (playerSpeed), rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jump)
            {
                anim.SetTrigger("Jump");
                addjump++;
                rb.velocity = new Vector2(rb.velocity.x, playerjump);
                if (addjump >= howmanyjump)
                {
                    jump = false;
                }
            }
        }

        if (howManyKey > 0)
        {
            haveKey = true;
        }
        else
        {
            haveKey = false;
        }

        UpdateAnimation(horizontalMove);
    }

    private void UpdateAnimation(float horizontalMove)
    {
        if (horizontalMove > 0f)
        {
            state = MovementState.moving;
            sprite.flipX = false;
        }
        else if (horizontalMove < 0f)
        {
            state = MovementState.moving;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("MovementState", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //jump
        if (collision.gameObject.CompareTag("Ground"))
        {
            addjump = 0;
            jump = true;
            anim.SetInteger("MovementState", 0);
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            addjump = 0;
            jump = true;
        }
        //trap
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //respawn
        if (collision.gameObject.CompareTag("Respawn"))
        {
            currentCheckpoint = transform.position;
        }

        //goal
        if (collision.gameObject.CompareTag("Goal"))
        {
            CompleteLevel();
        }
    }

    private void Die()
    {        
        anim.SetTrigger("Dead");
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //use in animator
    private void DeadCount()
    {
        liveCount--;
        ShowLive();
        if (liveCount < 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    //use in animator
    private void RestartLevel()
    {   
        transform.position = currentCheckpoint;
        anim.SetTrigger("Spawn");
    }

    //use in animator
    private void MoveAble()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetTrigger("Normal");
    }

    //use in animator
    private void UnMoveAble()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void GetKey()
    {
        howManyKey++;
        ShowKey();
    }

    public void TakeKey()
    {
        howManyKey--;
        ShowKey();
    }

    public void ShowKey()
    {
        if (howManyKey > 9)
        {
            KeyText.text = "x " + howManyKey;
        }
        else if (howManyKey >= 0)
        {
            KeyText.text = "x 0" + howManyKey;
        }
    }

    public void ShowLive()
    {
        LiveText.text = "x 0" + liveCount;
    }
}
