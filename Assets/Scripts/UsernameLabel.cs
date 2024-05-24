using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using System;
using Firebase.Database;
using Firebase.Extensions;

public class UsernameLabel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    private void Reset()
    {
        _label = GetComponent<TMP_Text>();
    }
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleStateChange;
    }

    private void HandleStateChange(object sender, EventArgs e) 
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        if (currentUser != null) 
        {
            SetLabelUsername(currentUser.UserId);
        }
    }

    private void SetLabelUsername(string userId) 
    {
        FirebaseDatabase.DefaultInstance
        .GetReference($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/username")
        .GetValueAsync().ContinueWithOnMainThread(task => {
              if (task.IsFaulted)
              {
                  Debug.Log(task.Exception);
              }
              else if (task.IsCompleted)
              {
                DataSnapshot snapshot = task.Result;
                _label.text = (string)snapshot.Value;
              }
          });
    }

}
