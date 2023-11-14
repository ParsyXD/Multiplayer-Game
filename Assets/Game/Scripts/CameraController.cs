using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CameraController : NetworkBehaviour
{

    public GameObject CameraHolder;

    public override void OnStartAuthority()         //if local player activate camera
    {
        CameraHolder.SetActive(true);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == MapManager.Instance.Map)
        {



        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        }
    }
}
