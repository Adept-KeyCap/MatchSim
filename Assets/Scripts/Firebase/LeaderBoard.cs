using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.Serialization;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] scores;
    private int count=0;

    public void Get5BestUsersHighScoreInOrder()
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
                    scores[count].text = child.Child("username").Value.ToString() + " : " + child.Child("score").Value.ToString();
                    count++;
                }
            }
        });
        
    }
   
}
