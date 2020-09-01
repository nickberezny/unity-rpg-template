using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private Item item;
    [SerializeField] private Color mouseOverColour, originalColor;

    private void Start()
    {
        originalColor = GetComponent<MeshRenderer>().material.color;
        if (mouseOverColour == new Color(0, 0, 0, 0))
            mouseOverColour = new Color(0.3f, 0.8f, 0.3f, 0.8f);
    }

    public override void Interact()
    {
        base.Interact(); //execute code in base interact method

        PickUp();
        //add to inventory 
        //make permanent
    }

    void PickUp()
    {
        
        Debug.Log("Picking up " + item.name);
        if (Inventory.Instance.Add(item))
            Destroy(gameObject);
        //open pickup menu
        
    }

    void OnMouseOver()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        GetComponent<MeshRenderer>().material.color = mouseOverColour;
    }

    void OnMouseExit()
    {
        // Reset the color of the GameObject back to normal
        GetComponent<MeshRenderer>().material.color = originalColor;
    }
}
