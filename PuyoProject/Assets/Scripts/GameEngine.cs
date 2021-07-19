using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyMatrix;
using Unity.VisualScripting;

public class GameEngine : MonoBehaviour
{
    private Matrix _matrix;
    private Spawner _spawner;
    private void Awake()
    {
        _spawner = FindObjectOfType<Spawner>();
         _matrix = FindObjectOfType<Matrix>();
    }

    public Spawner GetSpawner()
    {
        return _spawner;
    }

    public Matrix GetMatrix()
    {
        return _matrix;
    }

    public void SetPuyo(Puyo puyo)
    {
        puyo.OnClearOldPosition.AddListener(ClearPosition);
        puyo.OnSetNewPosition.AddListener(SetNewPosition);
    }
    //Validate borders
    
    //Validate 4 puyos
    
    //ComboPuyo
    private void ClearPosition(Vector3 actualPosition)
    {
        _matrix.SetValue( Mathf.RoundToInt(actualPosition.x), Mathf.RoundToInt(actualPosition.y), 0, null);
    }

    private void SetNewPosition(Vector3 newPosition, int type, Transform puyo)
    {
        _matrix.SetValue(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), type, puyo);
    }

    public bool VerifySpaceAvailable(Vector3 positionRequest)
    {
        if (_matrix.GetValue(Mathf.RoundToInt(positionRequest.x), Mathf.RoundToInt(positionRequest.y)) == 0)
        {
            return true;
        }
        else
            return false;
    }

    public bool CheckBorders(Vector3 positionRequest)
    {
        Vector3 limits = new Vector2(GetMatrix().GetNumberColumns() - 1, GetMatrix().GetNumberRows() - 1);
        if (positionRequest.x >= 0 && positionRequest.y>=0 && positionRequest.x <= limits.x && positionRequest.y<=limits.y)
            return true;
        else
            return false;
    }
    
    public void FloodFill(Vector2 position, int type)
    {
        
        Queue<Vector2> cells = new Queue<Vector2>();
        cells.Enqueue(position);
        int count = 0;
        while (cells.Count > 0)
        {
            Vector2 a = cells.First();
            cells.Dequeue();
            if (a.x <= GetMatrix().GetNumberColumns()-1 && a.x >= 0 &&
                a.y <= GetMatrix().GetNumberRows()-1 && a.y >= 0)
            {
                if (GetMatrix().GetValue(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y)) == type)
                {
                    count++;
                    if(a.x-1>=0 && a.x-1<= GetMatrix().GetNumberColumns()-1)
                        cells.Enqueue(new Vector2(a.x - 1, a.y));
                    if(a.x+1>=0 && a.x+1 <=GetMatrix().GetNumberColumns()-1)
                        cells.Enqueue(new Vector2(a.x + 1, a.y));
                    if(a.y-1>=0 && a.y-1 <= GetMatrix().GetNumberRows()-1)
                        cells.Enqueue(new Vector2(a.x, a.y - 1));
                    if(a.y+1>=0 && a.y+1 <= GetMatrix().GetNumberRows()-1)
                        cells.Enqueue(new Vector2(a.x, a.y + 1));
                }
            }
            
        }
        if (count >= 4)
        {
            Debug.Log("Hay 4 colores juntos");
        }
        //refresh our main picture box
        return;        
    }
    
    public bool FloodFill(){
        List<Transform> groupToDelete = new List<Transform>();

        for(int row = 0; row < GetMatrix().GetNumberRows(); row++){
            for(int col = 0; col < GetMatrix().GetNumberColumns(); col++ ){
                List<Transform> currentGroup = new List<Transform>();

                if(GetMatrix().GetValue(col, row) != 0){
                    Transform currentType = GetMatrix().GetValueTransform(col, row);
                    int index = (int)currentType.GetComponent<PuyoElement>().type;
                    if(groupToDelete.IndexOf(currentType) == -1){
                        AddNeighbors(currentType, currentGroup);
                    }
                }

                Debug.Log(currentGroup.Count);
                
                if(currentGroup.Count >= 4){
                    foreach(Transform puyo in currentGroup){
                        groupToDelete.Add(puyo);
                    }
                }
            }
        }
        
        if(groupToDelete.Count != 0)
        {
            foreach (var puyo in groupToDelete)
            {
                puyo.GetComponentInParent<Puyo>().DisableMovement();
                puyo.GetComponentInParent<Puyo>().puyoList.Remove(puyo.GetComponent<PuyoElement>());
                Destroy(puyo.gameObject);
            }

            var puyos = FindObjectsOfType<PuyoElement>();
            foreach (var puyo in puyos)
            {
                puyo.Fall();
            }

            return true;
        } else {
            return false;
        }
    }

    
    private void AddNeighbors(Transform currentUnit, List<Transform> currentGroup ){
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
        if(currentGroup.IndexOf(currentUnit) == -1){
            currentGroup.Add(currentUnit);
        } else { 
            return;
        }

        foreach(Vector3 direction in directions){
            int nextX = (int)(Mathf.Round(currentUnit.position.x) + Mathf.Round(direction.x));
            int nextY = (int)(Mathf.Round(currentUnit.position.y) + Mathf.Round(direction.y));
            
            if (nextX >= 0 && nextX <= GetMatrix().GetNumberColumns() - 1 && nextY >= 0 &&
                nextY <= GetMatrix().GetNumberRows() - 1)
            {
                if(GetMatrix().GetValue(Mathf.RoundToInt(nextX), Mathf.RoundToInt(nextY))!=0 && GetMatrix().GetValue(nextX, nextY) ==  (int)currentUnit.GetComponent<PuyoElement>().type)
                {
                    Transform nextUnit = GetMatrix().GetValueTransform(nextX, nextY);
                    AddNeighbors(nextUnit, currentGroup);
                }    
            }
            
        }
    }
    
}
