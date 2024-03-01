using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffectOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip _Clip;

    private void Start()
    {
        SoundManager._Instance.PlaySound(_Clip);
    }
}
