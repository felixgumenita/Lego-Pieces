using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsManager : MonoBehaviour
{
    #region Managers
    private GameManager gameManager;
    #endregion

    public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 };

    public Difficulty _Difficulty = Difficulty.Easy;

    public Vector2 CurrentCellSize = new Vector2(0, 0);

    public List<GridCells> Grids = new List<GridCells>();
    public void SetDifficulty(int value)
    {
        gameManager = FindObjectOfType<GameManager>(); 

        switch (value)
        {
            case 0:
                gameManager.gridSize = 4;
                _Difficulty = Difficulty.Easy;
                break;

            case 1:
                gameManager.gridSize = 5;
                _Difficulty = Difficulty.Medium;
                break;

            case 2:
                gameManager.gridSize = 6;
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
    }
}
