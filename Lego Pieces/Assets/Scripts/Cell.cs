using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 CellID;

    public List<Vector2> CellNeighbor;

    public void FindNeighbors(Vector2 gridSize)
    {
        var x = gridSize.x;
        var y = gridSize.y;

        var cX = CellID.x;
        var cY = CellID.y;

        
        if(x-(x-1) == cX && y-(y-1) == cY) //Left bottom corner
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x == cX && y - (y - 1) == cY) //Right bottom corner
        {
            var n1 = new Vector2(cX - 1, cY);
            var n2 = new Vector2(cX, cY + 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x - (x - 1) == cX && y == cY) //Left up corner
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x == cX && y == cY) //Right up corner
        {
            var n1 = new Vector2(cX - 1, cY);
            var n2 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
        }
        else if (x - (x - 1) == cX) //Left edge
        {
            var n1 = new Vector2(cX, cY + 1);
            var n2 = new Vector2(cX, cY - 1);
            var n3 = new Vector2(cX + 1, cY);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (x == cX) //Right edge
        {
            var n1 = new Vector2(cX, cY + 1);
            var n2 = new Vector2(cX, cY - 1);
            var n3 = new Vector2(cX - 1, cY);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (y == cY) //Up edge
        {
            var n1 = new Vector2(cX + 1, cY);
            var n2 = new Vector2(cX - 1, cY);
            var n3 = new Vector2(cX, cY - 1);

            CellNeighbor.Add(n1);
            CellNeighbor.Add(n2);
            CellNeighbor.Add(n3);
        }
        else if (y - (y - 1) == cY) //Down edge
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
