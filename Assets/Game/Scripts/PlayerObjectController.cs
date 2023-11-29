using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;


public class PlayerObjectController : NetworkBehaviour
{
    //Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;
    public GameObject camera;

    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (isLocalPlayer)
        {
            camera.gameObject.SetActive(true);
        }
        else
        {
            camera.gameObject.SetActive(false);
        }
    }


    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }





    public override void OnStartAuthority()
    {
       
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer();
        LobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
    }




    //Start Game

    public void CanStartGame(string SceneName)
    {
        if (hasAuthority)
        {
            CmdCanStartGame(SceneName);
        }
    }

    // Method to leave the current Steam lobby.
    public void LeaveLobby()
    {

        //Leave Steam Lobby
        steam_lobby.Instance.LeaveLobby();
        Debug.LogWarning("Lobby Closed");

        if (hasAuthority)
        {
            if (isServer)
            {
                manager.StopHost();
            }
            else
            {
                manager.StopClient();
            }
        }

        //Set the offline scene to null
        manager.offlineScene = "";

        //Make the active scene the offline scene
        SceneManager.LoadScene("Server Browser",LoadSceneMode.Single);
    }
#warning  on application quit auto leave

#warning  transfer host





}
