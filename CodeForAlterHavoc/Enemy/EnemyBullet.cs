using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float _CurrentDamage;
    public float _CurrentSpeed;
    public Vector3 _Direction;

    ObjectPool _Pool;
    public float _DestroyAfterSeconds;

    private void Start()
    {
        _Pool = FindObjectOfType<ObjectPool>();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _Direction, _CurrentSpeed * Time.deltaTime);
        _DestroyAfterSeconds -= Time.deltaTime;
        if (_DestroyAfterSeconds <= 0)
            _Pool.ReturnGameObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            stats.TakeDamage(_CurrentDamage);
            _Pool.ReturnGameObject(gameObject);
        }   
    }
}
