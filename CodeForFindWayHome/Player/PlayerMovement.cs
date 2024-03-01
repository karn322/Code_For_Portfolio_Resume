using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private StarGravitation _StarGravity;
    private bool _SetStar;
    private bool _ResetRotation;
    private Rigidbody _rb;
    private Transform _PlayerTranform;
    private bool _IsOnStar;

    [SerializeField] private Vector3 _Force;

    private Vector3 _WalkDirection;
    private float _WalkSpeed = 12f;
    private Vector3 _JumpDirecrion;
    [SerializeField]private float _JumpHeight = 20f;

    private bool _IsGrounded;
    [SerializeField] private Transform _GroundCheck;
    [SerializeField] private float _GroundDistance = 0.1f;
    [SerializeField] private LayerMask _GroundMask;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _PlayerTranform = transform;

        _rb.AddForce(_Force);
        _StarGravity = GameObject.Find("Gravity Ball").GetComponent<StarGravitation>();
        _SetStar = false;
        _ResetRotation = false;
        _IsOnStar = false;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (_SetStar)
        {
            _rb.useGravity = false;
            _StarGravity.Attract(_PlayerTranform);
            _IsOnStar = true;
        }
        else
        {
            _rb.useGravity = true;
            SetRotationToZero();
            _IsOnStar = false;
        }

        _WalkDirection = new Vector3(x, 0, z);

        _JumpDirecrion = new Vector3(0, _JumpHeight, 0);
         
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _rb.MovePosition(_rb.position + transform.TransformDirection(_WalkDirection) * _WalkSpeed * 2 * Time.deltaTime);
        }
        else
        {
            _rb.MovePosition(_rb.position + transform.TransformDirection(_WalkDirection) * _WalkSpeed * Time.deltaTime);
        }

        Quaternion _DeltaRotation = Quaternion.Euler(new Vector3(0, x, 0) * Time.deltaTime);
        _rb.MoveRotation(_rb.rotation * _DeltaRotation);

        _IsGrounded = Physics.CheckSphere(_GroundCheck.position, _GroundDistance, _GroundMask);

        if(_IsGrounded && Input.GetButton("Jump"))
        {
            _rb.velocity = _JumpDirecrion;
        }
    }

    public void ChangeGravityTrue()
    {
        _SetStar = true;
    }

    public void ChangeGravityFalse()
    {
        _SetStar = false;
        _ResetRotation = true;
    }

    private void SetRotationToZero()
    {
        if (_ResetRotation)
        {
            transform.rotation = Quaternion.identity;
            _ResetRotation = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Destroy(other);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public bool IsOnStar()
    {
        return _IsOnStar;
    }
}
