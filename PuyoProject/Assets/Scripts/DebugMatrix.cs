using System.Collections;
using System.Collections.Generic;
using MyMatrix;
using TMPro;
using UnityEngine;

public class DebugMatrix : MonoBehaviour
{
    private Matrix _matrix;
    [SerializeField] private TextMeshProUGUI debugText;

    
    void Start()
    {
        _matrix = FindObjectOfType<Matrix>();
    }

    // Update is called once per frame
    void Update()
    {
        VisualizeMatrix();
    }

    public void VisualizeMatrix()
    {
        string matrixText = "";
        for (int i= _matrix.GetNumberRows()-1; i >= 0; i--)
        {
            for(int j = 0; j<_matrix.GetNumberColumns(); j++)
            {
                matrixText +=  _matrix.GetValue(j,i).ToString();
            }

            matrixText += "\n";
        }

        debugText.text = matrixText;
    }
}
