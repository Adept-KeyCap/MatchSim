using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
        [SerializeField]
        private Button _loginButton;
        [SerializeField]
        private TMP_InputField _emailInputField;
        [SerializeField]
        private TMP_InputField _passwordInputField;
        [SerializeField] private Auth authVar;
        public ErrorMessage _errorMessage;
        void Reset()
        {
            _loginButton = GetComponent<Button>();
            _emailInputField = GameObject.Find("InputFieldEmail").GetComponent<TMP_InputField>();
            _passwordInputField = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>();
        }
        
        void Start()
        {
            _loginButton.onClick.AddListener(HandleSignupButtonClicked);
        }
    
        private void HandleSignupButtonClicked()
        {
            var auth = FirebaseAuth.DefaultInstance;
            auth.SignInWithEmailAndPasswordAsync(_emailInputField.text, _passwordInputField.text).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    _errorMessage.ShowErrorMessage("Invalid email or password");
                    return;
                }
                AuthResult result = task.Result;
                Debug.LogFormat("User was logged in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                
                authVar.ChangeAuthState();
                
            });
        } 
        
        
}
