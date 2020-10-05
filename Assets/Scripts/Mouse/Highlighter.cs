using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
   
    
    [SerializeField] private Color highlightColor;

    private Color previousColor;
    private GameObject highlightedObject;
    

    public void highlightObject(GameObject gameObject)
    {
        if (highlightedObject) highlightedObject.GetComponent<Renderer>().material.color = previousColor;
        highlightedObject = gameObject;

        Renderer renderer = gameObject.GetComponent<Renderer>();
        previousColor = renderer.material.color;
        renderer.material.color = highlightColor;
    }

    public void deHighlightObject()
    {
        if (highlightedObject) highlightedObject.GetComponent<Renderer>().material.color = previousColor;
        highlightedObject = null;
    }
}
 