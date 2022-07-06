using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCells
{
    [HideInInspector] public string CellIDName = "";
    public Vector2 ID = new Vector2(-1, -1);
    [HideInInspector] public bool isFilled = false;
    public string Color = "Red";
    public bool IsParent = false;
    public Vector2 OriginalPosition = new Vector2(0, 0);

    public GridCells(Vector2 id, bool isFilled, string name)
    {
        CellIDName = name;
        ID = id;
        this.isFilled = isFilled;
    }
}
public class GameManager : MonoBehaviour
{
    #region Managers
    PlayerSettingsManager playerSettings;
    #endregion

    [HideInInspector] public int gridSize = 4;
    [HideInInspector] public int minPieceCount = 6;
    [HideInInspector] public int maxPieceCount = 7;
    [SerializeField] private GameObject Cell;
    [SerializeField] private BoxCollider2D randomPoints;
    [SerializeField] private GameObject winButton;

    #region Lists
    public List<GameObject> Pieces;
    public List<GameObject> childPieces = new List<GameObject>();
    private List<GameObject> spawnedCells = new List<GameObject>();
    [HideInInspector] public List<GameObject> parentPieces = new List<GameObject>();
    private List<Vector2> spawnedPieces = new List<Vector2>();
    [HideInInspector] public List<GameObject> spawnedPiecesObj = new List<GameObject>();
    [HideInInspector] public List<GridCells> gridCells = new List<GridCells>();
    #endregion

    private bool isFinished = false;

    private void Awake()
    {
        playerSettings = FindObjectOfType<PlayerSettingsManager>();
        minPieceCount = 6;
        maxPieceCount = 7;

    }
    private void Start()
    {
        playerSettings._Difficulty = PlayerSettingsManager.Difficulty.Easy;
        GenerateGrid(gridSize);
    }

    #region Public 
    public void ReGenerateGrid()
    {
        ResetGrid();
        GenerateGrid(gridSize);
    }
    public void ResetGrid()
    {
        ResetList(spawnedCells);
        ResetList(parentPieces);
        ResetList(spawnedPiecesObj);

        spawnedPieces.Clear();

        gridCells.Clear();

        isFinished = false;
    }
    public void CheckWinLevel()
    {
        for(int i = 0; i < spawnedCells.Count; i++)
        {
            var cell = spawnedCells[i].GetComponent<Cell>();
            if(cell.snapedPiece == null)
            {
                print($"Cell: {cell.CellID} is null");
                return;
            }
        }

        print("Level Win");
        WinLevel();


    }
    #endregion

