
using System;
using UnityEngine;

namespace MyMatrix
{
    public class Matrix : MonoBehaviour
    {
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        private int[,] matrix;

        private void OnEnable()
        {
            matrix = new int[rows, columns];
        }

        public void SetValue(int column, int row, int value)
        {
            matrix[row, column] = value;
        }

        public  int GetValue(int column, int row)
        {
            return matrix[row, column];
        }

        public int GetNumberRows()
        {
            return rows;
        }

        public int GetNumberColumns()
        {
            return columns;
        }
    }
}

