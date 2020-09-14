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
    private bool cameraFreeToZoom = true;

    private const float cameraShiftForDialogue = 2f;
    private const float cameraShiftTime = 1f;

    private void Awake()
    {
        zoom = 7f;
        DontDestroyOnLoad(this);
    }


    private void Update()
    {
        if(_camera != null && cameraFreeToZoom)
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

    public void moveCameraForDialogue(bool activating)
    {
        Debug.Log("Moving Camera for dialogue");
        if (activating)
        {
            cameraFreeToZoom = false;
            StartCoroutine(moveCamera(_camera.transform.position + new Vector3(cameraShiftForDialogue, 0, 0), cameraShiftTime, false));
        }
        else
        {
            StartCoroutine(moveCamera(new Vector3(_player.transform.position.x, _camera.transform.position.y, _camera.transform.position.z), cameraShiftTime, true));
        }
    }

    private IEnumerator moveCamera(Vector3 destination, float seconds, bool freeCamera)
    {
        float elapsedTime = 0;
        Vector3 startingPos = _camera.transform.position;
        while (elapsedTime < seconds)
        {
            _camera.transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _camera.transform.position = destination;
        cameraFreeToZoom = freeCamera;
;
    }
}
