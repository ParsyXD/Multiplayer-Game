using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{  
    [Header("Stats")]
    public float speed = 1;
    bool MovementAllowed = false;

    [Header("Objects")]
    public GameObject PlayerModel;
    public Transform PlayerCamera;

    void Start()
    {
        PlayerModel.SetActive(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == MapManager.Instance.Map)
        {
            if (PlayerModel.activeSelf == false)
            {
                PlayerModel.SetActive(true);
                SpawnPosition();
            }
            MovementAllowed = hasAuthority;
            if (MovementAllowed)
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 cameraForward = PlayerCamera.forward;
        cameraForward.y = 0.0f;
        cameraForward.Normalize();

        Vector3 cameraRight = PlayerCamera.right;
        cameraRight.y = 0.0f;
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraRight * xDirection + cameraForward * zDirection).normalized;
        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;

        transform.position = newPosition;
    }

        }
    }

    

    public void SpawnPosition()
    {
        transform.position = new Vector3(12, 3, Random.Range(-5, 7));
    }
}
