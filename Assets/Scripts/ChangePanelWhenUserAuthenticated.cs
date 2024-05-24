using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;
using Firebase.Extensions;
using TMPro;
using System.Linq;

public class ChangePanelWhenUserAuthenticated : MonoBehaviour
{
    public GameObject LogInPanel;
    public GameObject GamePanel;
    public GameObject panelLeaderboardPrefab;
    public GameObject canvasTransform;
    [SerializeField] private TextMeshProUGUI[] userData;
    [SerializeField] private TextMeshProUGUI[] scoreData;
    private int count = 0;

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
        }
    }
    public void GetHighScore()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Faulted");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children.Reverse())
                {
                    userData[count].text = child.Child("username").Value.ToString() + " : ";
                    scoreData[count].text = child.Child("score").Value.ToString();
                    count++;
                }
            }
        });

    }
}

