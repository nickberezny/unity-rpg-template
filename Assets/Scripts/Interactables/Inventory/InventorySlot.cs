using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void onItemButton()
    {
        
        if(item != null)
        {
            item.Use();
        }

        Inventory.Instance.Remove(item);
    }

    public void onRemoveButton()
    {
        Inventory.Instance.Remove(item);
    }
}
