using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSoundOnCondition : MonoBehaviour
{
    [SerializeField] private AudioClip _Clip;
    public bool _Trigger;
    private bool _Hit = true;

    private void Update()
    {
        if(!_Trigger)
        {
            _Hit = true;
        }

        if(_Trigger && _Hit)
        {
            PlaySoundEffect();
            _Trigger = false;
            _Hit = false;
        }
    }

    public void PlaySoundEffect()
    {
        SoundManager._Instance.PlaySound(_Clip);
    }
}
