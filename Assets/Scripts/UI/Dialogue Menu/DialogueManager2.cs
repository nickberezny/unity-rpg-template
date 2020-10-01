using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager2 : MonoBehaviour
{
    public static DialogueManager2 Instance { get; private set; }

    [SerializeField] Sprite defaultPortrait;
    [SerializeField] DialogueMenu2 dialogueMenu;

    private Dictionary<int, DialogueData> _dialogueDictionary;
    private string _name;
    private int[] _index;

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
        _name = "";
    }

    public void activateNewDialogue(string name, Sprite portrait)
    {
        _dialogueDictionary = DataManager.Instance.readDialogueData("Assets/Data/Dialogue/" + name + ".json");
        _name = name;

        if (!portrait) portrait = defaultPortrait;

        if(dialogueMenu.openDialogueMenu()) addNextLine(new int[] { _dialogueDictionary[-1].entryState });

        //dialogueMenu.activateNewDialogue(_name, portrait, _dialogueDictionary[-1].entryState);
    }

    public void closeDialogue()
    {
        DataManager.Instance.writeDialogueData("Assets/Data/Dialogue/" + _name + ".json", _dialogueDictionary);
        dialogueMenu.closeDialogueMenu();
    }

    public void continueToNextLine(int index, bool markAsSelected = false)
    {
        DialogueData dialogueData = _dialogueDictionary[_index[index]];
        DataManager.Instance.setGameStateTrue(dialogueData.statesToSetTrue);
        DataManager.Instance.setGameStateFalse(dialogueData.statesToSetFalse);

        if (dialogueData.newEntryState > 0) _dialogueDictionary[-1].entryState = dialogueData.newEntryState;

        Debug.Log("mark as selected:" + markAsSelected);
        if (markAsSelected) dialogueData.active = false;

        addNextLine(dialogueData.pointer);
        Debug.Log("Continue to " + index.ToString());
    }

    private void addNextLine(int[] pointer)
    {
        if (pointer[0] == 0)
        {
            closeDialogue();
            return;
        }
        
        int len = pointer.Length;
        if(len == 1)
        {
            dialogueMenu.addText(_dialogueDictionary[pointer[0]].text, _name);
            _index = new int[1];
            _index[0] =  pointer[0];
        }
        else if(len > 1)
        {
            _index = new int[5];
            string[] optionText = new string[5];
            bool[] isActive = new bool[5];

            int ii = 0;

            for (int i = 0; i < len; i++)
            {
                
                bool checkStates = true;

                for (int j = 0; j < _dialogueDictionary[pointer[i]].states.Length; j++)
                {
                    if (!DataManager.Instance.getGameState(_dialogueDictionary[pointer[i]].states[j]))
                    {
                        checkStates = false;
                    }
                }

                if(checkStates)
                {
                    optionText[ii] = _dialogueDictionary[pointer[i]].text;
                    _index[ii] = pointer[i];
                    isActive[ii] = _dialogueDictionary[pointer[i]].active;
                    Debug.Log(isActive[ii]);
                    ii++;
                }
                
            }

            dialogueMenu.addOptions(optionText, isActive, ii );
        }
    }
}
