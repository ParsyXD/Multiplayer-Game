using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class LobbyDataEntry : MonoBehaviour
{
//Data
public CSteamID lobbyID;
public string lobbyName;
public string players;
public string maxPlayers;
public TMP_Text LobbyNameText;
public TMP_Text playersText;
public TMP_Text maxPlayersText;


public void SetLobbyData()
{
    playersText.text = players;
    maxPlayersText.text = maxPlayers;
    if (lobbyName == "")
    {
        LobbyNameText.text = "Empty";
    }
    else
    {
        LobbyNameText.text = lobbyName;
    }
}

public void JoinLobby()
{
    steam_lobby.Instance.JoinLobby(lobbyID);

}
}
