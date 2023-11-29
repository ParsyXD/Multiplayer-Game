using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InventorySystem : NetworkBehaviour
{
    public GameObject gunPrefab;
    public Transform weaponHolder;
    

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CmdPickupGun();
        }  
    }



    [Command]
    public void CmdPickupGun()
    {
        // Spawn the gun object on the server
        GameObject newGun = Instantiate(gunPrefab, weaponHolder);
        
        // Spawn the gun on all clients
        NetworkServer.Spawn(newGun);

        // Assign authority to the spawned gun object for the client that picked it up
        newGun.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }
        
    }
            
         
#warning instead logic: pick up gun (with e) raycast pickup object and access (or access directly in inventory later) the script with the prefab,
#warning then spawn prefab in weaponholder, when killed drop weapon and remove authority with something like identity.RemoveClientAuthority();
