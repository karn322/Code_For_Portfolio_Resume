using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector _Instance;
    public CharacterScriptableObject _CharacterData;

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
    }

    public static CharacterScriptableObject GetData()
    {
        return _Instance._CharacterData;
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        _CharacterData = character;
    }

    public void DestroySingleton()
    {
        _Instance = null;
        Destroy(gameObject);
    }
}
