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
            Debug.LogError("Error al obtener la lista de amigos: " + task.Exception);
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
      Debug.Log("MonitorFriendOnlineStatus: " + friendOnlineRef.ToString() + " is set to true");
      friendOnlineRef.ValueChanged += OnFriendOnlineStatusChanged;
   }
   private void OnFriendOnlineStatusChanged(object sender, ValueChangedEventArgs args)
   {
      string friendId = args.Snapshot.Reference.Parent.Key;
      bool isOnline = (bool)args.Snapshot.Value;
      Debug.Log("OnFriendOnlineStatusChangedFriend ID: " + friendId + " is online: " + isOnline);
      // Obtener el nombre de usuario del amigo
      databaseReference.Child("users").Child(friendId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
      {
         if (task.IsFaulted)
         {
            Debug.LogError("Error al obtener el nombre de usuario: " + task.Exception);
            return;
         }

         string friendUsername = task.Result.Value.ToString();
         Debug.Log("Friend username CHANGESTATE: " + friendUsername);

         // identify if friend is connected 
         if (notificationManager != null)
         {
            if (isOnline)
            {
               Debug.Log("Friend is online: " + friendUsername);
               notificationManager.SetNotificationConnected(friendUsername);
            }
            else
            {
               Debug.Log("Friend is offline: " + friendUsername);
            }
         }
      });
   }
}
