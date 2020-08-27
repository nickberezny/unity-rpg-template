using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Player
{
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private GameObject _player;


        public void Awake()
        {
            _player = this.gameObject;
        }

        public void MoveToDestination(Vector3 destination)
        {
            _player.transform.position = destination;
        }

        private void ArriveAtDestination()
        {

        }

    }
}