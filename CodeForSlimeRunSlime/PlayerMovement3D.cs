using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement3D : MonoBehaviour
{
    private Rigidbody rb;

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

    private RigidbodyConstraints originalConstraints;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalConstraints = rb.constraints;
    }

    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(horizontalMove * (playerSpeed), rb.velocity.y,0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jump)
            {
                anim.SetTrigger("Jump");
                addjump++;
                rb.velocity = new Vector3(rb.velocity.x, playerjump,0);
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


    private void OnCollisionEnter(Collision collision)
    {
        //jump
        if (collision.gameObject.CompareTag("Ground"))
        {
            addjump = 0;
            jump = true;
            anim.SetInteger("MovementState", 0);
        }
       
        //trap
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Dead");
    }

    private void OnTriggerEnter(Collider other)
    {
        //respawn
        if (other.gameObject.CompareTag("Respawn"))
        {
            currentCheckpoint = transform.position;
        }

        //goal
        if (other.gameObject.CompareTag("Goal"))
        {
            CompleteLevel();
        }
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
        //rb.constraints = originalConstraints;
        anim.SetTrigger("Normal");
    }

    //use in animator
    private void UnMoveAble()
    {
        //rb.constraints = RigidbodyConstraints.FreezePosition;
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

    public void ForceMoveDown()
    {
        transform.position += new Vector3(0, -1, 0);
    }

    public void ForceMoveUp()
    {
        transform.position += new Vector3(0, 1, 0);
    }
}
