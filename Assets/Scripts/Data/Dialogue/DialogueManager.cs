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

    /*
    private void Start()
    {
        _previousText = textBase;
        
        
        Text newText = Instantiate(textBase);

        newText.text = "hello from dialogue manager manager mananger mananger overflow overflow overflow overflow";

        Canvas.ForceUpdateCanvases();;

        newText.transform.SetParent(viewport.transform);
        newText.transform.position = new Vector2(_previousText.transform.position.x, _previousText.transform.position.y - _previousText.preferredHeight);
    

        viewport.GetComponent<RectTransform>().sizeDelta += new Vector2(0, newText.preferredHeight);
        //newText.rectTransform.offsetMax = new Vector2(_previousText.rectTransform.offsetMax.x, _previousText.rectTransform.offsetMax.y + 100);
        //newText.rectTransform.offsetMin = new Vector2(_previousText.rectTransform.offsetMin.x, _previousText.rectTransform.offsetMin.y - 100);

        // newText.rectTransform.anchorMax



    }
    */

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
