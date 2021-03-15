using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetUsernameButton : MonoBehaviour
{
    public InputField usernameField;

    [SerializeField]
    Client client;

    [SerializeField]
    private TMP_Text buttonText;

    public void Update() {
        buttonText.text = "Set username";
    }

    public void OnClick()
    {
        if(usernameField.text != "") {
            client.SetUsername(usernameField.text);
        } else {
            Debug.Log("Username must not be empty");
        }

        
    }
}