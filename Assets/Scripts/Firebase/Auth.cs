using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Firebase;

public class Auth : MonoBehaviour
{
    private DatabaseReference dataBaseReference;
    private FirebaseAuth auth;
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += AuthStateChange;
        dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }
    public void ChangeAuthState()
    {
        Debug.Log("Auth State  Changed void");
        
    }

    public void AuthStateChange(object sender, EventArgs e)
    {
        Debug.Log("Auth State Changed");
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log("Authenticated");
            SceneManager.LoadScene(1);
            Debug.Log("Changing Scene...");
            
        }
        else
        {
            Debug.Log("Not Authenticated");
        }
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
    void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= AuthStateChange;
    }
}
