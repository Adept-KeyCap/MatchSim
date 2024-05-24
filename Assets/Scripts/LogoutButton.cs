using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Firebase.Auth;
using Firebase.Database;

public class LogoutButton : MonoBehaviour
{
    public GameObject LogInPanel;
    public GameObject GamePanel;

    private DatabaseReference databaseReference;
    //public NewManager _nManager;
    public void OnPointerClick() 
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        databaseReference.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("online").SetValueAsync(false);


        FirebaseAuth.DefaultInstance.SignOut();
        GamePanel.SetActive(false);
        LogInPanel.SetActive(true);
    }
}
