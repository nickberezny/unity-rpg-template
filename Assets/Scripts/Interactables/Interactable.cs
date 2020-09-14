using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Transform interactionTransform;

    public Transform _interactionTransform {get; private set;}

    private void Awake()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        //Debug.Log("starting interactable" + name);

        _interactionTransform = interactionTransform;
    }

    public virtual void Interact()
    {
        //override ...
        Debug.Log("interacting with " + name);
    }
}
