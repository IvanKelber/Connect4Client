using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    private Client client;

    [SerializeField]
    private Canvas canvas;

    private string gameId;
    private bool isMyTurn;
    public bool IsMyTurn {
        get {
            return isMyTurn;
        }
    }
    private Color chipColor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(string gameId, bool isMyTurn) {
        this.gameId = gameId;
        this.isMyTurn = isMyTurn;
        if(isMyTurn) {
            chipColor = Color.red;
        } else {
            chipColor = Color.black;
        }
        canvas.gameObject.SetActive(false);
    }
}
