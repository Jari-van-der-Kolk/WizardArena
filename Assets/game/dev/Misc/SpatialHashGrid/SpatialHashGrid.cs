﻿using System;
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
                Vector3 cellCenter = new Vector3(
                    key.x * cellSize + cellSize / 2f,
                    key.y * cellSize + cellSize / 2f,
                    key.z * cellSize + cellSize / 2f
                );

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


        public SpatialHashGrid(List<T> values, float cellSize) 
        {
            this.cellSize = cellSize;
            grid = new Dictionary<Vector3Int, List<T>>();
            for (int i = 0; i < values.Count; i++)
            {
                Add(values[i], values[i].transform.position);
            }
        }

        private Vector3Int GetGridKey(Vector3 position)
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

        public List<T> GetItems(Vector3 position)
        {
            Vector3Int key = GetGridKey(position);

            if (grid.TryGetValue(key, out List<T> items))
            {
                return items;
            }

            return null;
        }

        public List<T> GetItemsInRadius(Vector3 position, float radius)
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

        public void Clear()
        {
            grid.Clear();
        }
    }
}
