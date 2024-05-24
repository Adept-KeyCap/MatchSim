using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SocialPlatforms.Impl;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField userInput;

    private DatabaseReference dataBaseRef;

    public void Start()
    {
        dataBaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void Registrar()
    {
        StartCoroutine(RegistrarUsuario(userInput.text,emailInput.text, passwordInput.text));
    }

    public void LoginUser()
    {
        StartCoroutine(LoginUsuario(userInput.text, emailInput.text, passwordInput.text));
    }

    private IEnumerator RegistrarUsuario(string usuario, string email, string contraseña)
    {
        var auth = FirebaseAuth.DefaultInstance;

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email,contraseña);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.IsCanceled)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
        }
        else if (registerTask.IsFaulted)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + registerTask.Exception);
        }
        else
        {
            Firebase.Auth.AuthResult result = registerTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
            dataBaseRef.Child("users").Child(result.User.UserId).Child("username").SetValueAsync(usuario);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(result.User.UserId).Child("searching").SetValueAsync(false);
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(result.User.UserId).Child("match").SetValueAsync("");
            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(result.User.UserId).Child("accepted").SetValueAsync(false);
        }
    }

    private IEnumerator LoginUsuario(string usuario,string email, string contraseña)
    {
        var auth = FirebaseAuth.DefaultInstance;

        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, contraseña);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.IsCanceled)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
        }
        else if (loginTask.IsFaulted)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + loginTask.Exception);
        }
        else
        {
            Firebase.Auth.AuthResult result = loginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
        }
    }



}
