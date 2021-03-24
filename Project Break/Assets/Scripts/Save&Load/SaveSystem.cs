using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(int Slot,PlayerMovement PM)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Data" + Slot + ".Soul";
        FileStream stream = new FileStream(path, FileMode.Create);

        SavedData data = new SavedData(PM);

        formatter.Serialize(stream,data);
        stream.Close();

        Debug.Log("Saved");
    }

    public static SavedData LoadData(int Slot)
    {
        string path = Application.persistentDataPath + "/Data" + Slot + ".Soul";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);

            SavedData data = formatter.Deserialize(stream) as SavedData;
            stream.Close();

            return data;
        }else
        {
            Debug.LogError("No File Data Found");
            return null;
        }
    }
}
