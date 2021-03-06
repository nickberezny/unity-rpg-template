﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Player
{
    public class PlayerManager : MonoBehaviour
    {

        public bool isMenuOpen { get; private set; }

        private PlayerMotor _playerMotor;
        private PlayerAnimator _playerAnimator;
        private GameObject _player;
        private GameObject _selectedObject;


        private delegate void MethodTemp();

        private const float InteractWaitTime = 0.5f;
        public static PlayerManager Instance { get; private set; }
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

                isMenuOpen = false;
            }
        }

        private void Update()
        {
            if (_playerMotor != null)
            {
                float velocity = _playerMotor.Velocity;
                if (velocity > 0f)
                {
                    animatePlayer("Walk", velocity);
                }
            }

        }


        public void GetNewPlayer()
        {
            _player = GameManager.Instance.player;
            if (_player == null)
            {
                Debug.Log("No Player Set!");
                return;
            }

            _playerMotor = _player.GetComponent<PlayerMotor>();
            _playerAnimator = _player.GetComponent<PlayerAnimator>();

        }

        public void movePlayer(RaycastHit hit)
        {

            _selectedObject = hit.collider.gameObject;
            if (_playerMotor != null && !isMenuOpen)
            {
                if (_selectedObject.TryGetComponent(out Interactable interactable)) _playerMotor.MoveToDestination(interactable._interactionTransform.position);
                else _playerMotor.MoveToDestination(hit.point);
            }

            else Debug.Log("No player to move or menu is open");
        }

        public void faceObject(GameObject gameObject)
        {
            if (gameObject != null) _playerMotor.FaceObject(gameObject);
        }

        public void reachedDestination()
        {
            if (_selectedObject)
            {
                if (_selectedObject.tag == "Interactable")
                {
                    faceObject(_selectedObject);
                    StartCoroutine(ExecuteAfterDelay(_selectedObject.GetComponent<Interactable>().Interact, InteractWaitTime));
                }
            }
            

            _selectedObject = null;
        }

        public void menuStatusChanged(bool open)
        {
            isMenuOpen = open;

            if(open && _playerMotor)
            {
                _selectedObject = null;
                _playerMotor.stopMovement();
            }

            Debug.Log("menu: " + open);
        }

        private void animatePlayer(string type, float velocity)
        {
            switch(type)
            {
                case "Walk":
                    if (_playerAnimator != null) _playerAnimator.walkingAnimation(velocity);
                    else Debug.Log("No player to animate");
                    break;
                default:
                    Debug.Log("Requesting animation which doesnt exist");
                    break;
            }
            
        }

        private IEnumerator ExecuteAfterDelay(MethodTemp method, float time)
        {
            yield return new WaitForSeconds(time);
            method();
        }
    }
}