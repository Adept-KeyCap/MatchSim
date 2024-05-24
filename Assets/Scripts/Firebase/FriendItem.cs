using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using System.Collections;
using TMPro;

public class FriendItem : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public Image onlineStatusImage;
    //public TextMeshProUGUI notificationText; // Texto de notificación

    private string friendId;
    private string currentUserId;
    private DatabaseReference databaseReference;

    private void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        
    }

    public void SetFriendId(string friendId)
    {
        this.friendId = friendId;
        Debug.Log("Friend ID: " + friendId);
        LoadFriendData();
        MonitorOnlineStatus();
    }

    private void LoadFriendData()
    {
        Debug.Log("Current user ID: " + currentUserId);
        databaseReference.Child("users").Child(currentUserId).Child("friends").Child(friendId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log(databaseReference.Child("users").Child(currentUserId).Child("friends").Child(friendId).Child("username"));
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener el nombre de usuario: " + task.Exception);
                return;
            }

            string username = task.Result.Value.ToString();
            Debug.Log("Friend username: " + username);
            usernameText.text = username;
        });
    }

    private void MonitorOnlineStatus()
    {
        databaseReference.Child("users").Child(friendId).Child("online").ValueChanged += OnlineStatusChanged;
        
    }

    private void OnlineStatusChanged(object sender, ValueChangedEventArgs args)
    {
        if (gameObject != null)
        {
            bool isOnline = (bool)args.Snapshot.Value;
            onlineStatusImage.color = isOnline ? Color.green : Color.red;
            GetComponentInParent<FriendListManager>().SortFriendsByOnlineStatus();
        }
        else
        {
            Debug.LogWarning("Null FriendItem Reference");
        }

    }

    public bool IsOnline
    {
        get
        {
            return onlineStatusImage.color == Color.green;
        }

    }

   
}

