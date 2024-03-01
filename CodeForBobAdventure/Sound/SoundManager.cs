using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Sound;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField] private AudioSource _MusicSource, _EffectsSource;
    [SerializeField] private Sound Sound;

    [SerializeField] private string[] _BattleSceneName;
    [SerializeField] private string[] _NormalSceneName;
    [SerializeField] private string _CookingSceneName;
    private string _CurrentSceneName = "";
    private bool _IsCheckScene = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_CurrentSceneName != SceneManager.GetActiveScene().name)
        {
            _CurrentSceneName = SceneManager.GetActiveScene().name;
            _IsCheckScene = false;
        }

        if (!_IsCheckScene)
        {
            _MusicSource.Pause();
            for (int i = 0; i < _BattleSceneName.Length; i++)
            {
                if (_CurrentSceneName == _BattleSceneName[i])
                {
                    PlayMusic(SoundMusicName.BattleBG);
                }
            }

            for (int i = 0; i < _NormalSceneName.Length; i++)
            {
                if (_CurrentSceneName == _NormalSceneName[i])
                {
                    PlayMusic(SoundMusicName.NormalBG);
                }
            }

            if (_CurrentSceneName == _CookingSceneName)
            {
                PlayMusic(SoundMusicName.CookingBG);
            }

            _IsCheckScene = true;
        }        
    }

    public void PlayEffect(SoundEffectName name)
    {
        AudioClip clip = null;
        foreach (SoundEffectAsset _SoundEffectArray in Sound._SoundEffectArray)
        {
            if (_SoundEffectArray.name == name)
            {
                clip = _SoundEffectArray.clip;
            }
        }
        _EffectsSource.PlayOneShot(clip);
    }

    public void PlayMusic(SoundMusicName name)
    {
        AudioClip clip = null;
        foreach (SoundMusicAsset _SoundMusicArray in Sound._SoundMusicArray)
        {
            if (_SoundMusicArray.name == name)
            {
                clip = _SoundMusicArray.clip;
            }
        }
        _MusicSource.clip = clip;
        _MusicSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
}
