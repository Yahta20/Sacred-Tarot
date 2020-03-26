using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad 
{

    private static Progress prog = new Progress();

    public static void saveData(Progress p)
    {
        
        SaveLoad.prog = p;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/progress.dt");
            bf.Serialize(file, SaveLoad.prog);
            file.Close();
    }
    public static Progress loadData()
    {
        if (File.Exists(Application.persistentDataPath + "/progress.dt"))
        {
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/progress.dt", FileMode.Open);
            SaveLoad.prog = (Progress)bf.Deserialize(file);
            file.Close();
        }
        return SaveLoad.prog;
    }
    public static void fileDelete() {
        if (File.Exists(Application.persistentDataPath + "/progress.dt"))
        {
            File.Delete(Application.persistentDataPath + "/progress.dt");
        }
    }

}
