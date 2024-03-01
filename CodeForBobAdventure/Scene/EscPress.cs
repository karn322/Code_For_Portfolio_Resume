using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscPress : MonoBehaviour
{
    [SerializeField] private GameObject GameObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.SetActive(false);
        }
    }
}
