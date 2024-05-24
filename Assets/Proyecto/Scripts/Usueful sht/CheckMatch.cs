using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CheckMatch : MonoBehaviour
{
    private string uid;
    public string matchInfo;

    private bool acceptMatch;
    private bool acceptUser;

    private string matchId;

    private void Start()
    {
        uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
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

                matchId = (string)snapshot.Value;
            }

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("accepted").ValueChanged += HandleMatch;
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").ValueChanged += HandleUser;
        });
    }

    private void HandleUser(object sender, ValueChangedEventArgs e)
    {
        Debug.Log("ae");
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        if (e.Snapshot.Exists)
        {
            Debug.Log(e.Snapshot.Child("accepted").GetRawJsonValue());
            Debug.Log(e.Snapshot.GetRawJsonValue());
            acceptUser = bool.Parse(e.Snapshot.GetRawJsonValue());
        }
        else
        {
            Debug.Log("No hay información en la ruta especificada.");
        }

        if (acceptUser == false)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("accepted").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("match").SetValueAsync("");
            gameObject.SetActive(false);
        }
    }
    private void HandleMatch(object sender, ValueChangedEventArgs e)
    {
        Debug.Log("ea");

        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        if (e.Snapshot.Exists)
        {
            Debug.Log(e.Snapshot.Child("accepted").GetRawJsonValue());
            Debug.Log(e.Snapshot.GetRawJsonValue());
            acceptMatch = bool.Parse(e.Snapshot.GetRawJsonValue());
        }
        else
        {
            Debug.Log("No hay información en la ruta especificada.");
        }

        if (acceptMatch == false)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("accepted").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("match").SetValueAsync("");

            gameObject.SetActive(false);
        }
    }
}