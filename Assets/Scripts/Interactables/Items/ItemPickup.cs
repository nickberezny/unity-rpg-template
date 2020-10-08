using RPG;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ItemPickup : Interactable
{
    [SerializeField] private Item item;
    [SerializeField] private Color mouseOverColour, originalColor;

    private string _sceneName;
    private string _file;


    private void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        _file = "/" +  _sceneName + "/data.json";

        if (!DataManager.Instance.readObjectData(_file, name))
        {
            Debug.Log("Object " + name + " is set to deactive");
            Destroy(gameObject);
        }

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

        //Debug.Log((bool)DataManager.Instance.readData(_filepath, name));

        //set object permenantly deactive 

        DataManager.Instance.writeLevelData(_file, name, false);

        if (Inventory.Instance.Add(item))
            Destroy(gameObject);
        //open pickup menu
        
    }

}
