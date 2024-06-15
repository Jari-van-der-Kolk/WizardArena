using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Saxon.NodePositioning;

namespace Saxon.HashGrid
{
    public class SpatialHashGrid<T> where T : HashNode
    {
        public Dictionary<Vector3Int, List<T>> grid;
        private float cellSize;

        #region debug

        [SerializeField] private bool debug;

        public void Debug()
        {
            Gizmos.color = Color.red;

            foreach (var key in grid.Keys)
            {
                Vector3 cellCenter = GetCellCenter(key);

                Vector3 size = new Vector3(cellSize, cellSize, cellSize);

                // Draw wire cube using Gizmos
                Gizmos.DrawWireCube(cellCenter, size);

                string labelText = grid[key].Count.ToString(); // Assuming objectsInCell is a Dictionary with key as Vector3
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                Handles.Label(cellCenter, labelText, style);
            }
        }

        #endregion


        public SpatialHashGrid(float cellSize) 
        {
            this.cellSize = cellSize;
            grid = new Dictionary<Vector3Int, List<T>>();
        }

        public Vector3Int GetGridKey(Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x / cellSize);
            int y = Mathf.FloorToInt(position.y / cellSize);
            int z = Mathf.FloorToInt(position.z / cellSize);

            return new Vector3Int(x, y, z);
        }

        public void Add(T item, Vector3 position)
        {
            Vector3Int key = GetGridKey(position);

            if (!grid.TryGetValue(key, out List<T> items))
            {
                items = new List<T>();
                grid[key] = items;
            }

            items.Add(item);
        }

        public void Remove(T item, Vector3Int id)
        {
            if (grid.TryGetValue(id, out List<T> items))
            {
                items.Remove(item);
                if (items.Count == 0)
                {
                    grid.Remove(id);
                }
            }
        }
        public void Clear()
        {
            grid.Clear();
        }

        public Vector3 GetCellCenter(Vector3 key) => new Vector3
       (
           key.x * cellSize + cellSize / 2f,
           key.y * cellSize + cellSize / 2f,
           key.z * cellSize + cellSize / 2f
       );

        public List<T> GetNodesFromOrigin(Vector3Int origin)
        {
            Vector3Int key = GetGridKey(origin);

            if (grid.TryGetValue(key, out List<T> items))
            {
                return items;
            }

            return null;
        }

        public List<T> GetNodesInRadius(Vector3 position, float radius)
        {
            List<T> result = new List<T>();

            Vector3Int minKey = GetGridKey(position - new Vector3(radius, radius, radius));
            Vector3Int maxKey = GetGridKey(position + new Vector3(radius, radius, radius));

            for (int x = minKey.x; x <= maxKey.x; x++)
            {
                for (int y = minKey.y; y <= maxKey.y; y++)
                {
                    for (int z = minKey.z; z <= maxKey.z; z++)
                    {
                        Vector3Int currentKey = new Vector3Int(x, y, z);

                        if (grid.TryGetValue(currentKey, out List<T> items))
                        {
                            result.AddRange(items);
                        }
                    }
                }
            }

            return result;
        }

        public List<Vector3Int> GetOriginNodesInRadius(Vector3 position, float radius)
        {
            List<Vector3Int> keys = new List<Vector3Int>();

            Vector3Int minKey = GetGridKey(position - new Vector3(radius, radius, radius));
            Vector3Int maxKey = GetGridKey(position + new Vector3(radius, radius, radius));

            for (int x = minKey.x; x <= maxKey.x; x++)
            {
                for (int y = minKey.y; y <= maxKey.y; y++)
                {
                    for (int z = minKey.z; z <= maxKey.z; z++)
                    {
                        Vector3Int currentKey = new Vector3Int(x, y, z);
                        keys.Add(currentKey);
                    }
                }
            }

            return keys;
        }



       
    }
}



/*public void AddValues(List<T> values)
{
    for (int i = 0; i < values.Count; i++)
    {
        Add(values[i], values[i].transform.position);
    }
}*/