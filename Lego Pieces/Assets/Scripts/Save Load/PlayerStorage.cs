using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStorage
{
    public Vector2 CellSize;
    public int Difficulty;
    public string str_Difficulty;

    public PlayerStorage(Vector2 cellSize, int difficulty, string strDifficulty)
    {
        CellSize = cellSize;
        Difficulty = difficulty;
        str_Difficulty = strDifficulty;
    }
}
