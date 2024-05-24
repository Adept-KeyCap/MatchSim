using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class LogOut : MonoBehaviour
{
    private DatabaseReference dataBaseReference;
    
    void Start()
    {
        dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void LogOutClick()
    {
        SetOnlineStatus(false);
        FirebaseAuth.DefaultInstance.SignOut();
        
        SceneManager.LoadScene(0);
    }
    
    private void SetOnlineStatus(bool isOnline)
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        DatabaseReference userRef = dataBaseReference.Child("users").Child(userId).Child("online");
        if (isOnline)
        {
            userRef.OnDisconnect().SetValue(false);
            userRef.SetValueAsync(true);
        }
        else
        {
            userRef.SetValueAsync(false);
        }
    }
}
