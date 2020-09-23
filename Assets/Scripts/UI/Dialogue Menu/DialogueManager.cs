using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{ 
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject viewport;
    [SerializeField] private Text textBase;
    [SerializeField] private DialogueMenu dialogueMenu;
    [SerializeField] private Sprite defaultPortrait; 


    private Dictionary<int, DialogueData> _dialogueDictionary;
    private string _name;
    DialogueData _data;
    

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

        _dialogueDictionary = new Dictionary<int, DialogueData>();
        _data = new DialogueData();
        _name = "";
    }

    public void setEntryState(string name, int state)
    {
        Debug.Log("Entry point for " + name + "is now " + state);
        
        if(name == _name)
        {
            _dialogueDictionary[-1].entryState = state;
        }
        else
        {
            Debug.Log("names don't match: " + name + ", " + _name);
        }
        
    }

    public void activateNewDialogue(string name, Sprite portrait)
    {
        _dialogueDictionary = DataManager.Instance.readDialogueData("Assets/Data/Dialogue/" + name + ".json");
        _name = name;

        if (!portrait) portrait = defaultPortrait;

        dialogueMenu.activateNewDialogue(_name, portrait, _dialogueDictionary[-1].entryState);

    }

    public void closeDialogue( )
    {
        DataManager.Instance.writeDialogueData("Assets/Data/Dialogue/" + _name + ".json", _dialogueDictionary);
        dialogueMenu.closeMenu();
        _name = null;
        
      
    }


    public DialogueData getNextLine(string name, int selection)
    {
        if(_name == name)
        {
            if (!_dialogueDictionary.TryGetValue(selection, out _data))
            {
                Debug.Log("Cannot access requested dialogue string at " + selection.ToString() + " for file " + name);
                return null;
            }
            return _data;
        }

        Debug.Log("names don't match: " + name + ", " + _name);
        return null;

    }
  
}
