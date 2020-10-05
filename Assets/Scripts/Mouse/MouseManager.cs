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
        [SerializeField] private Highlighter highlighter;

        private Camera _camera;

        public bool menuOpen { private set; get; }

        private RaycastHit _hit;
        private GameObject _cursorOverGameObject;
        private GameObject _previousCursorOverGameObject;

        private const string groundTag = "Ground";
        private const string interactableTag = "Interactable";

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _previousCursorOverGameObject = null;
        }

        private void Update()
        {

            if (_camera == null)
            {
                if (Camera.main == null)
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
            else if (!menuOpen)
            {

                if (CheckRaycastCollision())
                {

                    if (_cursorOverGameObject != _previousCursorOverGameObject)
                    {
                        if (_cursorOverGameObject.tag == "Interactable") highlighter.highlightObject(_cursorOverGameObject);
                        else highlighter.deHighlightObject();
                    }

                }
                else
                {
                    highlighter.deHighlightObject();
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
            if (Physics.Raycast(ray, out _hit))
            {
                _previousCursorOverGameObject = _cursorOverGameObject;
                _cursorOverGameObject = _hit.collider.gameObject;
                return true;
            }

            _previousCursorOverGameObject = _cursorOverGameObject;
            _cursorOverGameObject = null;
            return false;
           
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

        public void menuStatusChanged(bool open)
        {
            menuOpen = open;
        }
    }
    
}
