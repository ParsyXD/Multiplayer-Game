using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController GamePlayerPrefab;
    public List<PlayerObjectController> GamePlayers { get; } = new List<PlayerObjectController>();


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    { 
        if (SceneManager.GetActiveScene().name == MapManager.Instance.Map)
        {
            PlayerObjectController GamePlayerInstance = Instantiate(GamePlayerPrefab);

            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
            GamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)steam_lobby.Instance.CurrentLobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
            conn.identity.AssignClientAuthority(conn);
        }
    }


    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }











    public string weaponsFolder = "Assets/Game/Weapons";

    [ContextMenu("Fetch Weapon Prefabs")]
    void FetchWeaponPrefabs()
    {   NetworkManager networkManager = this;

        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:GameObject", new[] { weaponsFolder });
        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (!networkManager.spawnPrefabs.Contains(prefab))
            {
                 networkManager.spawnPrefabs.Add(prefab);
            }
        }
    }


}