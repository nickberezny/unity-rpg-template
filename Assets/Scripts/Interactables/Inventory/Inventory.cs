using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Singleton
    //inventory sigleton
    public static Inventory Instance { get; private set; }

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

    #endregion

    public delegate void OnItemChange();
    public OnItemChange onItemChangeCallback;

    public int space = 20;
    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        //add item to inventory, invoke delegate
        if(!item.isDefaultItem)
        {
            if(items.Count >= space)
            {
                Debug.Log("Not enough space in inventory");
                return false;
            }

            items.Add(item);

            if(onItemChangeCallback != null)
                onItemChangeCallback.Invoke();

            return true;
        }

        return false;

    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangeCallback != null)
            onItemChangeCallback.Invoke();
    }
}
