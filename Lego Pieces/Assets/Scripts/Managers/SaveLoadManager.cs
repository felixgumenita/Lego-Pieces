using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    #region Managers
    PlayerSettingsManager playerSettings;
    GameManager gameManager;
    #endregion

    [SerializeField] InputField inputField;
    [SerializeField] Text filePath;
    [SerializeField] Text lastSaveFileName;

    private void Awake()
    {
        playerSettings = FindObjectOfType<PlayerSettingsManager>();
        gameManager = FindObjectOfType<GameManager>();
        filePath.text = $"Path: <b>{Application.persistentDataPath}</b>";
    }
    public void SaveData()
    {
        var name = inputField.text;
        var namePlayer = $"{name}{SaveLoad.PlayerDataAdditional}.json";

        if (string.IsNullOrEmpty(name)) return;

        PlayerStorage playerData = new PlayerStorage
            (playerSettings.CurrentCellSize,
            (int)playerSettings._Difficulty,
            playerSettings._Difficulty.ToString());

        SaveLoad.SavePlayerData<PlayerStorage>(playerData, namePlayer);
        SaveLoad.SaveCellData<GridCells>(playerSettings.Grids, $"{name}.json");

        lastSaveFileName.text = $"Last Save: {inputField.text}";

        inputField.text = string.Empty;
    }

    public void LoadData()
    {
        var name = inputField.text;
        var namePlayer = $"{name}{SaveLoad.PlayerDataAdditional}.json";

        if (string.IsNullOrEmpty(name)) return;

        gameManager.ResetGrid();

        var playerData = SaveLoad.LoadPlayerData<PlayerStorage>(namePlayer);

        playerSettings.CurrentCellSize = playerData.CellSize;
        playerSettings._Difficulty = (PlayerSettingsManager.Difficulty)playerData.Difficulty;

        var levelData = SaveLoad.LoadLevelData<GridCells>($"{name}.json");
        playerSettings.Grids = levelData;
        gameManager.gridCells = levelData;

        gameManager.GenerateGridFromLoad();

        var piece = gameManager.Pieces;
        var child = gameManager.childPieces;
        foreach (GridCells g in levelData)
        {
            for(int i = 0; i < piece.Count; i++)
            {
                var ipiece = piece[i].GetComponent<Piece>();
                if (g.Color == ipiece.ColorName && g.IsParent)
                {
                    var obj = Instantiate(piece[i], g.ID, Quaternion.identity);
                    obj.GetComponent<Piece>().originalPosition = g.OriginalPosition;
                    gameManager.parentPieces.Add(obj);
                }
            }

            for (int i = 0; i < child.Count; i++)
            {
                var ichild = child[i].GetComponent<PieceSpawn>();
                if (g.Color == ichild.ColorName && !g.IsParent)
                {
                    var obj = Instantiate(child[i], g.ID, Quaternion.identity);
                    gameManager.spawnedPiecesObj.Add(obj);
                }
            }
        }

        gameManager.CombinePiecesFromLoad();

        inputField.text = string.Empty;
    }
}
