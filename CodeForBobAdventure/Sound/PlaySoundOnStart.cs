using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private Sound.SoundEffectName _name;

    private void Start()
    {
        SoundManager.Instance.PlayEffect(_name);
    }
}
