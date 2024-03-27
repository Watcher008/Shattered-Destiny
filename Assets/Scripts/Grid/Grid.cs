using System;
using UnityEngine;

namespace SD.Grids
{
    public class Grid<TGridObject>
    {
        //This event is called whenever a grid value changes
        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        private int width;
        private int height;
        private float cellSize;

        private Vector3 originPosition;
        private TGridObject[,] gridArray;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createdGribObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createdGribObject(this, x, y);
                }
            }
        }

        public void TriggerGridObjectChanged(int x, int y)
        {
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(((worldPosition - originPosition).x + cellSize * 0.5f) / cellSize);
            y = Mathf.FloorToInt(((worldPosition - originPosition).y + cellSize * 0.5f) / cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                TriggerGridObjectChanged(x, y);
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            //Invalid, outside of array
            return default(TGridObject);
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetGridObject(x, y);
        }

        public Vector3 GetNodePosition(int x, int y)
        {
            return originPosition + new Vector3(x * cellSize, y * cellSize);
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public float GetCellSize()
        {
            return cellSize;
        }

        public Vector3 GetOrigin()
        {
            return originPosition;
        }
    }
}