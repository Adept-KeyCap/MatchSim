using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;
using Firebase.Extensions;
using TMPro;
using System.Linq;
using UnityEngine.Events;

public class ChangePanelWhenUserAuthenticated : MonoBehaviour
{
    public GameObject LogInPanel;
    public GameObject GamePanel;

    public UnityEvent whenAutenticated;


    [SerializeField]
    private TMP_Text labelUsername;
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;
    }

    public void SetLabels(string username)
    {
        labelUsername.text = username;
    }

    private void HandleAuthStateChange(object sender, EventArgs e) 
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null) 
        {
            GamePanel.SetActive(true);
            LogInPanel.SetActive(false);

            whenAutenticated.Invoke();
        }
    }
}

