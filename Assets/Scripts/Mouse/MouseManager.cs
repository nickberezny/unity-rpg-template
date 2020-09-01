using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPG.Mouse
{
    public class MouseManager : MonoBehaviour
    {

        [SerializeField] private UnityEvent<RaycastHit> GroundSelected;
        [SerializeField] private UnityEvent<RaycastHit> InteractableSelected;

        private RaycastHit _hit;

        private const string groundTag = "Ground";
        private const string interactableTag = "Interactable";

        


        void Update()
        {

            if (EventSystem.current.IsPointerOverGameObject())
            {
                //mouse over UI
                //make sure cursor is menu cursor
                return;
            }
            else
            {

                if (CheckRaycastCollision())
                {
                    //change cursor, highlight, etc
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_hit.collider != null) HandleMouseClick();
                else Debug.Log("Nothing to click...");
            }

        }

        private bool CheckRaycastCollision()
        {
            //use raycast to find gameobject intersecting with cursor
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return (Physics.Raycast(ray, out _hit));
        }

        private void HandleMouseClick()
        {

            switch (_hit.collider.tag)
            {
                case groundTag:
                    GroundSelected.Invoke(_hit);
                    break;
                case interactableTag: 
                    InteractableSelected.Invoke(_hit);
                    break;
                default:
                    break;
            }
            return;
        }

        public void testing()
        {
            return;
        }    
    }
}
