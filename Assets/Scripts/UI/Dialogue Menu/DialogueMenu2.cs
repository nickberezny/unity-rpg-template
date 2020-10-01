﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu2 : MonoBehaviour
{

    [SerializeField] private Text textBase;
    [SerializeField] private GameObject viewport;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Button continueButtonBase;
    [SerializeField] Color selectedColor;

    private Text[] _addedText = new Text[100];
    private int _textIndex = 0;
    private Text _currentText;
    private Text _previousText;
    private int[] _optionIndices = new int[5];
    private RectTransform viewportRectTransform;
    private Canvas dialogueMenuCanvas;
    private Button _continueButton = null;

    private const float defaultVerticalOffset = 10f;
    private const float defaultHorizontalOffset = 10f;
    private const float defaultHorizontalIndent = 20f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        viewportRectTransform = viewport.GetComponent<RectTransform>();

        dialogueMenuCanvas = gameObject.GetComponent<Canvas>();
        dialogueMenuCanvas.enabled = false;

        _addedText[0] = textBase;
    }

    public void addText(string text, string speakerName, bool addContinueButton = true, float horizontalIndent = 0)
    {
        if (_continueButton) GameObject.Destroy(_continueButton.gameObject);
        _currentText = Instantiate(textBase);
        _previousText = _addedText[_textIndex];
        addToTextArray(_currentText);

        _currentText.text = speakerName + " : " + text;

        _currentText.transform.SetParent(viewport.transform);
        _currentText.transform.position = new Vector2(_previousText.transform.position.x, _previousText.transform.position.y - _previousText.preferredHeight - defaultVerticalOffset);
        _currentText.rectTransform.offsetMax = new Vector2(_previousText.rectTransform.offsetMax.x, _currentText.rectTransform.offsetMax.y);
        _currentText.rectTransform.offsetMin = new Vector2(textBase.rectTransform.offsetMin.x + defaultHorizontalOffset + horizontalIndent, _currentText.rectTransform.offsetMin.y);

        viewportRectTransform.sizeDelta += new Vector2(0, _currentText.preferredHeight + defaultVerticalOffset);

        if(addContinueButton)
        {
            Debug.Log("Creating Button");
            _continueButton = Instantiate(continueButtonBase, viewport.transform);
            _continueButton.transform.position = new Vector2(_continueButton.transform.position.x, _previousText.transform.position.y - _previousText.preferredHeight - defaultVerticalOffset);
            _continueButton.onClick.AddListener(delegate { DialogueManager2.Instance.continueToNextLine(0); });
        }

        StartCoroutine(zeroScrollbar());

    }

    public void addOptions(string[] text, bool[] isActive, int len)
    {
        for(int i = 0; i < len; i++)
        {
            addText(text[i], i.ToString(), false, defaultHorizontalIndent);
            //if (!isActive[i]) _addedText[_textIndex].color = selectedColor;
            _optionIndices[i] = _textIndex;

            Button tempButton = _addedText[_textIndex].GetComponent<Button>();
            if (!isActive[i])
            {
                ColorBlock cb = tempButton.colors;
                cb.normalColor = selectedColor;
                tempButton.colors = cb;
                Debug.Log("Changing colours");
            }
            tempButton.enabled = true;
            int tempIndex = i;
            tempButton.onClick.AddListener(delegate { deleteOptions(tempIndex); });
            Debug.Log("Selection added");

        }
    }

    private void deleteOptions(int exception)
    {
        string saveException = "";

        for(int i = 0; i < _optionIndices.Length; i++)
        {
            if (_optionIndices[i] == 0) break;
            Debug.Log("Option: " + _optionIndices[i]);
            if (i == exception) saveException = _addedText[_optionIndices[i]].text;
            Destroy(_addedText[_optionIndices[i]].gameObject);
            _textIndex--;
        }

        addText(saveException, "You", false);
  
        DialogueManager2.Instance.continueToNextLine(exception, true);
    }    

    public bool openDialogueMenu()
    {
        if (!dialogueMenuCanvas.enabled)
        {
            dialogueMenuCanvas.enabled = true;
            return true;
        }
        else
        {
            Debug.Log("Dialogue menu already open");
            return false;
        }
    }

    public void closeDialogueMenu()
    {
        for(int i = 1; i <= _textIndex; i++)
        {
            GameObject.Destroy(_addedText[i].gameObject);
            Debug.Log(i);
        }
        _textIndex = 0;
        dialogueMenuCanvas.enabled = false;


    }

    private IEnumerator zeroScrollbar()
    {
        yield return null;
        scrollbar.value = 0;
    }

    public void addToTextArray(Text newText)
    {
        _textIndex++;
        _addedText[_textIndex] = newText;
        Debug.Log(_textIndex);
    }
}
