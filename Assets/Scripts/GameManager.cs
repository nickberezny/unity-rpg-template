using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace RPG
{
    public class GameManager : MonoBehaviour
    {

        //Singleton Game Manager...
        public static GameManager Instance { get; private set; }

        public int Score { get; set; }
        public GameObject player { get; set; }


        [SerializeField] private UnityEvent PlayerCreated;
        [SerializeField] private GameObject _playerPreFab;

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

                //temporarily create player at start
                CreatePlayer(transform);
            }
        }

        private void Start()
        {
            Score = 10;

        }

        private void CreatePlayer(Transform newTransform)
        {
            Debug.Log("Creating Player");
            if (player != null) Destroy(player);
            player = Instantiate(_playerPreFab, newTransform);
            PlayerCreated.Invoke();
        }

    }
}

