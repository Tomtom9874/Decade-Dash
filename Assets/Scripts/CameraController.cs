using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraOffset = 1.0f;
    private GameObject _player;

    private PlayerController playerControllerScript;

    //when player gets to like 1 unit from right side, need to keep moving camera right
    //so that it's position keeps player 1 unit from right side


    private void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        _player = GameObject.Find("Player");
    }
    
    private void Update()
    {
        //need to update the camera location
        if (_player.transform.position.x >= transform.position.x + cameraOffset)
            transform.position = new Vector3(_player.transform.position.x - cameraOffset, transform.position.y,
                transform.position.z);

        if (_player.transform.position.y >= transform.position.y + cameraOffset)
            transform.position = new Vector3(transform.position.x, _player.transform.position.y - cameraOffset,
                transform.position.z);

        if (_player.transform.position.y <= transform.position.y - cameraOffset)
            transform.position = new Vector3(transform.position.x, _player.transform.position.y + cameraOffset,
                transform.position.z);
    }
}