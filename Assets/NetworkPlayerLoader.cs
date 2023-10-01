using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkPlayerLoader : NetworkBehaviour
{
    public GameObject Camera;
    private MyNetworkManager game;
    private MyNetworkManager Game
    {
        get
        {
            if (game != null)
            {
                return game;
            }
            return game = MyNetworkManager.singleton as MyNetworkManager;
        }
    }

    // Update is called once per frame
    void Update()
    {

            if(hasAuthority && SceneManager.GetActiveScene().name != "Scene_SteamworksLobby")
            {
                Camera.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Destroy(this, 2);
            }
            else 
            {
                Camera.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        
    }
}
