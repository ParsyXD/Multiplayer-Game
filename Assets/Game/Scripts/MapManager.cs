using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour
{
public static MapManager Instance;

    public string Map;
    public CustomNetworkManager manager;
    MapManager mapManager;
    public Image LoadingScreen;
    public TMP_Text MapText;
    public GameObject Ui;
    public GameObject LoadingUi;
    public Sprite[] LoadingScreens;


void Awake()
{
    Instance = this;
    mapManager = manager.GetComponent<MapManager>();
}

public void Load()
{
    LoadingUi.SetActive(true);
    Ui.SetActive(false);
    string loadingScreenImageName = "LoadingScreen_" + mapManager.Map;
    MapText.text = mapManager.Map;
    foreach (Sprite Image in LoadingScreens)
    {
        if (Image.name == mapManager.Map)
        {
            LoadingScreen.sprite = Image;
            break;
        }
    }
    }
}


