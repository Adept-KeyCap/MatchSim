using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using System;

public class Username : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userText;
    // Start is called before the first frame update
    void Reset()
    {
        userText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        HandleAuthChanged();
    }

    void HandleAuthChanged()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            FirebaseDatabase.DefaultInstance.GetReference($"users/{currentUser.UserId}/username").GetValueAsync().ContinueWithOnMainThread(
                task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Faulted");
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        userText.text = (string)snapshot.Value;
                        //userText.text = currentUser.UserId;
                        Debug.Log("Username: " + userText.text);
                    }
                } );
        }
    }
}
