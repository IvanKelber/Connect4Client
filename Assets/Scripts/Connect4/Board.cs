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

    [SerializeField]
    private Piece pieceHint;

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
        pieceHint.gameObject.SetActive(false);
        pieceHint.DisablePhysics();
        pieceHint.SetBoard(this);
        pieceHint.UpdateSize(SideLength);
    }

    void Update() {


    }

    void CalculateCellWidth() {
        cellWidth = Mathf.Min(BoardRawDimensions.x/Dimensions.x, BoardRawDimensions.y/Dimensions.y);
        cellHeight = cellWidth;
    }

    public void Render() {
        tiles = new Tile[Dimensions.x, Dimensions.y];
        CalculateCellWidth();
        UpdateCorners();
        // StartCoroutine(SlowRender());
        RenderTiles();
        border.Width = cellWidth * Dimensions.x;
        border.Height = cellHeight * Dimensions.y;
        border.Color = tiles[0,0].tileColor;
        boardBase.UpdateSize(border.Width + 1, .2f * cellHeight);
        boardBase.UpdatePosition(transform.position + Vector3.down * (border.Height/2 + (.2f * cellHeight)/2));
        CreateColumnColliders(Dimensions.x);
    }


    // Used for debugging the order of tile creation
    private IEnumerator SlowRender() {
        for(int i = 0; i < Dimensions.x; i++) {
            
            for(int j = 0; j < Dimensions.y; j++) {
                yield return new WaitForSeconds(.1f);
                Vector3 tileCenter = topLeft + Vector3.right * i * cellWidth
                                             + Vector3.down * j * cellHeight;
                GameObject tile = Instantiate(tilePrefab, tileCenter, Quaternion.identity);
                tile.transform.parent = transform;
                Tile t = tile.GetComponent<Tile>();
                t.UpdateSideLength(cellWidth);
                tiles[i,j] = t;
            }
        }

        border.Width = cellWidth * Dimensions.x;
        border.Height = cellHeight * Dimensions.y;
        border.Color = tiles[0,0].tileColor;
        boardBase.UpdateSize(border.Width + 1, .2f * cellHeight);
        boardBase.UpdatePosition(transform.position + Vector3.down * (border.Height/2 + (.2f * cellHeight)/2));
        CreateColumnColliders(Dimensions.x);

    }

    private Vector3 GetDropPosition(int column) {
        return tiles[column, 0].transform.position + Vector3.up * cellHeight * 1.5f;
    }

    private void CreateColumnColliders(int columns) {
        for(int i = 0; i < columns; i++) {
            Vector3 position = new Vector3(tiles[i, 0].transform.position.x, 0, transform.position.z); 
            ColumnCollider col = Instantiate(columnColliderPrefab, position, Quaternion.identity).GetComponent<ColumnCollider>();
            col.Init(this, i);
            col.UpdateSize(new Vector2(cellHeight, cellHeight * Dimensions.y));
            col.transform.parent = transform;
        }
    }

    public int FindPieceRowFromColumn(int column) {
        int i = Dimensions.y - 1; // Number of rows
        while(i >= 0 && tiles[column, i].IsOccupied) {
            i--;
        }
        return i;
    }

    public void DropPiece(int column, Color pieceColor) {
        // Column is validated before this function is called
        Piece piece = Instantiate(piecePrefab, GetDropPosition(column), Quaternion.identity).GetComponent<Piece>();
        pieceGOs.Add(piece);
        piece.SetColor(pieceColor);
        piece.UpdateSize(SideLength);
        piece.SetBoard(this);
        piece.transform.parent = transform;

        tiles[column,FindPieceRowFromColumn(column)].SetOccupied();
    }

    public void ShowPieceHint(int column, int row, Color color) {
        pieceHint.gameObject.SetActive(true);
        pieceHint.SetColor(new Color(color.r, color.g, color.b, .5f));
        pieceHint.transform.position = tiles[column, row].transform.position;
    }

    public void HidePieceHint() {
        pieceHint.gameObject.SetActive(false);
    }

    public bool IsColumnValid(int column) {
        for(int i = 0; i < Dimensions.y; i++) {
            if(!tiles[column, i].IsOccupied) {
                return true;
            }
        }
        return false;
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
