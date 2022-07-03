using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCells
{
    [HideInInspector] public string CellIDName = "";
    public Vector2 ID = new Vector2(-1, -1);
    public bool isFilled = false;

    public GridCells(Vector2 id, bool isFilled, string name)
    {
        CellIDName = name;
        ID = id;
        this.isFilled = isFilled;
    }
}
public class GridManager : MonoBehaviour
{
    #region Managers
    PlayerSettingsManager playerSettings;
    #endregion

    private int minSize = 4;
    private int maxSize = 6;
    [SerializeField] private int pieceCount = 6;
    [SerializeField] private GameObject Cell;
    [SerializeField] private List<GameObject> Pieces;

    private List<GameObject> currentCells = new List<GameObject>();
    private List<GameObject> currentPieces = new List<GameObject>();
    private List<Vector2> spawnedPieces = new List<Vector2>();
    private List<GridCells> gridCells = new List<GridCells>();

    private void Awake()
    {
        playerSettings = FindObjectOfType<PlayerSettingsManager>();
    }
    private void Start()
    {
        GenerateGrid(minSize, maxSize);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GenerateGrid(minSize, maxSize);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnPieces(currentCells, pieceCount, Pieces);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            ScalePieces(currentPieces);
        }
    }
    void GenerateGrid(int _MinSize, int _MaxSize)
    {
        if(currentCells != null)
        {
            foreach(GameObject g in currentCells)
            {
                Destroy(g);
            }
        }

        var xSize = Random.Range(_MinSize, _MaxSize);

        var ySize = Random.Range(_MinSize, _MaxSize);

        playerSettings.CurrentCellSize = new Vector2(xSize, ySize);

        for (int y = 0; y < ySize; y++)
        {
            for(int x = 0; x < xSize; x++)
            {
                var spawnCell = Instantiate(Cell, new Vector3(x, y), Quaternion.identity);
                spawnCell.name = $"{x},{y}";

                var cell = spawnCell.GetComponent<Cell>();
                cell.CellID = new Vector2(x, y);

                cell.FindNeighbors(playerSettings.CurrentCellSize); //Assign Neighbors

                var g = new GridCells(cell.CellID, false, $"{x} - {y}");

                gridCells.Add(g);
                currentCells.Add(spawnCell);
            }
        }
    }
    void SpawnPieces(List<GameObject> Cells, int AmountOfPieces, List<GameObject> Pieces)
    {
        for(int i = 0; i < AmountOfPieces; i++)
        {
            var random = Random.Range(0, Cells.Count);

            var cell = Cells[random];

            var cellID = cell.GetComponent<Cell>().CellID;

            var cellNeighbors = cell.GetComponent<Cell>().CellNeighbor;

            var piece = Instantiate(Pieces[i], cellID, Quaternion.identity);
            var piecePiece = piece.GetComponent<Piece>();


            piecePiece.Neighbors = cellNeighbors;
            piecePiece.inCellID = cellID;

            currentPieces.Add(piece);

            foreach (GridCells g in gridCells)
            {
                if (g.ID == cellID)
                {
                    g.isFilled = true;
                }
            }

            Cells.Remove(cell);
        }

        //Check Near Pieces
        foreach(GameObject o in currentPieces)
        {
            var cellID = o.GetComponent<Piece>().inCellID;

            foreach(GameObject a in currentPieces)
            {
                if (a.GetComponent<Piece>().Neighbors.Contains(cellID))
                {
                    a.GetComponent<Piece>().Neighbors.Remove(cellID);
                }
            }
        }
    }
    void ScalePieces(List<GameObject> CurrentPieces)
    {
        foreach (GameObject o in CurrentPieces)
        {
            var neighbors = o.GetComponent<Piece>().Neighbors;

            if (neighbors.Count != 0)
            {
                var cellID = new Vector2(neighbors[0].x, neighbors[0].y);
                var piece = o.GetComponent<Piece>();
                bool canSpawn = true;

                foreach (GridCells g in gridCells)
                {
                    if (g.ID == cellID && !g.isFilled) canSpawn = true;
                    else if (g.ID == cellID && g.isFilled)
                    {
                        canSpawn = false;
                        neighbors.Remove(cellID);
                    }
                }

                if (canSpawn)
                {
                    foreach (GridCells g in gridCells)
                    {
                        if (g.ID == cellID) g.isFilled = true;
                    }

                    var scaledPiece = Instantiate(piece.spawnPrefab, cellID, Quaternion.identity);

                    scaledPiece.GetComponent<PieceSpawn>().CellID = cellID;
                    o.GetComponent<Piece>().lastSpawnedIDs.Add(cellID);

                    spawnedPieces.Add(scaledPiece.GetComponent<PieceSpawn>().CellID);

                    var scaledPiecesN = scaledPiece.GetComponent<PieceSpawn>().CellNeighbor;

                    FindPieceNeighbors(playerSettings.CurrentCellSize, cellID, scaledPiecesN);

                    foreach (Vector2 v in scaledPiecesN)
                    {
                        neighbors.Add(v);
                    }

                    foreach (Vector2 v in piece.lastSpawnedIDs)
                    {
                        neighbors.Remove(v);
                    }

                    neighbors.Remove(piece.inCellID);

                    o.transform.parent = scaledPiece.transform;
                }
            }
            
        }

        foreach(GameObject o in currentPieces)
        {
            var n = o.GetComponent<Piece>().Neighbors;

            foreach(GridCells g in gridCells)
            {
                for(int i = 0; i < n.Count; i++)
                {
                    if (g.ID == n[i] && g.isFilled)
                    {
                        n.Remove(g.ID);
                    }
                }
            }
        }

    }
    void FindPieceNeighbors(Vector2 GridSize, Vector2 CellID, List<Vector2> CellNeighbor)
    {
        var x = GridSize.x;
        var y = GridSize.y;

        var cX = CellID.x;
        var cY = CellID.y;


        if (cX == 0 && cY == 0) //Left bottom corner
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x - 1 == cX && cY == 0) //Right bottom corner
        {
            var n1 = new Vector2(cX - 1, cY);
            var n2 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (cX == 0 && y - 1 == cY) //Left up corner
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x - 1 == cX && y - 1 == cY) //Right up corner
        {
            var n1 = new Vector2(cX - 1, cY);
            var n2 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (cX == 0) //Left edge
        {
            var n1 = new Vector2(cX, cY + 1);
            var n2 = new Vector2(cX, cY - 1);
            var n3 = new Vector2(cX + 1, cY);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (x - 1 == cX) //Right edge
        {
            var n1 = new Vector2(cX, cY + 1);
            var n2 = new Vector2(cX, cY - 1);
            var n3 = new Vector2(cX - 1, cY);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (y - 1 == cY) //Up edge
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX - 1, cY);
            var n3 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (cY == 0) //Down edge
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX - 1, cY);
            var n3 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else //Middle Parts
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX - 1, cY);
            var n3 = new Vector2(cX, cY + 1);
            var n4 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
            CellNeighbor.Add(n4);
        }
    }
}
