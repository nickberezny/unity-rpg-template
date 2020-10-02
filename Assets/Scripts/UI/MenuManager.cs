using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Canvas InventoryMenu;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void toggleInventoryMenu(bool open)
    {
        InventoryMenu.enabled = open;
    }
}
