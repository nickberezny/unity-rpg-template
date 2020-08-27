using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Player
{
    public class PlayerManager : MonoBehaviour
    {
        

        private PlayerMotor _playerMotor;
        private GameObject _player;
        

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


        public void GetNewPlayer()
        {
            _player = GameManager.Instance.player;
            if(_player == null)
            {
                Debug.Log("No Player Set!");
                return;
            }
            _playerMotor = _player.GetComponent<PlayerMotor>();

        }

        public void movePlayer(Vector3 destination)
        {
            if (_playerMotor != null) _playerMotor.MoveToDestination(destination);
            else Debug.Log("No player to move");
        }
    }
}