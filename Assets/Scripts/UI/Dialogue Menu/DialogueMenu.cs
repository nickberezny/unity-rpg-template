using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : MonoBehaviour
{ 
     public static DialogueMenu Instance { get; private set; }

    [SerializeField] private Text textBase;
    [SerializeField] private Button continueButton;
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

    private const float defaultVerticalOffset = 10f;
    private const float defaultHorizontalOffset = 10f;
    private const float horizontalIndent = 5f;


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
            _currentText = textBase;
            viewportRectTransform = viewport.GetComponent<RectTransform>();
            dialogueMenuCanvas = gameObject.GetComponent<Canvas>();
            dialogueMenuCanvas.enabled = false;
        }
    }
    


    public void activateNewDialogue(string name, Sprite portrait)
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
        Debug.Log("Activating");
        dialogueMenuCanvas.enabled = true;
        cameraManager.moveCameraForDialogue(true);
        int[] temp = { 1 };
        //Debug.Log("temp!" + temp[0].ToString());
        addNextLine(name, temp);
 
    }

    public void closeMenu()
    {
        dialogueMenuCanvas.enabled = false;
        cameraManager.moveCameraForDialogue(false);
        _activeName = null;
    }


    private void addNextLine(string name, int[] selection)
    {
        bool tagAsOption = false;
        int len = selection.Length;
        float horizontalOffset = defaultHorizontalOffset;

        if (len > 1)
        {
            tagAsOption = true;
            horizontalOffset += horizontalIndent;
        }

        for(int i = 0; i < len; i++)
        {
            _dialogueArray[i] = dialogueManager.getNextLine(name, selection[i]);
            addText(_dialogueArray[i].text, defaultVerticalOffset, horizontalOffset, tagAsOption);
        }

        if (len == 1 && _dialogueArray[0].pointer.Length > 1)
        {
            StartCoroutine(delayedAddLine(1f, name, _dialogueArray[0].pointer));
        }
        else if(len ==1 && _dialogueArray[0].pointer.Length == 1)
        {
            Instantiate(continueButton);
            continueButton.transform.position = new Vector2(continueButton.transform.position.x, _previousText.transform.position.y - _previousText.preferredHeight - defaultVerticalOffset);
        }
        
        
        return;
    }

    private IEnumerator delayedAddLine(float delay, string name, int[] selection)
    {
        Debug.Log("Waiting to add line");
        yield return new WaitForSeconds(delay);
        addNextLine(name, selection);

    }

    private void addText(string text, float verticalOffset, float horizontalOffset, bool tagAsOption)
    {
        if (_currentText != null)
        {
            _previousText = _currentText;
            _currentText = Instantiate(textBase);

            _currentText.text = text;
            _currentText.transform.SetParent(viewport.transform);
            _currentText.transform.position = new Vector2(_previousText.transform.position.x, _previousText.transform.position.y - _previousText.preferredHeight - verticalOffset);
            _currentText.rectTransform.offsetMax = new Vector2(_previousText.rectTransform.offsetMax.x, _currentText.rectTransform.offsetMax.y);
            _currentText.rectTransform.offsetMin = new Vector2(textBase.rectTransform.offsetMin.x + horizontalOffset, _currentText.rectTransform.offsetMin.y);
            if (tagAsOption) _currentText.tag = "DialogueOption";

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

}
