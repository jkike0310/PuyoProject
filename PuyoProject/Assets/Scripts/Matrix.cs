
using System;
using UnityEngine;

namespace MyMatrix
{
    public class Matrix : MonoBehaviour
    {
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        private int[,] matrix;
        private Transform[,] matrixTransform;

        private void OnEnable()
        {
            matrix = new int[rows, columns];
            matrixTransform = new Transform[rows, columns];
        }

        public void SetValue(int column, int row, int value, Transform puyo)
        {
            matrix[row, column] = value;
            matrixTransform[row, column] = puyo;
        }

        public  int GetValue(int column, int row)
        {
            return matrix[row, column];
        }

        public Transform GetValueTransform(int column, int row)
        {
            return matrixTransform[row, column];
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

