using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage();
            Destroy(gameObject);
        }
        else if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement player)) { }
        else if (collision.TryGetComponent<EnemyWall>(out EnemyWall wall)) { }
        else if (collision.TryGetComponent<MapEdge>(out MapEdge mapEdge)) { }
        else if (collision.TryGetComponent<Fan>(out Fan fan)) { }
        else
        {
            Destroy(gameObject);
        }
    }
}
