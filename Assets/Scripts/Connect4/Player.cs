using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField]
    private Board board;
    public Board Board {
        get {
            return board;
        }
    }

    [SerializeField]
    private Color playerColor;

    public bool TakingTurn = false;

    public int Id;

    public void StartTurn() {
        TakingTurn = true;
    }

    public void PlacePiece(int column) {
        TakingTurn = false;
        board.PlacePiece(Id, playerColor, column);
    }
}
