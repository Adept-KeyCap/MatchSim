using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGame : MonoBehaviour
{
    private string uid;
    [SerializeField] private string scene;

    private bool match;
    private bool user;

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
            match = bool.Parse(e.Snapshot.GetRawJsonValue());
        }
        else
        {
            Debug.Log("No hay información en la ruta especificada.");
        }

        if (match == false)
        {

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("searching").SetValueAsync(false);

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("match").SetValueAsync("");

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("accepted").SetValueAsync(false);

            SceneManager.LoadScene(scene);
        }
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
            user = bool.Parse(e.Snapshot.GetRawJsonValue());
        }
        else
        {
            Debug.Log("No hay información en la ruta especificada.");
        }

        if (user == false)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("searching").SetValueAsync(false);

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync("");
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("match").SetValueAsync("");

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(matchId).Child("accepted").SetValueAsync(false);

            SceneManager.LoadScene(scene);
        }
    }

    public void Back()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").SetValueAsync(false);
    }
}