using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door3D : MonoBehaviour
{
    private PlayerMovement3D player;
    private Animator anim;
    [SerializeField] private Transform doorPosition;
    [SerializeField] private BoxCollider boxCollider;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement3D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player.haveKey && (Vector3.Distance(doorPosition.transform.position, transform.position) < 0.1f))
            {
                player.TakeKey();
                anim.SetTrigger("Unlock");
            }
        }
    }

    public void animUnlock()
    {
        boxCollider.enabled = false;
    }
}
