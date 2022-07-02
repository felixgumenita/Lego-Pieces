using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<GameObject> currentCells;

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

        currentCells = new List<GameObject>(xSize * ySize);

        playerSettings.CurrentCellSize = new Vector2(xSize, ySize);

        for (int y = 0; y < ySize; y++)
        {
            for(int x = 0; x < xSize; x++)
            {
                var spawnCell = Instantiate(Cell, new Vector3(x, y), Quaternion.identity);
                spawnCell.name = $"{x + 1},{y + 1}";

                var cell = spawnCell.GetComponent<Cell>();
                cell.CellID = new Vector2(x + 1, y + 1);

                cell.FindNeighbors(playerSettings.CurrentCellSize); //Assign Neighbors

                currentCells.Add(spawnCell);
            }
        }

        var _camera = Camera.main;

        _camera.transform.position = new Vector3((float)xSize - .1f , (float)ySize / 2 - .5f, -10);

    }
}
