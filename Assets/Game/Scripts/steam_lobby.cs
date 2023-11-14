using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using TMPro;

public class steam_lobby : MonoBehaviour
{
    public static steam_lobby Instance; // Singleton instance of the steam_lobby script.

    // Callbacks for Steam lobby events.
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    //Lobby Callbacks
    protected Callback<LobbyMatchList_t> LobbyList;
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;
    
    public List <CSteamID> lobbyIDs = new List<CSteamID>();

    // Variables
    public ulong CurrentLobbyID; // ID of the current Steam lobby.
    private const string HostAddressKey = "HostAddress"; // Key used to store the host address in Steam lobby data.
    public CustomNetworkManager manager; // Reference to a custom network manager script.

    void Start()
    {   
        if (!SteamManager.Initialized) { Debug.LogError("SteamManagerNotInitialized");return;} // If Steam is not initialized, return.
        if (Instance == null) {Instance = this;} // Set the singleton instance of the steam_lobby script.
        manager.GetComponent<CustomNetworkManager>(); // Get a reference to the custom network manager script.
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated); // Create a Steam lobby creation callback.
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest); // Create a Steam lobby join request callback.
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered); // Create a Steam lobby enter callback.

        LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
        GetLobbiesList();
    }

    public void HostLobby()
    {
    

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, manager.maxConnections);

        // Get the created lobby ID
        CSteamID lobbyId = new CSteamID(SteamMatchmaking.GetLobbyByIndex(0).m_SteamID);

        // Set the host's Steam ID as lobby data with the HostAddressKey.
        SteamMatchmaking.SetLobbyData(lobbyId, HostAddressKey, SteamUser.GetSteamID().ToString());

        // Set the lobby name as lobby data with the name key.
        string lobbyName = SteamFriends.GetPersonaName().ToString() + "'s Lobby" + "11365";
        SteamMatchmaking.SetLobbyData(lobbyId, "name", lobbyName);

        // Get the maximum player count from the custom network manager and set it as lobby data.
        int maxPlayers = manager.maxConnections;
        SteamMatchmaking.SetLobbyData(lobbyId, "max_players", maxPlayers.ToString());

        // Get the current player count (which is 1 for the host) and set it as lobby data.
        int currentPlayers = 1;
        SteamMatchmaking.SetLobbyData(lobbyId, "current_players", currentPlayers.ToString());

        SteamMatchmaking.SetLobbyData(lobbyId, "current_map", MapManager.Instance.Map);


        // Delay the invitation process by 1 second (adjust the time as needed)
        StartCoroutine(DelayedInvite(lobbyId, maxPlayers, currentPlayers));
    }

    private IEnumerator DelayedInvite(CSteamID lobbyId, int maxPlayers, int currentPlayers)
    {
        yield return new WaitForSeconds(1f); // Adjust the delay time as needed

        // Call the InvitePlayer method from DiscordController and pass the lobby data
        if(DiscordController.Instance != null)
        {
            DiscordController.Instance.InvitePlayer(SteamUser.GetSteamID().m_SteamID, lobbyId.ToString(), maxPlayers.ToString(), currentPlayers.ToString());
        }
        
    }

    private void OnLobbyCreated(LobbyCreated_t callback) // Callback for when a Steam lobby is created.
    {
        if (callback.m_eResult != EResult.k_EResultOK) {return;} // If lobby creation was not successful, return.
        Debug.Log("Lobby Created Success");
        manager.StartHost(); // Start hosting the network manager as a host.

        CSteamID lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        Debug.Log(lobbyId + "lobbyID Created");

        // Set the host's Steam ID as lobby data with the HostAddressKey.
        SteamMatchmaking.SetLobbyData(lobbyId, HostAddressKey, SteamUser.GetSteamID().ToString());

        // Set the lobby name as lobby data with the name key.
        SteamMatchmaking.SetLobbyData(lobbyId, "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby" + "11365");

        // Get the maximum player count from the custom network manager and set it as lobby data.
         int maxPlayers = manager.maxConnections;
         SteamMatchmaking.SetLobbyData(lobbyId, "max_players", maxPlayers.ToString());
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback) // Callback for when a join request is received.
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby); // Join the Steam lobby specified in the join request.
    }

    private void OnLobbyEntered (LobbyEnter_t callback) // Callback for when a player enters the lobby.
    {
        CurrentLobbyID = callback.m_ulSteamIDLobby; // Set the current lobby ID to the entered lobby's ID.
        // LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name"); // Set the lobby name text to the lobby data with the name key.

        // Client
        if (NetworkServer.active) {return;} // If already a server, return.
    manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
    manager.StartClient();
}

    public void JoinLobby(CSteamID lobbyID)
    {
        MapManager.Instance.Map = SteamMatchmaking.GetLobbyData(lobbyID, "current_map");
        SteamMatchmaking.JoinLobby(lobbyID);
        Debug.Log("Joined Lobby");
        manager.GetComponent<MapManager>().Load();
    }


public void TransferHostOnLeave()
{
//TRANSFER HOST TO NEW PLAYER

}

public void LeaveLobby()
{
    SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyID));
    manager.StopClient(); // Stop the network client.
    manager.StopHost(); // Stop hosting the network manager as a host.
    CurrentLobbyID = 0; // Reset the current lobby ID.
    // Perform any additional cleanup or actions needed when leaving the lobby.
}



// ---- Server Browser ----

    public void GetLobbiesList() // ONLY LOADING 60 LOBBIES
    {
        if(lobbyIDs.Count > 0)
        {
            lobbyIDs.Clear();
        }

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60); 
        SteamMatchmaking.RequestLobbyList();
    }
    
    void OnGetLobbyList(LobbyMatchList_t result)
    {
        if (LobbiesListManager.Instance.listOfLobbies.Count > 0)
        {
            LobbiesListManager.Instance.DestroyLobbies();
        }
        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
    } 

    void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbiesListManager.Instance.DisplayLobbies(lobbyIDs, result);
    }

    
}