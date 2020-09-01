using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace RPG.Player
{
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private UnityEvent DestinationReached;

        private NavMeshAgent _playerAgent;
        private Coroutine _TrackMovement;
        public int Velocity { get; set; }


        public void Awake()
        {
            _player = gameObject;
            _playerAgent = _player.GetComponent<NavMeshAgent>();
        }

        public void MoveToDestination(Vector3 destination)
        {
            if (_playerAgent.SetDestination(destination) == true)
            {
                if (_TrackMovement != null) StopCoroutine(_TrackMovement);
                _TrackMovement = StartCoroutine(TrackMovement());
            }
            
        }

        private IEnumerator TrackMovement()
        {
            while(_playerAgent.remainingDistance > _playerAgent.stoppingDistance || _playerAgent.pathPending)
            { 
                yield return null;
            }

            //stopped
            //Debug.Log("Stopped");
            PlayerManager.Instance.reachedDestination();

        }


    }
}