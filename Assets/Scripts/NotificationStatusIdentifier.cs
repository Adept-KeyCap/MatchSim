using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationStatusIdentifier : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI notificationText;

    public void SetNotificationConnected(string friendName)
    {
        panel.SetActive(true);
        notificationText.text = friendName + " just connected";
        StartCoroutine(DisableNotificationAfterSeconds(2.5f));
    }

    private IEnumerator DisableNotificationAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panel.SetActive(false);
        notificationText.text = "";
    }
}
