using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider _VolumeSlider;
    [SerializeField] bool _Master;
    [SerializeField] bool _Music;
    [SerializeField] bool _Effect;


    private void Awake()
    {
        if(SoundManager._Instance == null)
        {
            return;
        }

        if (_Master)
        {
            _VolumeSlider.value = AudioListener.volume;
        }

        if (_Music)
        {
            _VolumeSlider.value = SoundManager._Instance._MusicSource.volume;
        }

        if (_Effect)
        {
            _VolumeSlider.value = SoundManager._Instance._EffectSource.volume;
        }

    }
    private void Start()
    {
        if (_Master)
        {
            _VolumeSlider.onValueChanged.AddListener(val => SoundManager._Instance.ChangeMasterVolume(val));
        }

        if (_Music)
        {
            _VolumeSlider.onValueChanged.AddListener(val => SoundManager._Instance.ChangeMusicVolume(val));
        }

        if (_Effect)
        {
            _VolumeSlider.onValueChanged.AddListener(val => SoundManager._Instance.ChangeEffectVolume(val));
        }
    }
}
