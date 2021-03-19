using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using ScriptableObjectArchitecture;

public class Piece : MonoBehaviour
{

    BoxCollider2D collider;
    
    [SerializeField]
    private Rectangle decal;

    [SerializeField]
    private Disc body;

    [SerializeField]
    private GameEvent animationComplete;

    private Board board;

    private bool atRest = true;
    private Rigidbody2D rb;

    void Awake() {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start() {
        rb.velocity = Vector2.down * .01f;
        atRest = false;
    }

    public void UpdateSize(float sideLength) {
        collider.size = new Vector2(sideLength/1.2f,sideLength);
        body.Radius = sideLength/2;
        decal.Width = decal.Height = sideLength/2;
    }

    public void SetColor(Color color) {
        body.Color = color;
    }

    public void SetBoard(Board board) {
        this.board = board;
    }

    public void DisablePhysics() {
        rb.isKinematic = true;
        collider.enabled = false;
    }

    void Update() {
        if(!atRest && rb.velocity == Vector2.zero) {
            animationComplete.Raise();
            atRest = true;
        }
    }
    
}
