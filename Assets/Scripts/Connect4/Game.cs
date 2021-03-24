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

    private bool gameOver = false;
    private bool iWon = false;

    private bool enemyPieceFalling = false;

    private int columnPlaced;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            isMyTurn = true;
        }
        mousePosition = danielCam.ScreenToWorldPoint(Input.mousePosition);
        if(IsMyTurn) {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0, clickMask);
            int columnPlaced = -1;
            if(hit) {
                ColumnCollider col = hit.transform.GetComponent<ColumnCollider>();
                if(col != null) {
                    columnPlaced = col.CheckColumn();
                    if(columnPlaced >= 0) { 
                        int row = board.FindPieceRowFromColumn(columnPlaced);
                        board.ShowPieceHint(columnPlaced, row, clientColor);
                    } else {
                        board.HidePieceHint();
                    }
                }
            } else {
                // If we are not hovering over a column
                board.HidePieceHint();
            }
            if(Input.GetMouseButtonDown(0) && columnPlaced >= 0) {
                board.DropPiece(columnPlaced, clientColor);
                this.columnPlaced = columnPlaced;
                EndTurn();
            }
        }
    }

    public void PlaceOpponentPiece(int column) {
        board.DropPiece(column, opponentColor);
        enemyPieceFalling = true;
    } 

    public void StartTurn() {
        isMyTurn = true;
    }

    public void EndTurn() {
        isMyTurn = false;
        board.HidePieceHint();
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
 
    public void GameOver(bool iWon) {
        this.gameOver = true;
        this.iWon = iWon;
        if(!enemyPieceFalling) {
            EndGame();
        }
    }

    public void OnAnimationComplete() {
        if(enemyPieceFalling) {
            enemyPieceFalling = false;
            isMyTurn = !gameOver;
        } else {
            client.PlacePiece(gameId, columnPlaced);
            EndTurn();
        }

        if(gameOver) {
            EndGame();
        }
    }

    public void EndGame() {
        Debug.Log("GAME IS OVER");
        Debug.Log(iWon? "AND I AM THE WINNER" : "AND I AM THE LOSER");
    }

}
