using RPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] Canvas InventoryMenu;
    [SerializeField] Canvas HUD;

    private bool _menuOpen = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }

    }

    public void toggleInventoryMenu(bool open)
    {
        if(_menuOpen && open)
        {
            return;
        }

        _menuOpen = open;
        InventoryMenu.enabled = open;
        HUD.enabled = !open;
        GameManager.Instance.setMenuOpen(open);
    }

    public void openDialogueMenu(string name, Sprite portrait)
    {
        if (_menuOpen) return;

        _menuOpen = true;
        DialogueManager2.Instance.activateNewDialogue(name, portrait);
        HUD.enabled = false;
        GameManager.Instance.setMenuOpen(true);
    }

    public void closeDialogueMenu()
    {
        _menuOpen = false;
        DialogueManager2.Instance.closeDialogue();
        HUD.enabled = true;
        GameManager.Instance.setMenuOpen(false);
    }


            

        

    
}
