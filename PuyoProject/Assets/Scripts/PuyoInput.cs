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
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(rotateLeftKey))
            OnRotateLeft();

        if (Input.GetKeyDown(rotateRightKey))
            OnRotateRight();
    }
}
