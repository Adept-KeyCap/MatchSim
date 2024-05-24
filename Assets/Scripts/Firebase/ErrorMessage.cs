using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ErrorMessage : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI errorTxt;

    private void Start()
    {
        errorTxt = GetComponent<TextMeshProUGUI>();
    }

    public void ShowErrorMessage(string Error)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(ShowError(Error));
    }

    private IEnumerator ShowError(string error)
    {
        errorTxt.text = error;
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
