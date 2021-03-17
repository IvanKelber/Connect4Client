using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnCollider : MonoBehaviour
{

    private Board board;

    private BoxCollider2D boxCollider;

    private int column;
    public int Column {
        get {
            return column;
        }
        set {
            column = value;
        }
    }
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void UpdateSize(Vector2 size) {
        boxCollider.size = size;
    }

    public void Init(Board board, int column) {
        this.board = board;
        this.column = column;
    }

    public void OnClick() {
        Debug.Log($"Column {column} clicked");
    }
}
