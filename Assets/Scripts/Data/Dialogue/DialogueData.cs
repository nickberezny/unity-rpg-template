using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
    public string text;
    public int[] pointer;
    public string[] states;
    public string[] statesToSetTrue;
    public string[] statesToSetFalse;
    public bool active;
    public int entryState;
    public int newEntryState;
}