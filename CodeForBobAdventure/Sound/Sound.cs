using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public enum SoundEffectName { PlayerAttack, cook, finishCook, OpenMap, OpenPot}
    public enum SoundMusicName { BattleBG, NormalBG, CookingBG}

    public SoundEffectAsset[] _SoundEffectArray;

    public SoundMusicAsset[] _SoundMusicArray;

    [System.Serializable] 
    public class SoundEffectAsset{
        public SoundEffectName name;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SoundMusicAsset{
        public SoundMusicName name;
        public AudioClip clip;
    }

}
