using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using System;

public class NotificationManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    [SerializeField] private GameObject notification_Connected;
    [SerializeField] private GameObject notification_Disconnected;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SetNotificationConnected(string friendName)
    {
        notification_Connected.SetActive(true);
        notification_Connected.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = friendName + " just connected";
        StartCoroutine(Notification());
    }
    
    private IEnumerator Notification()
    {
        yield return new WaitForSeconds(5);
        notification_Connected.SetActive(false);
        notification_Disconnected.SetActive(false);
    }

    
}
