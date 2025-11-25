using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject logInBox;
    public TextMeshProUGUI passwordBox;
    public TMP_InputField inputPassBox;

    private string normalPassword = "NormPass";
    private string adminPassword = "AdminPass";

    public void AttemptLogIn()
    {
        if (inputPassBox.text == normalPassword)
        {
            Debug.Log("Log in successful");
            logInBox.SetActive(false);
        }
        else
        {
            Debug.Log("Log in failed.");
        }
    }
}
