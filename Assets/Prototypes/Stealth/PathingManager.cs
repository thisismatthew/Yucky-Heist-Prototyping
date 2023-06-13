using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingCell
{
    public int x, y, f, g;
    public float h;
    public bool blocked = false;
    public PathingCell fromCell;
    public List<PathingCell> neighbours = new List<PathingCell>();
    public PathingCell(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public void AddNeighbours(List<List<PathingCell>> PathingCellsGrid)
    {
        if (x < PathingCellsGrid.Count - 1) 
            neighbours.Add(PathingCellsGrid[x + 1][y]);

        if (y < PathingCellsGrid[x].Count - 1)
            neighbours.Add(PathingCellsGrid[x][y + 1]);

        if (x > 0)
            neighbours.Add(PathingCellsGrid[x - 1][y]);

        if (y > 0) 
            neighbours.Add(PathingCellsGrid[x][y - 1]);

        if (x > 0 && y > 0) 
            neighbours.Add(PathingCellsGrid[x - 1][y - 1]);

        if (x < PathingCellsGrid.Count - 1 && y > 0) 
            neighbours.Add(PathingCellsGrid[x + 1][y - 1]);

        if (x < PathingCellsGrid.Count - 1 && y < PathingCellsGrid[x].Count - 1)
            neighbours.Add(PathingCellsGrid[x + 1][y + 1]);

        if (x > 0 && y < PathingCellsGrid[x].Count - 1)
            neighbours.Add(PathingCellsGrid[x - 1][y + 1]);

    }
}

public class PathingManager : MonoBehaviour
{
   
    
    public Grid grid;
    public Vector3Int TopCorner, BottomCorner;
    private List<List<PathingCell>> pathingCells = new List<List<PathingCell>>();
    public List<Collider2D> PathingBlockColliders;
    
    private void Start()
    {
        if (BottomCorner.x < 0 || BottomCorner.y < 0) 
            Debug.LogError("Uh the Pathing manager Bottom Corner is now Below Zero, I cut this corner in prototyping sorry!");

        grid = GetComponent<Grid>();
        for (int x = BottomCorner.x; x < TopCorner.x; x++)
        {
            pathingCells.Add(new List<PathingCell>());
            for (int y = BottomCorner.y; y < TopCorner.y; y++)
            {
                pathingCells[x].Add(new PathingCell(x,y));
                
            }
        }
        for (int x = BottomCorner.x; x < TopCorner.x; x++)
        {
            for (int y = BottomCorner.y; y < TopCorner.y; y++)
            {
                pathingCells[x][y].AddNeighbours(pathingCells);

            }
        }
        foreach (Collider2D col in PathingBlockColliders) SetCellsBlockedWithCollider(col);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(grid.CellToWorld(BottomCorner), .5f);
        Gizmos.DrawSphere(grid.CellToWorld(TopCorner), .5f);

        Color c = new Color(0, 1, 0, 0.2f);
        Gizmos.color = c;
        for (int x = BottomCorner.x; x < TopCorner.x; x++)
        {
            for (int y = BottomCorner.y; y < TopCorner.y; y++)
            {
                if (Application.isPlaying)
                {
                    if (pathingCells[x][y].blocked) Gizmos.color = Color.red;
                    else Gizmos.color = c;
                }
                Gizmos.DrawSphere(grid.GetCellCenterWorld(new Vector3Int(x, y)), .5f);
            }
        }
        
    }

    public List<Vector2> AStarPath(Vector2 start, Vector2 goal)
    {
        List<Vector2> finalPath = new List<Vector2>();
        var startingCoords = grid.WorldToCell(start);
        var goalCoords = grid.WorldToCell(goal);

        List<PathingCell> closedSet = new List<PathingCell>();

        List<PathingCell> openSet = new List<PathingCell>();

        openSet.Add(pathingCells[startingCoords.x][startingCoords.y]);

        

        while (openSet.Count > 0)
        {
            var winner = 0;
            for (var i =0; i < openSet.Count-1; i++)
            {
                if (openSet[i].f < openSet[winner].f)
                {
                    winner = i;
                }
            }

            var current = openSet[winner];

            if (openSet[winner] == pathingCells[goalCoords.x][goalCoords.y])
            {
                Debug.Log("DONE");
                var temp = current;
                while (temp.fromCell != null)
                {
                    finalPath.Add(grid.GetCellCenterWorld(new Vector3Int(temp.fromCell.x, temp.fromCell.y)));
                    temp = temp.fromCell;
                }
                finalPath.Reverse();
                return finalPath;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            var neighbors = current.neighbours;

            for (int n = 0; n<neighbors.Count-1; n++)
            {
                var neighbor = neighbors[n];

                if (!closedSet.Contains(neighbor) && !neighbor.blocked)
                {
                    var tentative_gScore = current.g + 1;

                    if (openSet.Contains(neighbor))
                    {
                        if (tentative_gScore < neighbor.g)
                            neighbor.g = tentative_gScore;
                    }
                    else
                    {
                        neighbor.g = tentative_gScore;
                        openSet.Add(neighbor);
                    }

                    var currentCoord = grid.CellToWorld(new Vector3Int(neighbor.x, neighbor.y));
                    neighbor.h = Vector2.Distance(currentCoord, goal);
                    neighbor.f = (int)(neighbor.g + neighbor.h);
                    neighbor.fromCell = current;

                }


            }

        }

        return null;
    }
    
    public void SetCellsBlockedWithCollider(Collider2D col)
    {
        for (int x = BottomCorner.x; x < TopCorner.x; x++)
        {
            for (int y = BottomCorner.y; y < TopCorner.y; y++)
            {
               if (col.OverlapPoint(grid.GetCellCenterWorld(new Vector3Int(x, y))))
                {
                    pathingCells[x][y].blocked = true;
                }
            }
        }
    }

}
