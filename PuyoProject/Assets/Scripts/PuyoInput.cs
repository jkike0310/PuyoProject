using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoInput : MonoBehaviour
{
    public float vertical { get; private set; }
    public  float horizontal { get; private set; }

    [SerializeField] private KeyCode rotateLeftKey;
    [SerializeField] private KeyCode rotateRightKey;
    
    public event Action OnRotateLeft = delegate {  };
    public event Action OnRotateRight = delegate {  };

    private void Update()
    {
        MovementInput();
        RotationInput();
    }

    private void MovementInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            vertical = -1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            horizontal = -1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            horizontal = 1;
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            horizontal = 0;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            vertical = 0;
    }

    private void RotationInput()
    {
        if (Input.GetKeyDown(rotateLeftKey))
            OnRotateLeft();

        if (Input.GetKeyDown(rotateRightKey))
            OnRotateRight();
    }
}
