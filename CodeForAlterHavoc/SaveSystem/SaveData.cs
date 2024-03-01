using PersistentAssetLite;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save",menuName = "SaveSlot/Save")]

public class SaveData : PersistentScriptableObject
{
    [Header("Sound")]
    public float _MasterVolume;
    public float _MusicVolume;
    public float _EffectVolume;

    [Header("Tutorial")]
    public bool _FirstTutorial;

    [Header("All Count")]
    public int _AllEnemyKill;
    public float _AllTimePLay;

    [Header("Collect")]
    public int[] _AllExpOrbCollect = new int[3];
    public int[] _AllGemCollect = new int[4];

    public int _AllPotionCollect;
    public int _AllUpgradeChestCollect;

    [Header("MonsterKill")]
    public int[] _MonsterKill = new int[9];
    public int[] _BossKill = new int[3];

    [Header("3 Last Save")]
    public List<LastGame> _LastGames;
    
    [System.Serializable]
    public class LastGame
    {
        public CharactorID _CharactorID;
        public int _EnemyKill;
        public float _PlayTime;
        public WeaponID[] _WeaponID = new WeaponID[6];
        public GemType[] _InventoryA = new GemType[9];
        public GemType[] _InventoryB = new GemType[9];
        public GemType[] _InventoryC = new GemType[9];
        public GemType[] _InventoryD = new GemType[9];
    }

    public void ClearData()
    {
        _FirstTutorial = false;

        _AllEnemyKill = 0;
        _AllTimePLay = 0;

        for (int i = 0; i < _AllExpOrbCollect.Length; i++)
        {
            _AllExpOrbCollect[i] = 0;
        }

        for (int i = 0; i < _AllGemCollect.Length; i++)
        {
            _AllGemCollect[i] = 0;
        }

        _AllPotionCollect = 0;
        _AllUpgradeChestCollect = 0;

        for (int i = 0; i < _MonsterKill.Length; i++)
        {
            _MonsterKill[i] = 0;
        }
        for (int i = 0; i < _BossKill.Length; i++)
        {
            _BossKill[i] = 0;
        }

        _LastGames.Clear();
    }
}
