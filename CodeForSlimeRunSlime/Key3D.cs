using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key3D : MonoBehaviour
{
    private PlayerMovement3D player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement3D>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetKey();
            gameObject.SetActive(false);
        }
    }
}
