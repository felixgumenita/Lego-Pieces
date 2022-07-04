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
}
