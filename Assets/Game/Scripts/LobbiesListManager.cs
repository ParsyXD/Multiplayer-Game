using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbiesListManager : MonoBehaviour
{
    public static LobbiesListManager Instance;

    public GameObject lobbyDataItemPrefab;
    public GameObject lobbyListContent;
    public CustomNetworkManager customNetworkManager;
    public List<GameObject> listOfLobbies = new List<GameObject>();

    public void Awake()
    {
        if(Instance == null) {Instance = this;}
        
    }

    public void GetListOfLobbies()
    {
        steam_lobby.Instance.GetLobbiesList();
    }

    public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
    {
        for (int i=0; i < lobbyIDs.Count; i++)
        {
            int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)lobbyIDs[i].m_SteamID);
            string players = numPlayers.ToString();
            string maxPlayers = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "max_players");
            string lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");

            if (lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby  && lobbyName.Contains("11365"))
            {
                lobbyName = lobbyName.Replace("11365", ""); // remove "11365" from the lobby name
                GameObject createdItem = Instantiate(lobbyDataItemPrefab);
                createdItem.GetComponent<LobbyDataEntry>().lobbyID = (CSteamID)lobbyIDs[i].m_SteamID;
                createdItem.GetComponent<LobbyDataEntry>().lobbyName = lobbyName;
                createdItem.GetComponent<LobbyDataEntry>().players = players;
                createdItem.GetComponent<LobbyDataEntry>().maxPlayers = maxPlayers;
                createdItem.GetComponent<LobbyDataEntry>().SetLobbyData();
                createdItem.transform.SetParent(lobbyListContent.transform);
                createdItem.transform.localScale = Vector3.one;
                listOfLobbies.Add(createdItem);
            }   
        }
    }


void QuickSearch()
{
    if (listOfLobbies.Count > 0)
    {
        // Find the lobbies that are not full
        List<GameObject> nonFullLobbies = new List<GameObject>();

        foreach (GameObject lobbyItem in listOfLobbies)
        {
            CSteamID lobbyID = lobbyItem.GetComponent<LobbyDataEntry>().lobbyID;
            int currentPlayers = SteamMatchmaking.GetNumLobbyMembers(lobbyID);

            // Retrieve the max allowed players from the lobby data using the MaxPlayersKey
            string maxPlayersData = SteamMatchmaking.GetLobbyData(lobbyID, "max_players");

            // Convert the string to an int.
            int lobbyMaxPlayers = 0;
            if (!string.IsNullOrEmpty(maxPlayersData))
            {
                int.TryParse(maxPlayersData, out lobbyMaxPlayers);
            }

            if (currentPlayers < lobbyMaxPlayers)
            {
                nonFullLobbies.Add(lobbyItem);
            }
        }

        if (nonFullLobbies.Count > 0)
{
    GameObject lobbyToJoin = null;
    int maxPlayers = 0; // Initialize maxPlayers to 0.
    int currentPlayerValue = 0;

    foreach (GameObject lobbyItem in nonFullLobbies)
    {
        CSteamID lobbyID = lobbyItem.GetComponent<LobbyDataEntry>().lobbyID;
        int currentPlayers = SteamMatchmaking.GetNumLobbyMembers(lobbyID);

        if (currentPlayers > currentPlayerValue)
        {
            currentPlayerValue = currentPlayers;
            lobbyToJoin = lobbyItem;
            maxPlayers = SteamMatchmaking.GetLobbyMemberLimit(lobbyID); // Update maxPlayers with the current lobby's maximum player count.
        }
    }

    if (lobbyToJoin != null)
    {
        CSteamID lobbyID = lobbyToJoin.GetComponent<LobbyDataEntry>().lobbyID;
        int currentPlayers = SteamMatchmaking.GetNumLobbyMembers(lobbyID);

        Debug.Log("Joining lobby " + lobbyID + " with " + currentPlayers + " players out of " + maxPlayers + " max connections.");

        JoinLobby(lobbyID);
    }

            else
            {
                Debug.Log("No available lobbies to join.");
            }
        }
        else
        {
            Debug.Log("All lobbies are full.");
        }
    }
    else
    {
        Debug.Log("No lobbies found in the list.");
    }
}

    

        public void JoinLobby(CSteamID lobbyID)
    {
        SteamMatchmaking.JoinLobby(lobbyID);
    }

    public void DestroyLobbies()
    {
        foreach (GameObject lobbyItem in listOfLobbies)
        {
            Destroy(lobbyItem);
        }
        listOfLobbies.Clear();
    }

       
}
