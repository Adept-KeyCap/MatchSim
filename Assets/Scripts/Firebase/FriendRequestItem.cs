using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class FriendRequestItem : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public Button acceptButton;
    public Button rejectButton;

    private string senderId;
    private string senderUsername;
    private string receiverId;
    private string requestKey;
    private DatabaseReference databaseReference;
    private FriendRequestManager friendRequestManager;

    private void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        acceptButton.onClick.AddListener(AcceptRequest);
        rejectButton.onClick.AddListener(RejectRequest);
    }

    public void SetRequestData(string senderId,string senderUsername, string receiverId,string requestKey, FriendRequestManager friendRequestManager)
    {
        this.senderId = senderId;
        this.senderUsername = senderUsername;
        this.receiverId = receiverId;
        this.requestKey = requestKey;
        this.friendRequestManager = friendRequestManager;
        LoadUserData();
    }

    private void LoadUserData()
    {
        databaseReference.Child("users").Child(senderId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al obtener el nombre de usuario: " + task.Exception);
                return;
            }

            string username = senderUsername;
            usernameText.text = username;
        });
    }

    private void AcceptRequest()
    {
        friendRequestManager.AcceptFriendRequest(senderId, requestKey);
        Destroy(gameObject);
    }

    private void RejectRequest()
    {
        friendRequestManager.RejectFriendRequest(senderId, requestKey);
        Destroy(gameObject);
    }
}
