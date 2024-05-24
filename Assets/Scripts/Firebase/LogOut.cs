using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class LogOut : MonoBehaviour
{
    private DatabaseReference dataBaseReference;
    public OnlineState onlineState;
    
    void Start()
    {
        dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void LogOutClick()
    {
        //string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //DatabaseReference userRef = dataBaseReference.Child("users").Child(userId).Child("online");

        dataBaseReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("searching").SetValueAsync(false);
        dataBaseReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("match").SetValueAsync("");
        dataBaseReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("accepted").SetValueAsync(false);
        dataBaseReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("online").SetValueAsync(false);
        onlineState.SetOnlineStatus(false);

        FirebaseAuth.DefaultInstance.SignOut();
        //Highscore.SetAmount(0);
        SceneManager.LoadScene(0);
    }

}
