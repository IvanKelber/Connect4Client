using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class Board : MonoBehaviour
{

    public Vector2Int Dimensions = new Vector2Int(7,6);

    public Vector2 BoardRawDimensions = new Vector2(12,12);

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private GameObject piecePrefab;

    [SerializeField]
    private GameObject columnColliderPrefab;

    [SerializeField]
    private Rectangle border;

    [SerializeField]
    private BoardBase boardBase;

    private Tile[,] tiles;


    private List<Piece> pieceGOs = new List<Piece>();

    private List<BoxCollider2D> columnColliders = new List<BoxCollider2D>();

    private float cellWidth;
    private float cellHeight;

    public float SideLength {
        get {
            return cellWidth;
        }
    }

    private Vector3 topLeft;
    
    void Start() {
        Render();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Clear();
            Render();
        }

    }

    void CalculateCellWidth() {
        cellWidth = Mathf.Min(BoardRawDimensions.x/Dimensions.x, BoardRawDimensions.y/Dimensions.y);
        cellHeight = cellWidth;
    }

    public void Render() {
        tiles = new Tile[Dimensions.x, Dimensions.y];
        CalculateCellWidth();
        UpdateCorners();
        RenderTiles();
        border.Width = cellWidth * Dimensions.x;
        border.Height = cellHeight * Dimensions.y;
        border.Color = tiles[0,0].tileColor;
        boardBase.UpdateSize(border.Width + 1, .2f * cellHeight);
        boardBase.UpdatePosition(transform.position + Vector3.down * (border.Height/2 + (.2f * cellHeight)/2));
        CreateColumnColliders(Dimensions.x);
    }

    private Vector3 GetDropPosition(int column) {
        return tiles[column - 1, 0].transform.position + Vector3.up * cellHeight * 1.5f;
    }

    private void CreateColumnColliders(int columns) {
        for(int i = 0; i < columns; i++) {
            Vector3 position = new Vector3(tiles[i, 0].transform.position.x, 0, transform.position.z); 
            ColumnCollider col = Instantiate(columnColliderPrefab, position, Quaternion.identity).GetComponent<ColumnCollider>();
            col.Init(this, i);
            col.UpdateSize(new Vector2(cellHeight, cellHeight * Dimensions.y));
        }
    }

    public void PlacePiece(int playerId, Color playerColor, int column) {
        Piece piece = Instantiate(piecePrefab, GetDropPosition(column), Quaternion.identity).GetComponent<Piece>();
        pieceGOs.Add(piece);
        
        piece.SetColor(playerColor);
        piece.UpdateSize(SideLength);
        piece.SetBoard(this);
        piece.transform.parent = transform;
    
    }

    void Clear() {
        foreach(Tile tile in tiles) {
            Destroy(tile.gameObject);
        }
        foreach(Piece piece in pieceGOs) {
            if(piece != null) {
                Destroy(piece.gameObject);
            }
        }
    }

    void UpdateCorners() {
        topLeft = transform.position + Vector3.left * (cellWidth * Dimensions.x/2 - cellWidth/2)
                                     + Vector3.up   * (cellHeight * Dimensions.y/2 - cellHeight/2);
    }

    void RenderTiles() {
        for(int i = 0; i < Dimensions.x; i++) {
            
            for(int j = 0; j < Dimensions.y; j++) {
                Vector3 tileCenter = topLeft + Vector3.right * i * cellWidth
                                             + Vector3.down * j * cellHeight;
                GameObject tile = Instantiate(tilePrefab, tileCenter, Quaternion.identity);
                tile.transform.parent = transform;
                Tile t = tile.GetComponent<Tile>();
                t.UpdateSideLength(cellWidth);
                tiles[i,j] = t;
            }
        }
    }
}
