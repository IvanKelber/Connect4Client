using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListItem : MonoBehaviour
{

    [SerializeField]
    private Client client;

    [SerializeField]
    private TMP_Text usernameText;

    private string username ="";

    public void UpdateUsername(string username) {
        this.username = username;
        usernameText.text = this.username;
    }

    public void SetClient(Client client) {
        this.client = client;
    }

    public void OnClick() {
        client.SendGameRequest(username);
    }
}
