using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager Instance;
    public static GameSaveManager _Instance => Instance;

    [SerializeField] private IngredientSo _IngredientSo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }

    public void SaveGame(int i)
    {
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }
        BinaryFormatter Bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/Save_Data"+i+".txt");
        var json = JsonUtility.ToJson(_IngredientSo);
        Bf.Serialize(file, json);
        file.Close();
    }

    public void LoadGame(int i)
    {
        if (File.Exists(Application.persistentDataPath + "/game_save/Save_Data" + i + ".txt"))
        {
            BinaryFormatter Bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/Save_Data" + i + ".txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)Bf.Deserialize(file), _IngredientSo);
            file.Close();
        }
    }

    public bool IsItHaveSave(int i)
    {
        if (File.Exists(Application.persistentDataPath + "/game_save/Save_Data" + i + ".txt"))
        {
            return true;
        }
        else
            return false;
    }   

    public void DeleteSaveFile(int i)
    {
        File.Delete(Application.persistentDataPath + "/game_save/Save_Data" + i + ".txt");
    }
}
