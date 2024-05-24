using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class FriendOnlineStatusManager : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private string currentUserId;

    // Class to manage the notifications when a friend has connected to the game
    public NotificationStatusIdentifier notificationManager;
    
    public void RetrieveFriendsOnlineStatus()
    {
       databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
       currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    
       LoadFriendList();
    }

    private void LoadFriendList()
    {
        databaseReference.Child("users").Child(currentUserId).Child("friends").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
               Debug.LogError("Friend List couldn't be retrieved: " + task.Exception);
               return;
            }
            
            DataSnapshot snapshot = task.Result;
            
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
               string friendId = childSnapshot.Key;
               MonitorFriendOnlineStatus(friendId);
            }
        });
    }

    private void MonitorFriendOnlineStatus(string friendId)
    {
       DatabaseReference friendOnlineRef = databaseReference.Child("users").Child(friendId).Child("online");
       friendOnlineRef.ValueChanged += OnFriendOnlineStatusChanged;


       //Debug.Log("Friend Status monitoring: " + friendOnlineRef.ToString() + " is true");
    }
    private void OnFriendOnlineStatusChanged(object sender, ValueChangedEventArgs args)
    {
       string friendId = args.Snapshot.Reference.Parent.Key;
       bool isOnline = (bool)args.Snapshot.Value;

       // Get friends username
       databaseReference.Child("users").Child(friendId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
       {
          if (task.IsFaulted)
          {
             Debug.LogError("Error al obtener el nombre de usuario: " + task.Exception);
             return;
          }
    
          string friendUsername = task.Result.Value.ToString();
    
          // is friend conected? 
          if (notificationManager != null)
          {
             if (isOnline)
             {
                notificationManager.SetNotificationConnected(friendUsername);
             }
          }
       });
    }
}
