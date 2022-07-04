using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosManager : MonoBehaviour
{
    [SerializeField] Color gizmosColor;

    private void OnDrawGizmos()
    {
        var _collider = GetComponent<BoxCollider2D>();
        var _colPos = (Vector2)_collider.transform.position + _collider.offset;
        var _colSize = _collider.size;

        Gizmos.color = gizmosColor;

        Gizmos.DrawCube(_colPos, _colSize);
    }
}
