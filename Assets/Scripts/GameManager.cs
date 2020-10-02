using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class GameManager : MonoBehaviour
    {

        //Singleton Game Manager...
        public static GameManager Instance { get; private set; }
        public GameObject player { get; set; }

        [SerializeField] private UnityEvent PlayerCreated;
        [SerializeField] private UnityEvent<bool> MenuStatusChanged = new UnityEvent<bool>();
        [SerializeField] private GameObject _playerPreFab;

        [SerializeField] private Canvas HUD;

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
                //CreatePlayer(transform, Vector3.zero, Quaternion.identity);
            }
        }

        public void StartGame()
        {

            LoadScene("Start", 1);
            HUD.enabled = true;


        }

        public void LoadScene(string scene, int door)
        {
            StartCoroutine(LoadAsyncScene(scene,door));
            //DataManager.Instance.readData("Assests/Scenes/" + scene + "/data.json", "objectExists");
        }

        public void setMenuOpen(bool open)
        {
            MenuStatusChanged.Invoke(open);
        }


        private void CreatePlayer(Transform newTransform, Vector3 offset, Quaternion rotationOffset)
        {
            Debug.Log("Creating Player");
            if (player != null) Destroy(player);
            player = Instantiate(_playerPreFab, newTransform.position + newTransform.TransformVector(offset), newTransform.rotation*rotationOffset);
            PlayerCreated.Invoke();

        }

        private IEnumerator LoadAsyncScene(string scene, int door)
        {

            DataManager.Instance.initializeLevelData(scene);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("Done Loading Scene...");
            CreatePlayer(GameObject.Find("Door" + door.ToString()).transform, new Vector3(5f,0,0), Quaternion.Euler(0, 90, 0));
            

        }

    }
}

