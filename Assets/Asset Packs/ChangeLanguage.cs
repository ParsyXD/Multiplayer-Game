using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{

void Start()
{
    if (!PlayerPrefs.HasKey("Language"))
    {
        PlayerPrefs.SetString("Language", "English");
    }
}

public void English()
{
    PlayerPrefs.SetString("Language", "English");
}

public void German()
{
    PlayerPrefs.SetString("Language", "German");
}
}
