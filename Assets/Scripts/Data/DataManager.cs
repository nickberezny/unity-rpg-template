using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine.Internal.VR;
using System.Dynamic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private JsonSerializerSettings settings = new JsonSerializerSettings();

    private Dictionary<string, bool> levelDictionary;
    private Dictionary<int, DialogueData> dialogueDictionary;
    private Dictionary<string, bool> stateDictionary;

    private const string pathToStates = "Assets/Data/States.json";

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
            settings.Formatting = Formatting.Indented;
            stateDictionary = loadGameState();
            //stateDictionary.Add("null", true);
            if(stateDictionary.TryGetValue("null", out bool trash))
            {
                stateDictionary["null"] = true;
            }
            else
            {
                stateDictionary.Add("null", true);
            }
        }

    }

    public void resetData()
    {
        //RESETS ALL SAVED DATA!

        string filepath = "Assets/Scenes";

        string[] levels = Directory.GetDirectories(filepath);
    
        for(int i = 0; i< levels.Length; i++)
        {
            string level = levels[i].Substring(0, levels[i].Length ); //remove backslash

            StreamReader reader = new StreamReader(level + "/Data_original.json");
            string data = reader.ReadToEnd();

            StreamWriter writer = new StreamWriter(level + "/Data.json", false);
            writer.WriteLine(data);

            reader.Close();
            writer.Close();

        }

        string[] dialogue = Directory.GetFiles("Assets/Data/Dialogue","*json");
        

        for(int i = 0; i< dialogue.Length; i++)
        {
            
            dialogue[i].Replace("\\","/");
            Dictionary<int, DialogueData> dict = readDialogueData(dialogue[i]);
            dict[-1].entryState = 1;
            writeDialogueData(dialogue[i], dict);

        }

        string[] tempKeys = new string[stateDictionary.Count];
        int j = 0;

        foreach(KeyValuePair<string,bool> keys in stateDictionary )
        {
            tempKeys[j] = keys.Key;
            j = j + 1;
        }

        for(int i = 0; i < tempKeys.Length; i++)
        {
            if(tempKeys[i] != "null") stateDictionary[tempKeys[i]] = false;
        }

        saveGameState(stateDictionary);

    }

    public bool getGameState(string key)
    {
        return stateDictionary[key];
    }

    public void setGameStateTrue(string[] key)
    {
        if(key != null)
        {
            for (int i = 0; i < key.Length; i++)
            {
                if (stateDictionary.TryGetValue(key[i], out bool val))
                {
                    Debug.Log("Setting state : " + key[i]);
                    stateDictionary[key[i]] = true;
                }
                else
                {
                    Debug.Log("State doesn't exist...");
                }
            }
        }
    }

    public void setGameStateFalse(string[] key)
    {
        if (key != null)
        {
            for (int i = 0; i < key.Length; i++)
            {
                if (stateDictionary.TryGetValue(key[i], out bool val))
                {
                    stateDictionary[key[i]] = false;
                }
                else
                {
                    Debug.Log("State doesn't exist...");
                }
            }
        }

    }

    private Dictionary<string,bool> loadGameState()
    {
        StreamReader reader = new StreamReader(pathToStates);
        return JsonConvert.DeserializeObject<Dictionary<string, bool>>(reader.ReadToEnd());
    }



    private void saveGameState(Dictionary<string, bool> gameStates)
    {
        StreamWriter writer = new StreamWriter(pathToStates, false);
        writer.WriteLine(JsonConvert.SerializeObject(gameStates, settings));
        writer.Close();
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

    public Dictionary<int,DialogueData> readDialogueData(string filepath)
    {
     
        dialogueDictionary = new Dictionary<int, DialogueData>();
        StreamReader reader = new StreamReader(filepath);
        dialogueDictionary = JsonConvert.DeserializeObject<Dictionary<int, DialogueData>>(reader.ReadToEnd(), settings);
        Debug.Log(dialogueDictionary[1].text);
        reader.Close();

        StreamWriter writer = new StreamWriter("Assets/TestDialogue.json", false);
        writer.WriteLine(JsonConvert.SerializeObject(dialogueDictionary, Formatting.Indented));
        writer.Close();

        return dialogueDictionary;
    }

    public void writeDialogueData(string filepath, Dictionary<int, DialogueData> dict)
    {
        StreamWriter writer = new StreamWriter(filepath, false);
        writer.WriteLine(JsonConvert.SerializeObject(dict, Formatting.Indented));
        writer.Close();

        Debug.Log("Dialogue Data saved");

        return;
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
