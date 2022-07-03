using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawn : MonoBehaviour
{
    public Vector2 CellID;
    public List<Vector2> CellNeighbor;

    void FindPieceNeighbors(Vector2 GridSize)
    {
        var x = GridSize.x;
        var y = GridSize.y;

        var cX = CellID.x;
        var cY = CellID.y;


        if (cX == 0 && cY == 0) //Left bottom corner
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x - 1 == cX && cY == 0) //Right bottom corner
        {
            var n1 = new Vector2(cX - 1, cY);
            var n2 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (cX == 0 && y - 1 == cY) //Left up corner
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x - 1 == cX && y - 1 == cY) //Right up corner
        {
            var n1 = new Vector2(cX - 1, cY);
            var n2 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (cX == 0) //Left edge
        {
            var n1 = new Vector2(cX, cY + 1);
            var n2 = new Vector2(cX, cY - 1);
            var n3 = new Vector2(cX + 1, cY);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (x - 1 == cX) //Right edge
        {
            var n1 = new Vector2(cX, cY + 1);
            var n2 = new Vector2(cX, cY - 1);
            var n3 = new Vector2(cX - 1, cY);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (y - 1 == cY) //Up edge
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX - 1, cY);
            var n3 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (cY == 0) //Down edge
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX - 1, cY);
            var n3 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else //Middle Parts
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX - 1, cY);
            var n3 = new Vector2(cX, cY + 1);
            var n4 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
            CellNeighbor.Add(n4);
        }
    }
}
