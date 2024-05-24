using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneWhenAccept : MonoBehaviour
{
    [SerializeField]
    private string scene;

    private void Start()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("accepted").ValueChanged += LoadScene;
    }

    private void LoadScene(object sender, ValueChangedEventArgs e)
    {
        SceneManager.LoadScene(scene);
    }
}
