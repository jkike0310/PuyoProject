using UnityEngine;
using MyMatrix;

public class GameEngine : MonoBehaviour
{
    private Matrix _matrix;
    private void Awake()
    {
         _matrix = FindObjectOfType<Matrix>();
    }

    public Matrix GetMatrix()
    {
        return _matrix;
    }

    public void SetPuyo(Puyo puyo)
    {
        puyo.OnClearOldPosition += ClearPosition;
        puyo.OnSetNewPosition += SetNewPosition;
    }
    //Validate borders
    
    //Validate 4 puyos
    
    //ComboPuyo
    
    //RefreshMatrix

    private void RefreshMatrix(Vector3 actualPosition, Vector3 newPosition, int type)
    {
        _matrix.SetValue( (int)actualPosition.x, (int)actualPosition.y, 0);
        _matrix.SetValue((int)newPosition.x, (int)newPosition.y, type);
    }
    private void ClearPosition(Vector3 actualPosition)
    {
        _matrix.SetValue( Mathf.RoundToInt(actualPosition.x), Mathf.RoundToInt(actualPosition.y), 0);
    }

    private void SetNewPosition(Vector3 newPosition, int type)
    {
        _matrix.SetValue(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), type);
    }


}
