using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkingButtons : MonoBehaviour
{
    CustomNetworkManager networkManager;
    GameObject manager;
    MapManager mapManager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Network Manager");
    networkManager = manager.GetComponent<CustomNetworkManager>();
    mapManager = manager.GetComponent<MapManager>();
    }


    public void Host()
    {
         manager.GetComponent<steam_lobby>().HostLobby();
        networkManager.onlineScene = mapManager.Map;
        mapManager.Load();
         

}
}

