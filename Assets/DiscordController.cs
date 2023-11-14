using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
    public static DiscordController Instance;

    public Discord.Discord discord;
    public Sprite Image;

    private string lastInvitedLobbyID; // Store the ID of the last invited lobby
    private string lastInvitedUserID; // Store the ID of the last invited user

    void Start()
    {
        if (Instance == null) { Instance = this; }
        discord = new Discord.Discord(1134195570748162119, (System.UInt64)Discord.CreateFlags.Default);
    }

    public void InvitePlayer(ulong userID, string lobbyID, string maxPlayers, string currentPlayers)
    {
        // Check if there is an existing invitation and clear it before sending a new one
        if (!string.IsNullOrEmpty(lastInvitedLobbyID) && !string.IsNullOrEmpty(lastInvitedUserID))
        {
            ClearExistingInvites();
        }

        lastInvitedLobbyID = lobbyID;
        lastInvitedUserID = userID.ToString();

        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            Name = "Multiplayer Test",
            State = "-where the player is in-",
            Details = "-what the player is doing-",
            Assets =
            {
                LargeImage = "test_image",
                LargeText = "test",
                SmallImage = "test_image",
                SmallText = "test",
            },
            Party =
            {
                Id = lobbyID, // Set the lobby ID as the Party Id
                Size =
                {
                    CurrentSize = int.Parse(currentPlayers), // Set the current players count
                    MaxSize = int.Parse(maxPlayers) // Set the max players count
                }
            },
            Secrets =
            {
                Join = lobbyID // Set the lobby ID as the secret for joining
            }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.LogError("Discord Working!");
                StartCoroutine(InviteWithDelay(userID, lobbyID)); // Send the invitation with a slight delay
            }
            else
            {
                Debug.LogError("Failed to update activity: " + res);
            }
        });
    }

    private IEnumerator InviteWithDelay(ulong userID, string lobbyID)
    {
        yield return new WaitForSeconds(1.0f); // Adjust the delay time as needed (1.0f is an example)
        SendInvitation(userID, lobbyID); // Send the invitation after the delay
    }

    private void SendInvitation(ulong userID, string lobbyID)
    {
        discord.GetActivityManager().SendInvite((long)userID, Discord.ActivityActionType.Join, lobbyID, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Invitation sent successfully!");
            }
            else
            {
                Debug.LogError("Failed to send invitation: " + res);
            }
        });
    }

    private void ClearExistingInvites()
    {
        var emptyActivity = new Discord.Activity();
        discord.GetActivityManager().UpdateActivity(emptyActivity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Cleared existing invites.");
            }
            else
            {
                Debug.LogError("Failed to clear existing invites: " + res);
            }
        });
    }

    void Update()
    {
        discord.RunCallbacks();
    }
}