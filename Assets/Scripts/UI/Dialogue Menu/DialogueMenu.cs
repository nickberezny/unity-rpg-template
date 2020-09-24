using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueMenu : MonoBehaviour
{ 

    [SerializeField] private Text textBase;
    [SerializeField] private Button continueButtonBase;
    [SerializeField] private GameObject viewport;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Image portraitImage;

    private Canvas dialogueMenuCanvas;
    private DialogueData[] _dialogueArray = new DialogueData[6];
    private Text _previousText;
    private Text _currentText;
    private RectTransform viewportRectTransform;
    private string _activeName;
    private GameObject[] _addedText = new GameObject[100];
    private int _addedTextIndex = 0;
    private Button _continueButton = null;

    private const float defaultVerticalOffset = 10f;
    private const float defaultHorizontalOffset = 10f;
    private const float horizontalIndent = 5f;


    private void Awake()
    {
        DontDestroyOnLoad(this);

        _currentText = textBase;
        viewportRectTransform = viewport.GetComponent<RectTransform>();
        dialogueMenuCanvas = gameObject.GetComponent<Canvas>();
        dialogueMenuCanvas.enabled = false;
        
    }
    


    public void activateNewDialogue(string name, Sprite portrait, int entryState)
    {
        if(_activeName != null || dialogueMenuCanvas.enabled)
        {
            Debug.Log("Dialogue menu already open");
            return;
        }

        if(portrait != null)
        {
            portraitImage.sprite = portrait;
        }


        _activeName = name;
        dialogueMenuCanvas.enabled = true;
        cameraManager.moveCameraForDialogue(true);
        Debug.Log(_activeName);
        addNextLine(_activeName, new int[] { entryState });
 
    }

    public void closeMenu()
    {
        dialogueMenuCanvas.enabled = false;
        cameraManager.moveCameraForDialogue(false);
        _activeName = null;
        _currentText = textBase;
        _previousText = _currentText;

        for(int i = 0; i < _addedTextIndex; i++)
        {
            Destroy(_addedText[i]);
        }

        Array.Clear(_addedText, 0, _addedTextIndex+1);
        _addedTextIndex = 0;
    }


    private void addNextLine(string name, int[] selection)
    {

        if(selection[0] == 0)
        {
            DialogueManager.Instance.closeDialogue();
            return;
        }
        if(selection[0] == -1)
        {
            selection [0] = dialogueManager.getNextLine(name, -1).entryState;
        }

        bool tagAsOption = false;
        int len = selection.Length;
        float horizontalOffset = defaultHorizontalOffset;

        if(_continueButton != null) Destroy(_continueButton.gameObject);

        if (len > 1)
        {
            tagAsOption = true;
            horizontalOffset += horizontalIndent;
        }

        for(int i = 0; i < len; i++)
        {
            _dialogueArray[i] = dialogueManager.getNextLine(name, selection[i]);

            bool checkStates = true;

            for(int j = 0; j < _dialogueArray[i].states.Length; j++)
            {
                Debug.Log(i.ToString() + "," + j.ToString());
                if(!DataManager.Instance.getGameState(_dialogueArray[i].states[j]))
                {
                    checkStates = false;
                }
            }

            if(checkStates) addText(_dialogueArray[i], defaultVerticalOffset, horizontalOffset, tagAsOption);
        }

        if (len == 1 && _dialogueArray[0].pointer.Length > 1)
        {
            StartCoroutine(delayedAddLine(1f, name, _dialogueArray[0].pointer));
        }
        else if(len == 1 && _dialogueArray[0].pointer.Length == 1)
        {
            Debug.Log("Creating Button");
            _continueButton = Instantiate(continueButtonBase, viewport.transform);
            addObjectToList(_continueButton.gameObject);
            if (_dialogueArray[len - 1].newEntryState > 0) _continueButton.onClick.AddListener(delegate { dialogueManager.setEntryState(name, _dialogueArray[len - 1].newEntryState); });
            _continueButton.transform.position = new Vector2(_continueButton.transform.position.x, _previousText.transform.position.y - _previousText.preferredHeight - defaultVerticalOffset);
            _continueButton.onClick.AddListener(delegate { addNextLine(name, _dialogueArray[len - 1].pointer);});
           
        }
        
        
        return;
    }

    private IEnumerator delayedAddLine(float delay, string name, int[] selection)
    {
        yield return new WaitForSeconds(delay);
        addNextLine(name, selection);

    }

    private void addText(DialogueData dialogueData, float verticalOffset, float horizontalOffset, bool tagAsOption)
    {
        Debug.Log("added text");
        if (_currentText != null)
        {
            Debug.Log("added text");
            _previousText = _currentText;
            _currentText = Instantiate(textBase);
            
            addObjectToList(_currentText.gameObject);

            _currentText.text = dialogueData.text ;
            _currentText.transform.SetParent(viewport.transform);
            _currentText.transform.position = new Vector2(_previousText.transform.position.x, _previousText.transform.position.y - _previousText.preferredHeight - verticalOffset);
            _currentText.rectTransform.offsetMax = new Vector2(_previousText.rectTransform.offsetMax.x, _currentText.rectTransform.offsetMax.y);
            _currentText.rectTransform.offsetMin = new Vector2(textBase.rectTransform.offsetMin.x + horizontalOffset, _currentText.rectTransform.offsetMin.y);
            
            if (tagAsOption)
            {
                string name = _activeName;
                _currentText.tag = "DialogueOption";
                Button tempButton = _currentText.GetComponent<Button>();
                tempButton.enabled = true; 
                tempButton.onClick.AddListener(delegate{addNextLine(_activeName, dialogueData.pointer);});
                tempButton.onClick.AddListener(delegate { DataManager.Instance.setGameStateTrue(dialogueData.statesToSetTrue); });
                tempButton.onClick.AddListener(delegate { DataManager.Instance.setGameStateFalse(dialogueData.statesToSetFalse); });
                if (dialogueData.newEntryState > 0) tempButton.onClick.AddListener(delegate { dialogueManager.setEntryState(name, dialogueData.newEntryState);  });
                Debug.Log("Selection added");

            }

            viewportRectTransform.sizeDelta += new Vector2(0, _currentText.preferredHeight + verticalOffset);
            StartCoroutine(zeroScrollbar());

        }
        else
        {
            Debug.Log("Adding text failed");
        }
        
    }

    private IEnumerator zeroScrollbar()
    {
        yield return null;
        scrollbar.value = 0;
    }

    private void addObjectToList(GameObject go)
    {
        _addedText[_addedTextIndex] = go;
        _addedTextIndex += 1;
        return;
    }


}
