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

        private Camera _camera;

        private RaycastHit _hit;

        private const string groundTag = "Ground";
        private const string interactableTag = "Interactable";

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {

            if(_camera == null)
            {
                if(Camera.main == null)
                {
                    //Debug.Log("No Camera in Scene for Raycasting...");
                    return;
                }
                else
                {
                    _camera = Camera.main;
                }
            }

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
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
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
