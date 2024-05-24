using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Firebase.Auth;

public class LogoutButton : MonoBehaviour
{
    public GameObject LogInPanel;
    public GameObject GamePanel;
    //public NewManager _nManager;
    public void OnPointerClick() 
    {   
        //_nManager.Reset();
        FirebaseAuth.DefaultInstance.SignOut();
        GamePanel.SetActive(false);
        LogInPanel.SetActive(true);
    }
}
