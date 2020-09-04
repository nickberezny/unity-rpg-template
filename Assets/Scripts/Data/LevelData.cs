using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public Dictionary<string, string> objects = new Dictionary<string, string>();
}

