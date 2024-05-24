using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Found : MonoBehaviour
{
    [SerializeField]
    private MatchMaking match;
    [SerializeField]
    private GameObject found;

    [SerializeField]
    private string scene;

    private string uid;

    public GameObject matchPanel;

    public bool userAccepted;
    public bool matchAccepted;

    private void Start()
    {
        uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(match.match).Child("accepted").ValueChanged += HandleMatch;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid).Child("accepted").ValueChanged += HandleUser;
    }

    public void Rechazar()
    {
        found.SetActive(false);

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(match.match).Child("searching").SetValueAsync(false);

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(match.match).Child("match").SetValueAsync("");

        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(match.match).Child("accepted").SetValueAsync(false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(match.match).Child("accepted").SetValueAsync(true);
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(match.match).Child("accepted").SetValueAsync(false);
    }

    private void HandleUser(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        if (e.Snapshot.Exists)
        {
            Debug.Log(e.Snapshot.Child("accepted").GetRawJsonValue());
            Debug.Log(e.Snapshot.GetRawJsonValue());
            userAccepted = bool.Parse(e.Snapshot.GetRawJsonValue());
        }
        else
        {
            Debug.Log("No hay información en la ruta especificada.");
        }
    }

    private void HandleMatch(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        if (e.Snapshot.Exists)
        {
            Debug.Log(e.Snapshot.Child("accepted").GetRawJsonValue());
            Debug.Log(e.Snapshot.GetRawJsonValue());
            matchAccepted = bool.Parse(e.Snapshot.GetRawJsonValue());
        }
        else
        {
            Debug.Log("No hay información en la ruta especificada.");
        }
    }

    private void Update()
    {
        if (userAccepted == true && matchAccepted == true)
        {
            matchPanel.SetActive(true);
        }
    }

    public void Aceptar()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(true);
    }
}