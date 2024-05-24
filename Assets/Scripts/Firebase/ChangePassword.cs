using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangePassword : MonoBehaviour
{
    [SerializeField] private Button _changePasswordButton; 
    [SerializeField] private ErrorMessage _errorMessage;
    private DatabaseReference dataBaseReference;
    
    private void Reset()
    {
        _changePasswordButton = GetComponent<Button>();
    }
    void Start()
    {
        _changePasswordButton.onClick.AddListener(HandleChangePasswordButtonClicked);
        dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    private void HandleChangePasswordButtonClicked()
    {
        string email = GameObject.Find("InputFieldEmail").GetComponent<TMP_InputField>().text;
        StartCoroutine(ChangeThePassword(email));
    }

    IEnumerator ChangeThePassword(string email)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.SendPasswordResetEmailAsync(email);
        yield return new WaitUntil(()=>registerTask.IsCompleted);
        if (registerTask.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else if (registerTask.IsFaulted)
        {
            Debug.Log("Encountered an error" + registerTask.Exception);
        }
        else
        {
            _errorMessage.ShowErrorMessage("the email was sent to change the password");
            Debug.LogFormat($"The email was sent to change the password");

        }
    }
}
