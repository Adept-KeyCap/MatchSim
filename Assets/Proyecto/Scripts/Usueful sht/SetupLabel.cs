using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetupLabel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text labelText;

    private string matchInfo;

    private void Start()
    {

        FirebaseDatabase.DefaultInstance.GetReference($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/match").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot);

                matchInfo = (string)snapshot.Value;
            }

            FirebaseDatabase.DefaultInstance.GetReference($"users/{matchInfo}/username").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                Debug.Log(matchInfo);
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot);

                    labelText.text = (string)snapshot.Value;
                }
            });
        });
    }
}