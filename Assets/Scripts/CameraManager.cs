using RPG;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float zoom { get; set; }

    [SerializeField] private Camera CameraPreFab;

    private GameObject _player;
    private Camera _camera;

    private void Awake()
    {
        zoom = 7f;
        DontDestroyOnLoad(this);
    }


    private void Update()
    {
        if(_camera != null)
        {
            zoom -= Input.GetAxis("Mouse ScrollWheel");
            _camera.transform.position = _player.transform.position + new Vector3(0, zoom, -zoom);
            _camera.transform.LookAt(_player.transform);
        }
    }

    public void CreateCamera()
    {
        if (FindObjectOfType<Camera>())
        {
            Debug.Log("Already a camera in the scene");
            return;
        }

        Debug.Log("Creating Camera");
        _player = GameManager.Instance.player;
        _camera = Instantiate(CameraPreFab, _player.transform.position + new Vector3(0, zoom, -zoom), Quaternion.identity);
        _camera.transform.LookAt(_player.transform);

        return;
    }
}
