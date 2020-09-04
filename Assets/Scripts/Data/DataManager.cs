using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using UnityEditor;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

    }

    private Dictionary<string,bool> levelDictionary;

    public void resetData()
    {
        //RESETS ALL SAVED DATA!

        string filepath = "Assets/Scenes";

        string[] levels = Directory.GetDirectories(filepath);
        for(int i = 0; i< levels.Length; i++)
        {
            string level = levels[i].Substring(0, levels[i].Length ); //remove backslash

            Debug.Log(level);
            StreamReader reader = new StreamReader(level + "/Data_original.json");
            string data = reader.ReadToEnd();

            StreamWriter writer = new StreamWriter(level + "/Data.json", false);
            writer.WriteLine(data);

            reader.Close();
            writer.Close();

            
        }


    }

    public void initializeLevelData(string sceneName)
    {
        string filepath = "Assets/Scenes/" + sceneName;
        if (!File.Exists(filepath + "/Data.json"))
        {
            StreamReader reader = new StreamReader(filepath + "/Data_original.json");
            string data = reader.ReadToEnd();

            StreamWriter writer = new StreamWriter(filepath + "/Data.json", false);
            writer.WriteLine(data);

            reader.Close();
            writer.Close();

        }
        else
        {
            Debug.Log("Data file exists, cannot initialize level");
        }

    }

    public bool readObjectData(string filepath, string objectName)
    {
        StreamReader reader = new StreamReader(filepath);
       
        return JsonConvert.DeserializeObject<Dictionary<string, bool>>(reader.ReadToEnd())[objectName];
    }

    public Dictionary<string, bool> readLevelData(string filepath)
    {
        levelDictionary = new Dictionary<string, bool>();
        StreamReader reader = new StreamReader(filepath);
        levelDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(reader.ReadToEnd());
        reader.Close();

        return levelDictionary;

    }

    public void writeLevelData(string filepath, string variable, bool value)
    {
        levelDictionary = new Dictionary<string, bool>();
        StreamReader reader = new StreamReader(filepath);
        levelDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(reader.ReadToEnd());
        reader.Close();

        levelDictionary[variable] = value;

        StreamWriter writer = new StreamWriter(filepath, false);
        writer.WriteLine(JsonConvert.SerializeObject(levelDictionary));
        writer.Close();

        return;
    }

}
