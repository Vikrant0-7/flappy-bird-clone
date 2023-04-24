using System;
using Godot;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveData
{
    public int worldGenerator;
    public int hardnessLevel;

    public int[] highscore_easy;
    public int[] highscore_normal;
    public int[] highscore_hard;

    bool isInvalidInstance = false;

    public SaveData(){
        isInvalidInstance = true;
    }

    public bool IsNull(){
        return isInvalidInstance;
    }


    public SaveData(int worldGenerator, int hardnessLevel, int[] highscore_easy, int[] highscore_normal, int[] highscore_hard)
    {
        this.worldGenerator = worldGenerator;
        this.hardnessLevel = hardnessLevel;
        this.highscore_easy = highscore_easy;
        this.highscore_normal = highscore_normal;
        this.highscore_hard = highscore_hard;
        this.isInvalidInstance = false;
    }
}

public static class SaveSystem{
    public static void Save(SaveData data){
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = OS.GetUserDataDir() + "/vikrant.vicky";
        FileStream fileStream = new FileStream(path,FileMode.Create);

        binaryFormatter.Serialize(fileStream,data);
        fileStream.Close();
    }

    public static SaveData Load(){
        string path = OS.GetUserDataDir() + "/vikrant.vicky";
        if(System.IO.File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else{
            return new SaveData();
        }
    }
}
