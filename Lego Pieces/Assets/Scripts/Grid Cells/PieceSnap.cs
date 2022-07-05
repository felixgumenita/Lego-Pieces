using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSnap : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] Color inCellColor;

    public bool isSnap = false;
    public bool isParent = false;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Cell")) return;

        isSnap = true;

        if (!isParent) return;

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        isSnap = false;
    }
    private void OnDrawGizmos()
    {
        if (!isSnap) return;

        var _collider = GetComponent<BoxCollider2D>();
        var _colPos = (Vector2)_collider.transform.position + _collider.offset;
        var _colSize = _collider.size;

        Gizmos.color = inCellColor;

        Gizmos.DrawCube(_colPos, _colSize);
    }
}
