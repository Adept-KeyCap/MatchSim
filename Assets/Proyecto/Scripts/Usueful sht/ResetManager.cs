using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [SerializeField] private GameObject resetPanel;
    [SerializeField] private GameObject loginPanel;

    public void ShowReset()
    {
        loginPanel.SetActive(false);
        resetPanel.SetActive(true);
    }

    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        resetPanel.SetActive(false);
    }

}
