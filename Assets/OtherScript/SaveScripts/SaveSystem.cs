using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    public static void NewSaveData(int SaveNum, CharactersSO CharacterData,PlayerMovement PlayerMoveData)
    {
        BinaryFormatter Formatter = new BinaryFormatter();

        string SavePath = $"{Application.persistentDataPath}/Save{SaveNum}.TeamDexterity";
        FileStream Stream = new FileStream(SavePath, FileMode.Create);

        SaveData Data = new SaveData(CharacterData, PlayerMoveData);

        Formatter.Serialize(Stream, Data);
        Stream.Close();
    }

    public static SaveData LoadData(int LoadNum)
    {
        string LoadPath = $"{Application.persistentDataPath}/Save{LoadNum}.TeamDexterity";
        if(File.Exists(LoadPath))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(LoadPath, FileMode.Open);

            SaveData Data = Formatter.Deserialize(Stream) as SaveData;
            Stream.Close();
            return Data;
        }
        else
        {
            return null;
        }
    }
}