using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public Transform itemsParent;

    InventorySlot[] slots;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangeCallback += UpdateUI; //subscribe to callback
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }


    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
