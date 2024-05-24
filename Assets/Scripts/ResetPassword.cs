using System.Collections;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetPassword : MonoBehaviour
{
    [SerializeField] 
    private Button _resetPasswordButton;
    private DatabaseReference dataBaseReference;
    public GameObject sentMailSuccess;

    private void Reset()
    {
        _resetPasswordButton = GetComponent<Button>();
    }
    void Start()
    {
        _resetPasswordButton.onClick.AddListener(HandleChangePasswordButtonClicked);
        dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;

    }
    public void HandleChangePasswordButtonClicked()
    {
        string email = GameObject.Find("ResetInputFieldEmail").GetComponent<TMP_InputField>().text;
        StartCoroutine(ChangeThePassword(email));
    }

    IEnumerator ChangeThePassword(string email)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.SendPasswordResetEmailAsync(email);
        yield return new WaitUntil(() => registerTask.IsCompleted);
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
            Debug.LogFormat($"The email was sent to change the password");
            sentMailSuccess.SetActive(true);
        }
    }
}
