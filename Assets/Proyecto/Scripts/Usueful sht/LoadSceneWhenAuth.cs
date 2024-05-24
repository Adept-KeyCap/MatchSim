using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWhenAuth : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;
    }

    private void HandleAuthStateChange(object sender, EventArgs e)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log(sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }

    }
}
