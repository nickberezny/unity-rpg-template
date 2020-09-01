using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    new public string name = "New Item"; //override name 

    [SerializeField] private Sprite _icon = null;
    [SerializeField] private bool _isDefaultItem = false;

    public bool isDefaultItem  { get; set; }
    public Sprite icon { get; set; }

    private void Awake()
    {
        isDefaultItem = _isDefaultItem;
        icon = _icon;
        return;
    }

    public virtual void Use()
    {
        //use the item 
        Debug.Log("Using " + name);
    }


}
