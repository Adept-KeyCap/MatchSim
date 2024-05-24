using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class OnlineState : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentUserId;

    public void RetrieveOnlineStatus()
    {
        currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        SetOnlineStatus(true);
        Debug.LogWarning("current user ID:" + currentUserId);
    }

    private void OnApplicationQuit()
    {
        SetOnlineStatus(false);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SetOnlineStatus(!pauseStatus);
    }

    private void OnDestroy()
    {
        SetOnlineStatus(false);
    }

    public void SetOnlineStatus(bool isOnline)
    {
        if (!string.IsNullOrEmpty(currentUserId))
        {
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(currentUserId).Child("online");
            if (isOnline)
            {
                Debug.Log("current User is now online.");
                userRef.SetValueAsync(true);
                Debug.Log(userRef.ToString() + " is set to true");

            }
            else
            {
                Debug.Log("Current User has gone offline.");
                userRef.SetValueAsync(false);
            }
        }
        else
        {
            Debug.LogError("currentUserId is null or empty.");
        } 
    }
    
}
