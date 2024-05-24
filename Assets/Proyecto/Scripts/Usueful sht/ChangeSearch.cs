using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ChangeSearch : MonoBehaviour
{
    public bool isSearch;

    private void Start()
    {
        isSearch = false;

    }

    public void ChangeState()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot);

                isSearch = bool.Parse(snapshot.GetRawJsonValue());
            }

            if (isSearch)
            {
                FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
                isSearch = false;
            }
            else if (!isSearch)
            {
                FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(true);
                isSearch = true;
            }

        });

    }
}