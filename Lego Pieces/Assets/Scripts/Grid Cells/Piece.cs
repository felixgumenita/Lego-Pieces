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

    public Cell SnapCell;

    private void Update()
    {
        if (!isDragging) return;

        var pos = CursorPosition() - offset;
        transform.position = Vector2.Lerp(transform.position, pos, 10f);
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = CursorPosition() - (Vector2)transform.position;
        childInCell.Clear();
    }

    private void OnMouseUp()
    {
        isDragging = false;

        FindSnapComponentsInChilds();

        int i = 0;

        foreach(bool snap in childInCell)
        {
            if (snap) i++;
        }

        if (childInCell.Count != i) return;

        var pos = new Vector2(SnapCell.CellID.x, SnapCell.CellID.y);

        transform.position = pos;

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
