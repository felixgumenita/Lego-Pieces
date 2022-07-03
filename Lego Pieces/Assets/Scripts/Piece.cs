using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 offset;
    bool inCell = false;

    public Vector2 inCellID;

    public List<Vector2> Neighbors;

    public List<Vector2> lastSpawnedIDs;

    public GameObject spawnPrefab;

    [SerializeField] private Vector2 originalPosition;

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

        if (!inCell)
        {
            //transform.position = originalPosition;
            return;
        }
    }

    private Vector2 CursorPosition()
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
