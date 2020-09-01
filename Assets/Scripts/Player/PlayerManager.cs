using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Player
{
    public class PlayerManager : MonoBehaviour
    {
        
        private PlayerMotor _playerMotor;
        private PlayerAnimator _playerAnimator;
        private GameObject _player;
        private GameObject _selectedObject;
        

        //Singleton Player Manager...
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
            }
        }

        private void Update()
        {
            float velocity = _playerMotor.Velocity;
            if (velocity > 0f)
            {
                animatePlayer("Walk", velocity);
            }
        }


        public void GetNewPlayer()
        {
            _player = GameManager.Instance.player;
            if(_player == null)
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
            if (_playerMotor != null) _playerMotor.MoveToDestination(hit.point);
            else Debug.Log("No player to move");
        }

        public void reachedDestination()
        {
            if (_selectedObject != null) _selectedObject.GetComponent<Interactable>().Interact();
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
    }
}