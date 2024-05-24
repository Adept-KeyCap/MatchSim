using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class ScoreSetter : MonoBehaviour
{
    // Start is called before the first frame update
       void Start()
        {
            SetScore();
        }
    
    public void SetScore()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        int amount = 100;
        
        //If the user has a score in the database, check if the new score is higher than the old one
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid).Child("score").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting score: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value != null)
                {
                    int oldScore = int.Parse(snapshot.Value.ToString());
                    if (amount > oldScore)
                    {
                        FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid).Child("score").SetValueAsync(amount);
                    }
                    else
                    {
                        //Highscore.SetAmount(oldScore);
                        //Highscore.DisplayAmount();
                    }
                }
                else
                {
                    FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid).Child("score").SetValueAsync(amount);
                }
            }
        });
    }
}