    #region For Load
    public void GenerateGridFromLoad()
    {
        PlayerSettingsManager._GameState = PlayerSettingsManager.GameState.Generate;

        var size = playerSettings.CurrentCellSize;

        playerSettings.CurrentCellSize = new Vector2(size.x, size.y);

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var spawnCell = Instantiate(Cell, new Vector3(x, y), Quaternion.identity);
                spawnCell.name = $"{x},{y}";

                var cell = spawnCell.GetComponent<Cell>();
                cell.CellID = new Vector2(x, y);
            }
        }
    }
    public void CombinePiecesFromLoad()
    {
        foreach (GameObject Parent in parentPieces)
        {
            foreach (GameObject Child in spawnedPiecesObj)
            {
                var parent = Parent.GetComponent<Piece>();
                var childe = Child.GetComponent<PieceSpawn>();

                if (parent.ColorName == childe.ColorName)
                {
                    Child.transform.parent = Parent.transform;
                }
            }
        }

        foreach(GameObject Parent in parentPieces)
        {
            var piece = Parent.GetComponent<Piece>();
            Parent.transform.position = piece.originalPosition;
        }

        PlayerSettingsManager._GameState = PlayerSettingsManager.GameState.InGame;
    }
    #endregion

    #region Private 
    void ResetList(List<GameObject> objList)
    {
        if(objList.Count != 0)
        {
            foreach (GameObject o in objList)
            {
                Destroy(o);
            }
        }

        objList.Clear();
    }
    void GenerateGrid(int gridSize)
    {
        PlayerSettingsManager._GameState = PlayerSettingsManager.GameState.Generate;

        var size = gridSize;

        playerSettings.CurrentCellSize = new Vector2(size, size);
        spawnedCells = new List<GameObject>(size);

        for (int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                var spawnCell = Instantiate(Cell, new Vector3(x, y), Quaternion.identity);
                spawnCell.name = $"{x},{y}";

                var cell = spawnCell.GetComponent<Cell>();
                cell.CellID = new Vector2(x, y);

                FindPieceNeighbors(playerSettings.CurrentCellSize, cell.CellID, cell.CellNeighbor); //Assign Neighbors

                var g = new GridCells(cell.CellID, false, $"{x} - {y}");

                gridCells.Add(g);
                spawnedCells.Add(spawnCell);
            }
        }

        int pieceCount = Random.Range(minPieceCount, maxPieceCount);

        SpawnPieces(spawnedCells, pieceCount, Pieces);
    }
    void SpawnPieces(List<GameObject> Cells, int AmountOfPieces, List<GameObject> Pieces)
    {
        List<GameObject> removedCells = new List<GameObject>();

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

            parentPieces.Add(piece);

            foreach (GridCells g in gridCells)
            {
                if (g.ID == cellID)
                {
                    g.isFilled = true;
                    g.IsParent = true;
                    g.Color = piecePiece.ColorName;
                }
            }

            removedCells.Add(cell);
            Cells.Remove(cell);
        }

        //Add Back Removed Cells
        foreach(GameObject g in removedCells)
        {
            Cells.Add(g);
        }

        //Check Near Pieces
        foreach(GameObject o in parentPieces)
        {
            var cellID = o.GetComponent<Piece>().inCellID;

            foreach(GameObject a in parentPieces)
            {
                if (a.GetComponent<Piece>().Neighbors.Contains(cellID))
                {
                    a.GetComponent<Piece>().Neighbors.Remove(cellID);
                }
            }
        }

        while(!isFinished) ScalePieces(parentPieces);
    }
    void ScalePieces(List<GameObject> parentPieces)
    {
        foreach (GameObject o in parentPieces)
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
                    var scaledPiece = Instantiate(piece.spawnPrefab, cellID, Quaternion.identity);

                    foreach (GridCells g in gridCells)
                    {
                        if (g.ID == cellID)
                        {
                            g.isFilled = true;
                            g.Color = scaledPiece.GetComponent<PieceSpawn>().ColorName;
                        }
                    }
                    spawnedPiecesObj.Add(scaledPiece);

                    scaledPiece.GetComponent<PieceSpawn>().CellID = cellID;
                    o.GetComponent<Piece>().lastSpawnedIDs.Add(cellID);
                    scaledPiece.GetComponent<PieceSpawn>().ColorName = o.GetComponent<Piece>().ColorName;

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
                }
            }

        }

        foreach (GameObject o in parentPieces)
        {
            var n = o.GetComponent<Piece>().Neighbors;

            foreach (GridCells g in gridCells)
            {
                for (int i = 0; i < n.Count; i++)
                {
                    if (g.ID == n[i] && g.isFilled)
                    {
                        n.Remove(g.ID);
                    }
                }
            }
        }

        List<bool> checkFinished = new List<bool>();

        foreach(GridCells g in gridCells)
        {
            checkFinished.Add(g.isFilled);
        }

        int count = 0;

        for(int i =0; i<checkFinished.Count; i++)
        {
            if (checkFinished[i]) count++;
        }

        if (count == checkFinished.Count) 
        {
            isFinished = true;
            CombinePieces();
            SetRandomPoints(randomPoints, parentPieces);
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
    void CombinePieces()
    {
        foreach(GameObject Parent in parentPieces)
        {
            var parent = Parent.GetComponent<Piece>();

            foreach (GameObject Child in spawnedPiecesObj)
            {
                var childe = Child.GetComponent<PieceSpawn>();

                if(parent.ColorName == childe.ColorName)
                {
                    Child.transform.parent = Parent.transform;
                }
            }
        }

    }
    void SetRandomPoints(BoxCollider2D collider, List<GameObject> parentPiece)
    {
        if (collider == null) return;

        var _colPos = (Vector2)collider.transform.position + collider.offset;

        foreach (GameObject Parent in parentPiece)
        {
            var piece = Parent.GetComponent<Piece>();

            float randomPosX = Random.Range
                (_colPos.x - collider.size.x / 2, 
                _colPos.x + collider.size.x / 2);

            float randomPosY = Random.Range
                (_colPos.y - collider.size.y / 2, 
                _colPos.y + collider.size.y / 2);

            piece.originalPosition = new Vector2(randomPosX, randomPosY);

            Parent.transform.position = new Vector2(randomPosX, randomPosY);

            for (int i = 0; i < gridCells.Count; i++)
            {
                if (piece.ColorName == gridCells[i].Color && gridCells[i].IsParent)
                {
                    gridCells[i].OriginalPosition = piece.originalPosition;
                }
                else if (!gridCells[i].IsParent)
                {
                    gridCells[i].OriginalPosition = new Vector2(0, 0);
                }
            }
        }
        PlayerSettingsManager._GameState = PlayerSettingsManager.GameState.InGame;
    }
    void WinLevel()
    {
        winButton.SetActive(true);
    }
    #endregion
}
