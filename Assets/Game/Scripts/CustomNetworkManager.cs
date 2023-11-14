using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController GamePlayerPrefab; // The prefab for the player object that will be spawned on the server
    public List<PlayerObjectController> GamePlayers { get;} = new List<PlayerObjectController>(); // A list to keep track of all the spawned player objects

// This method is called when a player is added to the server
public override void OnServerAddPlayer(NetworkConnectionToClient conn)
{ 
    if(SceneManager.GetActiveScene().name == MapManager.Instance.Map)
    {
        PlayerObjectController GamePlayerInstance = Instantiate(GamePlayerPrefab);
            
        // Set the connection ID of the player object to the connection ID of the client
        GamePlayerInstance.ConnectionID = conn.connectionId;
            
        // Set the player ID number of the player object to the current count of GamePlayers list + 1
        GamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
            
        // Set the player Steam ID of the player object to the Steam ID of the lobby member at the corresponding index in the lobby
        GamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)steam_lobby.Instance.CurrentLobbyID, GamePlayers.Count);
            
        // Add the player object to the server and associate it with the client's connection
        NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);

        // Assign authority of the player object to the client
        conn.identity.AssignClientAuthority(conn);
    }
}

    // This method is used to start the game by changing the scene on the server
    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }
}