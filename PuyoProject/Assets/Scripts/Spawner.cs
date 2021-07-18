using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PuyoDB _puyoDB; 
    [SerializeField] private GameObject puyoPrefab;
    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        var newPuyo = Instantiate(puyoPrefab, spawnPosition.position, spawnPosition.rotation);
        for (int i = 0; i < 2; i++)
        {
            var index = Random.Range(0, _puyoDB.puyosList.Count);
            var puyoElement = Instantiate(_puyoDB.puyosList[index].prefab, new Vector3(newPuyo.transform.position.x + i, newPuyo.transform.position.y, newPuyo.transform.position.z), new Quaternion(0,0,180,0), newPuyo.transform);
            newPuyo.GetComponent<Puyo>().puyoList.Add(puyoElement);
        }
    }
}
