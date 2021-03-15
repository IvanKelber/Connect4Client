using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    [SerializeField]
    private GameObject listItemPrefab;

    [SerializeField]
    private Client client;

    List<string> playersInLobby = new List<string>();
    private bool isDirty = false;
    
    [SerializeField]
    private List<string> names;



    void Start()
    {
        
    }

    public void SetPlayersInLobby(List<string> players) {
        players.Sort();
        playersInLobby = players; 
        isDirty = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space)) {
        //     List<string> players = new List<string>();
        //     foreach(string name in names) {
        //         if(Random.value >= .5f) {
        //             players.Add(name);
        //         }
        //     }
        //     SetPlayersInLobby(players);
        // }

        if(isDirty) {
            UpdateList();
        }
    }

    void UpdateList() {
        int i = 0;
        foreach(Transform t in transform) {
            if(i >= playersInLobby.Count) {
                t.gameObject.SetActive(false);
            } else {
                t.gameObject.SetActive(true);
                ListItem listItem = t.GetComponent<ListItem>();
                listItem.SetClient(client);
                listItem.UpdateUsername(playersInLobby[i]);
            }
            i++;
        }
        for(;i < playersInLobby.Count;i++) {
            ListItem listItem = Instantiate(listItemPrefab, transform.position, Quaternion.identity).GetComponent<ListItem>();
            listItem.transform.parent = transform;
            listItem.SetClient(client);
            listItem.UpdateUsername(playersInLobby[i]);
        }
        isDirty = false;
    }
}
