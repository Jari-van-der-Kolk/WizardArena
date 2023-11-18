using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    public FitType fitType;

    public int rows, columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    
    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        rows = Mathf.Clamp(rows, 1, Int32.MaxValue);
        columns = Mathf.Clamp(columns, 1, Int32.MaxValue);
        
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }
        if (fitType == FitType.Width || fitType == FitType.FixedRows)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float) columns);
        }
        if (fitType == FitType.Height|| fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float) rows);
        }
        
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float) columns) - ((spacing.x / (float) columns) * 2) - (padding.left / (float) columns) - (padding.right / (float) columns);
        float cellHeight = (parentHeight / (float) rows) - ((spacing.y / (float)rows) * 2) - (padding.top /(float)rows)- (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var posX = (cellSize.x * columnCount) + (spacing.x * columnCount)+ padding.left;
            var posY = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top ;
            
            SetChildAlongAxis(item, 0, posX, cellSize.x);
            SetChildAlongAxis(item, 1, posY, cellSize.y);
        }

    }
    
    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
