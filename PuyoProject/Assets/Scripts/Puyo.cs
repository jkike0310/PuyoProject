using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Puyo : MonoBehaviour
{
    private PuyoInput _puyoInput;
    private int type = 1;
    [SerializeField] private float gravityDelay;
    private const float movementDelay = 0.05f;
    private float count;
    private float countMov;
    public List<GameObject> puyoList = new List<GameObject>();
    private bool canMove { get; set; }
    public event Action<Vector3> OnClearOldPosition = delegate {  };
    public event Action<Vector3, int> OnSetNewPosition = delegate {  };
    private GameEngine _gameEngine;

    private void Awake()
    {
        _gameEngine = FindObjectOfType<GameEngine>();
        _puyoInput = FindObjectOfType<PuyoInput>();
        _puyoInput.OnRotateLeft += RotateLeft;
        _puyoInput.OnRotateRight += RotateRight;
        _gameEngine.SetPuyo(this);
        canMove = true;
    }

    void Update()
    {
        Gravity();
        if (canMove)
        {
            Movement();
        }
    }

    private void Movement()
    {
       
        if (!(_puyoInput.vertical <= 0) ) return;
        if (countMov > movementDelay)
        {
            var movementAmount = new Vector3(Mathf.Round(_puyoInput.horizontal), Mathf.Round(_puyoInput.vertical), 0);
            if( puyoList[0].transform.position.x + movementAmount.x <0 || puyoList[1].transform.position.x + movementAmount.x<0
                || puyoList[0].transform.position.x + movementAmount.x> _gameEngine.GetMatrix().GetNumberColumns()-1 || puyoList[1].transform.position.x + movementAmount.x > _gameEngine.GetMatrix().GetNumberColumns()-1
                || puyoList[0].transform.position.y + movementAmount.y <0 || puyoList[1].transform.position.y + movementAmount.y<0)
                return;
            OnClearOldPosition(puyoList[0].transform.position);
            OnClearOldPosition(puyoList[1].transform.position);
            transform.position += movementAmount;
            OnSetNewPosition(puyoList[0].transform.position, type);
            OnSetNewPosition(puyoList[1].transform.position, type);
            Debug.Log(puyoList[1].transform.position.x + "    " + puyoList[1].transform.position.y);
            countMov = 0;
        }
        else
        {
            countMov += Time.deltaTime;
        }
        
    }

    private void Gravity()
    {
        if (count > gravityDelay)
        {
            if(puyoList[0].transform.position.y + Vector3.down.y<0 || puyoList[1].transform.position.y + Vector3.down.y<0)
                return;
            OnClearOldPosition(puyoList[0].transform.position);
            OnClearOldPosition(puyoList[1].transform.position);
            transform.position += Vector3.down;
            OnSetNewPosition(puyoList[0].transform.position, type);
            OnSetNewPosition(puyoList[1].transform.position, type);
            count = 0;
        }
        else
        {
            count += Time.deltaTime;
        }
    }

    private void RotateLeft()
    {
        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+90);
        SmoothRotation(transform.rotation, target, 0.1f, true);
    }

    private void RotateRight()
    {
        Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z-90);
        SmoothRotation(transform.rotation, target, 0.1f, false);
    }

    void SmoothRotation(Quaternion start, Quaternion end, float dur, bool isAdding)
    {
        OnClearOldPosition(puyoList[0].transform.position);
        OnClearOldPosition(puyoList[1].transform.position);
        float t = 0f;
        while(t < dur)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / dur);
            foreach (var puyo in puyoList)
            {
                if (isAdding)
                    puyo.transform.rotation = Quaternion.Slerp(puyo.transform.rotation,
                        new Quaternion(puyo.transform.rotation.x, puyo.transform.rotation.y, puyo.transform.rotation.z - 90, puyo.transform.rotation.w), t / dur);
                else
                    puyo.transform.rotation = Quaternion.Slerp(puyo.transform.rotation,
                        new Quaternion(puyo.transform.rotation.x, puyo.transform.rotation.y, puyo.transform.rotation.z + 90, puyo.transform.rotation.w), t / dur);
            }
           
            t += Time.deltaTime;
        }
        transform.rotation = end;
        OnSetNewPosition(puyoList[0].transform.position, type);
        OnSetNewPosition(puyoList[1].transform.position, type);
        Debug.Log(puyoList[1].transform.position.x + "    " + puyoList[1].transform.position.y);
    }
}
