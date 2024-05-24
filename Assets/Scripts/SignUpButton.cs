using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;

public class SignUpButton : MonoBehaviour
{
    [SerializeField]
    private Button _signupButton;
    [SerializeField]
    private TMP_InputField _emailInputField;
    [SerializeField]
    private TMP_InputField _usernameInputField;
    [SerializeField]
    private TMP_InputField _passwordInputField;

    private DatabaseReference _mDatabaseRef;

    private void Reset()
    {
        _signupButton = GetComponent<Button>();
        _emailInputField = GameObject.Find("InputFieldEmail").GetComponent<TMP_InputField>();
        _usernameInputField = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>();
        _passwordInputField = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>();
    }

    void Start()
    {
        //_signupButton.onClick.AddListener(HandleSignupButtonClicked);
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void HandleSignupButtonClicked() 
    {
        StartCoroutine(RegisterUser());
    }

    private IEnumerator RegisterUser() 
    {
        var auth = FirebaseAuth.DefaultInstance;

        var signuptask = auth.CreateUserWithEmailAndPasswordAsync(_emailInputField.text, _passwordInputField.text);

        Debug.Log("Creando user");

        yield return new WaitUntil(() => signuptask.IsCompleted);

        Debug.Log("Creando user 2");

        if (signuptask.IsCanceled)
        {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
        }

        else if (signuptask.IsFaulted)
        {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + signuptask.Exception);
        }

        else 
        {
            // Firebase user has been created.
            Firebase.Auth.AuthResult result = signuptask.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            _mDatabaseRef.Child("users").Child(result.User.UserId).Child("username").SetValueAsync(_usernameInputField.text);
        }
    }
}
