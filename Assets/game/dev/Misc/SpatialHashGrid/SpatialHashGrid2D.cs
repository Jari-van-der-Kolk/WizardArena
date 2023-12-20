using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saxon.HashGrid2D
{
    public class SpatialHashGrid2D<T>
    {
        public float cellSize = 3.0f;

        public Dictionary<Vector2Int, List<T>> grid = new Dictionary<Vector2Int, List<T>>();

        // Function to add an object to the spatial hash grid
        public void AddToGrid(Vector3 objPos ,T obj)
        {
            Vector2Int gridKey = GetGridKey(objPos);

            if (!grid.ContainsKey(gridKey))
            {
                grid[gridKey] = new List<T>();
            }

            grid[gridKey].Add(obj);
        }

        // Function to retrieve nearby objects from the spatial hash grid
        public List<T> GetNearbyObjects(Vector3 position, float radius)
        {
            List<T> nearbyObjects = new List<T>();

            Vector2Int minGridKey = GetGridKey(position - new Vector3(radius, radius, 0));
            Vector2Int maxGridKey = GetGridKey(position + new Vector3(radius, radius, 0));

            for (int x = minGridKey.x; x <= maxGridKey.x; x++)
            {
                for (int y = minGridKey.y; y <= maxGridKey.y; y++)
                {
                    Vector2Int gridKey = new Vector2Int(x, y);

                    if (grid.ContainsKey(gridKey))
                    {
                        nearbyObjects.AddRange(grid[gridKey]);
                    }
                }
            }
            return nearbyObjects;
        }

        // Function to convert world position to grid key
        private Vector2Int GetGridKey(Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x / cellSize);
            int y = Mathf.FloorToInt(position.y / cellSize);

            return new Vector2Int(x, y);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            foreach (var gridKey in grid.Keys)
            {
                Vector3 center = new Vector3(gridKey.x, gridKey.y, 0) + new Vector3(cellSize / 2, cellSize / 2, 0);
                Vector3 size = Vector3.one * cellSize;

                Gizmos.DrawWireCube(center, size);
            }
        }

    }
}


/*if (true)
{
    return;
}
#if UNITY_EDITOR
// Displaying the number of objects in the cell
//string labelText = objectsInCell.Count.ToString();
//GUIStyle style = new GUIStyle();
//style.normal.textColor = Color.white;
//Handles.Label(cellCenter, labelText, style);
#endif*/