using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public PlayerObjectController Controller;
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
    {
        // FÜRS ERSTE
        Controller.LeaveLobby();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Hides the cursor
    }
    }
}
