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
        
        private const float waitTimeForRotation = 0.01f;
        private const float rotationSpeed = 2.5f;
        private const float rotationMinThreshold = 0.001f;
        private const float stoppingDistanceMargin = 0.25f;

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

        public void FaceObject(GameObject gameObject)
        {
            StartCoroutine(RotateOverTime(gameObject.transform, rotationSpeed));

        }

        private IEnumerator TrackMovement()
        {
            while(_playerAgent.remainingDistance > _playerAgent.stoppingDistance + stoppingDistanceMargin || _playerAgent.pathPending)
            { 
                yield return null;
            }

            PlayerManager.Instance.reachedDestination();

        }

        private IEnumerator RotateOverTime(Transform target, float speed)
        {
            Vector3 dir = target.position - _player.transform.position;
            dir.y = 0;

            while (Mathf.Abs(Quaternion.Dot(_player.transform.rotation, Quaternion.LookRotation(dir))) < 1f - rotationMinThreshold)
            {
                dir = target.position - _player.transform.position;
                dir.y = 0;
                _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, Quaternion.LookRotation(dir), speed * waitTimeForRotation);

                yield return new WaitForSeconds(waitTimeForRotation);
            }

        }


    }
}