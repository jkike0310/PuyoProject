using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoElement : MonoBehaviour
{
    public Type type;
    private GameEngine gameEngine;
    private Puyo _puyo;

    private void Start()
    {
        _puyo = GetComponentInParent<Puyo>();
        gameEngine = FindObjectOfType<GameEngine>();
    }

    public void Fall()
    {
        for (int i = (int)transform.position.y; i>=0; i--)
        {
            if(gameEngine.VerifySpaceAvailable(new Vector3(transform.position.x, i,transform.position.z)))
            {
                _puyo.OnClearOldPosition.Invoke(transform.position);
                transform.position = new Vector3(transform.position.x, i, transform.position.z);
                _puyo.OnSetNewPosition.Invoke(transform.position, (int)type, transform);
            }
        }
        gameEngine.FloodFill();
    }
}
