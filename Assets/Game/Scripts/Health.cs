using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;

public class Health : NetworkBehaviour
{
    [Header("Values")]
    [SyncVar(hook = nameof(OnHealthChanged))]
    public int health = 100;
    public bool Player = true;

    [Header("References")]
    public GameObject DeathUI;

    public void TakeDamage(int damage)
    {
        if (Player && !isServer) return;

        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        // Update UI or any other logic when health changes
    }

    void Death()
    {
        if (DeathUI != null)
        {
            DeathUI.SetActive(true);
        }
    }




    [Command]
    public void CmdTakeDamage(int damage)
    {
        TakeDamage(damage);
    }
}