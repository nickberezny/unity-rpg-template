using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Transform interactionTransform;

    private void Start()
    {
        if (interactionTransform == null)
            interactionTransform = transform;
    }

    public virtual void Interact()
    {
        //override ...
        Debug.Log("interacting with " + name);
    }
}
