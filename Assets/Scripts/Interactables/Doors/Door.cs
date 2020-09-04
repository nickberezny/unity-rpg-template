using RPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    [SerializeField] private string ConnectingScene;
    [SerializeField] private int ConnectingDoorNumber;

    public override void Interact()
    {
        base.Interact(); //execute code in base interact method
    
        //save data
        //load scene
        //pass door # to load player

        GameManager.Instance.LoadScene(ConnectingScene, ConnectingDoorNumber);

    }

}
