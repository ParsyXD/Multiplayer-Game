using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkScripts : NetworkBehaviour 
{
    public List<MonoBehaviour> scripts = new List<MonoBehaviour>();


    void Start()
    {
        if(isLocalPlayer)Toggle(true);
        else Toggle(false);
    }

    void Toggle(bool isLocalPlayer)
    {
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = isLocalPlayer;
        }
        
    }
}
