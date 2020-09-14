using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : Interactable
{

    [SerializeField] private Sprite _portrait = null;

    public override void Interact()
    {
        base.Interact();
        DialogueMenu.Instance.activateNewDialogue(name, _portrait);
    }


}

