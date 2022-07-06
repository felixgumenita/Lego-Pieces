using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsManager : MonoBehaviour
{
    #region Managers
    private GameManager gameManager;
    #endregion

    public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 };
    public enum GameState { Generate, Dragging, InGame };

    [SerializeField] private LayerMask InputLayerMask;

    public Difficulty _Difficulty = Difficulty.Easy;
    public static GameState _GameState = GameState.Generate;

    public Vector2 CurrentCellSize = new Vector2(0, 0);

    public List<GridCells> Grids = new List<GridCells>();

    public void SetDifficulty(int value)
    {
        gameManager = FindObjectOfType<GameManager>(); 

        switch (value)
        {
            case 0:
                gameManager.gridSize = 4;

                gameManager.minPieceCount = 6;
                gameManager.maxPieceCount = 7;

                _Difficulty = Difficulty.Easy;
                break;

            case 1:
                gameManager.gridSize = 5;

                gameManager.minPieceCount = 7;
                gameManager.maxPieceCount = 9;

                _Difficulty = Difficulty.Medium;
                break;

            case 2:
                gameManager.gridSize = 6;

                gameManager.minPieceCount = 9;
                gameManager.maxPieceCount = 12;

                _Difficulty = Difficulty.Hard;
                break;
        }
    }

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        Grids = gameManager.gridCells;
        Camera.main.eventMask = InputLayerMask;
    }
}
