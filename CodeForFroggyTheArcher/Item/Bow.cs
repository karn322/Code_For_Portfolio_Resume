using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _Arrow;
    [SerializeField] private Transform _ShotPoint;

    [SerializeField] private Transform _Aim;
    [SerializeField] private float _Offset;
    private Vector3 _StartingSize;
    private Vector3 _AimStartingSize;

    void Start()
    {
        _StartingSize = transform.localScale;
        _AimStartingSize = _Aim.localScale;
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 mousePosition3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 perendicular = _Aim.position - mousePosition3;
        Quaternion val = Quaternion.LookRotation(Vector3.forward, perendicular);
        val *= Quaternion.Euler(0, 0, _Offset);
        _Aim.rotation = val;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ProjectileVelocity = CalculateProjectileVelocity(_ShotPoint.position, mousePosition, 1f);

            Rigidbody2D fire = Instantiate(_Arrow, _ShotPoint.position, Quaternion.identity);
            fire.velocity = ProjectileVelocity;
        }
    }

    private Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 distance = target - origin;
        float distanceY = distance.y;
        distance.y = 0;
        float distanceX = distance.magnitude;

        float velocityX = distanceX / time;
        float velocityY = distanceY / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        Vector2 result = distance.normalized;
        result *= velocityX;
        result.y = velocityY;

        return result;
    }
}
