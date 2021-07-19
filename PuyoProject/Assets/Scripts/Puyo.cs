using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Puyo : MonoBehaviour
{
    private PuyoInput _puyoInput;
    private int type = 1;
    [SerializeField] private float gravityDelay;
    private const float movementDelay = 0.05f;
    private float count;
    private float countMov;
    public List<PuyoElement> puyoList = new List<PuyoElement>();
    private bool canMove { get; set; }
    public UnityEvent<Vector3> OnClearOldPosition;
    public UnityEvent<Vector3, int, Transform> OnSetNewPosition;
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
        
        if (canMove)
        {
            Gravity();
            Movement();
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    private void Movement()
    {
        if (!(_puyoInput.vertical <= 0) || _puyoInput.horizontal == 0 ) return;
        if (countMov > movementDelay)
        {
            var movementAmount = new Vector3(Mathf.RoundToInt(_puyoInput.horizontal), Mathf.RoundToInt(_puyoInput.vertical), 0);
            foreach (var puyo in puyoList)
            {
                if (!_gameEngine.CheckBorders(puyo.transform.position+movementAmount))
                {
                   movementAmount = Vector3.zero;
                   if (!_gameEngine.VerifySpaceAvailable(puyo.transform.position + movementAmount))
                   {
                       DropPuyos();
                       return;
                   }
                }
                else
                {
                    OnClearOldPosition.Invoke(puyo.transform.position);
                }
            }
            transform.position += movementAmount;
            OnSetNewPosition.Invoke(puyoList[0].transform.position, (int)puyoList[0].GetComponent<PuyoElement>().type, puyoList[0].transform);
            OnSetNewPosition.Invoke(puyoList[1].transform.position, (int)puyoList[1].GetComponent<PuyoElement>().type, puyoList[0].transform);
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
            foreach (var puyo in puyoList)
            {
                if (!_gameEngine.CheckBorders(puyo.transform.position + Vector3.down) || !_gameEngine.VerifySpaceAvailable(puyo.transform.position + Vector3.down))
                {
                    DropPuyos();
                    return;
                }
                else
                {
                    OnClearOldPosition.Invoke(puyo.transform.position);
                }
            }
            transform.position += Vector3.down;
            foreach (var puyo in puyoList)
            {
                OnSetNewPosition.Invoke(puyo.transform.position, (int)puyo.type, puyo.transform);
            }
            count = 0;
        }
        else
        {
            count += Time.deltaTime;
        }
    }
    
    private void DropPuyos()
    {
        foreach (var puyo in puyoList)
        {
            puyo.Fall();
        }
        canMove = false;
        _puyoInput.OnRotateLeft -= RotateLeft;
        _puyoInput.OnRotateRight -= RotateRight;
        _gameEngine.GetSpawner().Spawn();
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
        OnClearOldPosition.Invoke(puyoList[0].transform.position);
        OnClearOldPosition.Invoke(puyoList[1].transform.position);
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
        OnSetNewPosition.Invoke(puyoList[0].transform.position, (int)puyoList[0].type, puyoList[0].transform);
        OnSetNewPosition.Invoke(puyoList[1].transform.position, (int)puyoList[1].type, puyoList[1].transform);
    }
}
