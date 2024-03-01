using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _Instance;
    [SerializeField] SaveData _SaveData;

    public AudioSource _MusicSource;
    public AudioSource _EffectSource;  

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioListener.volume = _SaveData._MasterVolume;
        _MusicSource.volume = _SaveData._MusicVolume;
        _EffectSource.volume = _SaveData._EffectVolume;
    }

    public void PlaySound(AudioClip clip)
    {
        _EffectSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float num)
    {
        AudioListener.volume = num;
        _SaveData._MasterVolume = num;
    }

    public void ChangeMusicVolume(float num)
    {
        _MusicSource.volume = num;
        _SaveData._MusicVolume = num;
    }

    public void ChangeEffectVolume(float num)
    {
        _EffectSource.volume = num;
        _SaveData._EffectVolume = num;
    }
}
