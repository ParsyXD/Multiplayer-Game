using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LobbyController : MonoBehaviour
{
public static LobbyController Instance;

//UI Elements
public TMP_Text LobbyNameText;

//Player Data
public GameObject LocalPlayerObject;

//Other Data
public ulong CurrentLobbyID;
public PlayerObjectController LocalplayerController;

//Manager
private CustomNetworkManager manager;

//Scene
 string SceneName;

private void Awake ()
{
    if (Instance == null) {Instance = this;}

}





public void UpdateLobbyName()
{
    CurrentLobbyID = Manager.GetComponent<steam_lobby>().CurrentLobbyID;
    //LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
}



public void FindLocalPlayer()
{
    LocalPlayerObject = GameObject.Find("LocalGamePlayer");
    LocalplayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
}



public void StartGame()
{
    SceneName = MapManager.Instance.Map;
    LocalplayerController.CanStartGame(SceneName);
}

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


    

}
