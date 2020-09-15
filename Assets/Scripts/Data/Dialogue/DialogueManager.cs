using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject viewport;
    [SerializeField] private Text textBase;

    private Text _previousText;

    private Dictionary<int, DialogueData> _dialogueDictionary;
    private string _name;
    DialogueData _data;
    

    private void Awake()
    {
        _dialogueDictionary = new Dictionary<int, DialogueData>();
        _data = new DialogueData();
        _name = "";
    }

    public DialogueData getNextLine(string name, int selection)
    {
        if(_name != name)
        {
            _dialogueDictionary = DataManager.Instance.readDialogueData("Assets/Data/Dialogue/" + name + ".json");

            if (_dialogueDictionary == null)
            {
                Debug.Log("Dialogue file not found for" + name);
                return null;
            }
        }

        if(!_dialogueDictionary.TryGetValue(selection, out _data))
        {
            Debug.Log("Cannot access requested dialogue string at " + selection.ToString() + " for file " + name);
            return null;
        }

        Debug.Log(_data.text);

        return _data;
        
    }
  
}
