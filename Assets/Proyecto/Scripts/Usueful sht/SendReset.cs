using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendReset : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_Text messageText;

    public void RecoverPassword()
    {
        string email = emailInput.text;

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            {
                messageText.text = "Se ha enviado un correo electrónico para restablecer tu contraseña.";
            }
            else
            {
                messageText.text = "Ha ocurrido un error al intentar enviar el correo electrónico. Por favor, intenta de nuevo.";
            }
        });
    }
}
