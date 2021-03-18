using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    private Client client;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Camera danielCam;

    [SerializeField]
    private LayerMask clickMask;

    [SerializeField]
    private Board board;

    private string gameId;
    private bool isMyTurn;
    public bool IsMyTurn {
        get {
            return isMyTurn;
        }
    }
    private Color clientColor = Color.red;
    private Color opponentColor;

    private Vector3 mousePosition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            isMyTurn = true;
        }
        mousePosition = danielCam.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) && IsMyTurn) {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0, clickMask);
            if(hit) {
                ColumnCollider col = hit.transform.GetComponent<ColumnCollider>();
                if(col != null) {
                    int columnPlaced = col.CheckColumn();
                    if(columnPlaced >= 0) {
                        board.DropPiece(columnPlaced, clientColor);
                        client.PlacePiece(gameId, columnPlaced);
                        EndTurn();
                    }
                }
            }
        }
    }

    public void StartTurn() {
        isMyTurn = true;
    }

    public void EndTurn() {
        isMyTurn = false;
    }

    public void StartGame(string gameId, bool isMyTurn) {
        this.gameId = gameId;
        this.isMyTurn = isMyTurn;
        if(isMyTurn) {
            clientColor = Color.red;
            opponentColor = Color.black;
        } else {
            clientColor = Color.black;
            opponentColor = Color.red;
        }
        canvas.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

}
