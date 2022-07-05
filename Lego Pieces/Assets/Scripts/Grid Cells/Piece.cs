using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 offset;

    public string ColorName = "Red";

    [HideInInspector] public Vector2 inCellID;
    [HideInInspector] public List<Vector2> Neighbors;
    [HideInInspector] public List<Vector2> lastSpawnedIDs;

    public GameObject spawnPrefab;

    [HideInInspector] public Vector2 originalPosition;

    public List<bool> childInCell = new List<bool>();

    private void Update()
    {
        if (!isDragging) return;

        transform.position = CursorPosition() - offset;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = CursorPosition() - (Vector2)transform.position;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private Vector2 CursorPosition()
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void FindSnapComponentsInChilds()
    {
        var childs = GetComponentsInChildren<PieceSnap>();

        foreach(PieceSnap p in childs)
        {
            if(!p.isParent) childInCell.Add(p.isSnap);
        }
    }
}
