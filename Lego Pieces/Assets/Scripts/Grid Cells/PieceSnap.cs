using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSnap : MonoBehaviour
{

    [Header("Colors")]
    [SerializeField] Color inCellColor;
    [SerializeField] Color outCellColor;

    public bool isSnap = false;
    public bool isParent = false;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag != "Cell") return;
        isSnap = true;

        if (!isParent) return;
        var piece = GetComponentInParent<Piece>();
        piece.SnapCell = col.GetComponent<Cell>();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        isSnap = false;

        if (!isParent) return;
        var piece = GetComponentInParent<Piece>();
        piece.SnapCell = null;
    }

    private void OnDrawGizmos()
    {
        var _collider = GetComponent<BoxCollider2D>();
        var _colPos = (Vector2)_collider.transform.position + _collider.offset;
        var _colSize = _collider.size;

        if (isSnap) Gizmos.color = inCellColor;
        else Gizmos.color = outCellColor;

        Gizmos.DrawCube(_colPos, _colSize);
    }
}
