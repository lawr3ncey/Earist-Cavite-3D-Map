using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public Vector3 gridOrigin;  // Start of the grid
    public float cellSize = 1f;  // Size of each grid cell
    public int gridWidth, gridHeight, gridDepth;  // Dimensions of the grid

    private GridCell[,,] grid;  // 3D array to store grid cells

    void Start()
    {
        InitializeGrid();  // Create the grid on start
    }

    // Initializes the grid with GridCells at proper world positions.
    public void InitializeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight, gridDepth];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    Vector3 worldPos = new(x * cellSize, y * cellSize, z * cellSize);
                    grid[x, y, z] = new GridCell(worldPos, isWalkable: true);  // Assume walkable
                }
            }
        }

        Debug.Log("Grid initialized.");
    }

    public List<GridCell> FindShortestPath(Vector3 fromPos, Vector3 toPos)
    {
        GridCell startCell = GetCellFromWorldPosition(fromPos);
        GridCell goalCell = GetCellFromWorldPosition(toPos);

        if (startCell == null || goalCell == null) return null;

        List<GridCell> openSet = new() { startCell };
        HashSet<GridCell> closedSet = new();

        Dictionary<GridCell, GridCell> cameFrom = new Dictionary<GridCell, GridCell>();
        Dictionary<GridCell, float> gScore = new Dictionary<GridCell, float> { [startCell] = 0 };
        Dictionary<GridCell, float> fScore = new Dictionary<GridCell, float> { [startCell] = Heuristic(startCell, goalCell) };

        while (openSet.Count > 0)
        {
            GridCell current = openSet.OrderBy(cell => fScore[cell]).First();

            if (current == goalCell) return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (GridCell neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || !neighbor.isWalkable) continue;

                float tentativeGScore = gScore[current] + Vector3.Distance(current.position, neighbor.position);

                if (tentativeGScore < gScore.GetValueOrDefault(neighbor, float.MaxValue))
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goalCell);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;  // No path found
    }

    private float Heuristic(GridCell fromPos, GridCell toPos)
    {
        return Vector3.Distance(fromPos.position, toPos.position);
    }

    private List<GridCell> ReconstructPath(Dictionary<GridCell, GridCell> cameFrom, GridCell current)
    {
        List<GridCell> path = new List<GridCell> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    public GridCell GetCellFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);
        int z = Mathf.FloorToInt(worldPosition.z / cellSize);

        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight && z >= 0 && z < gridDepth)
            return grid[x, y, z];

        return null;
    }

    private List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new List<GridCell>();
        Vector3Int[] directions = { new(1, 0, 0), new(-1, 0, 0), new(0, 1, 0), new(0, -1, 0), new(0, 0, 1), new(0, 0, -1) };

        foreach (var dir in directions)
        {
            GridCell neighbor = GetCellFromWorldPosition(cell.position + (Vector3)dir * cellSize);
            if (neighbor != null) neighbors.Add(neighbor);
        }

        return neighbors;
    }
}

public class GridCell
{
    public Vector3 position;
    public bool isWalkable;

    public GridCell(Vector3 worldPos, bool isWalkable)
    {
        WorldPos = worldPos;
        this.isWalkable = isWalkable;
    }

    public Vector3 WorldPos { get; }
}
