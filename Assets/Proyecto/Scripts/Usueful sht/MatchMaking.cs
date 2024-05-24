using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MatchMaking : MonoBehaviour
{
    public List<string> activeIds = new List<string>();

    [SerializeField]
    private ChangeSearch searching;
    [SerializeField]
    private GameObject found;

    private string uid;
    public string match;

    void Start()
    {
        found.SetActive(false);

        uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("searching").LimitToLast(5).ValueChanged += HandleValueChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (searching.isSearch == true)
        {
            if (activeIds.Count >= 2)
            {
                SelectUser();
            }
        }
    }

    private void SelectUser()
    {
        List<string> otherIds = activeIds.Where(id => id != uid).ToList();

        if (otherIds.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, otherIds.Count);
            match = otherIds[randomIndex];

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(match).Child("searching").SetValueAsync(false);

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("match").SetValueAsync(match);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(match).Child("match").SetValueAsync(uid);

            found.SetActive(true);

            Debug.Log("Selected user ID: " + match);
        }
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs e)
    {
        foreach (DataSnapshot snapshot in e.Snapshot.Children)
        {
            bool searching = snapshot.Child("searching").Value != null && (bool)snapshot.Child("searching").Value;
            string userId = snapshot.Key;

            if (searching)
            {
                if (!activeIds.Contains(userId))
                {
                    activeIds.Add(userId);
                }
            }
            else
            {
                if (activeIds.Contains(userId))
                {
                    activeIds.Remove(userId);
                }
            }
        }

        Debug.Log("Current searching users: " + string.Join(", ", activeIds));
    }
}