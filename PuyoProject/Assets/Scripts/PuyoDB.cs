using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Green = 1,
    Blue = 2,
    Red = 3,
    Purple = 4,
}

[Serializable]
[CreateAssetMenu(fileName = "PuyoDB", menuName = "Puyo")]
public class PuyoDB : ScriptableObject
{
    public List<PuyoObject> puyosList = new List<PuyoObject>();
}
[Serializable]
public class PuyoObject
{
    public Type type;
    public GameObject prefab;
}
