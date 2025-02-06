using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    //public Transform spawnLocation;
    public List<Transform> spawnLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.Space)){
            
            int randIndex = Random.Range(0, spawnLocation.Count);
            Vector3 spawnPos = spawnLocation[randIndex].position; 

            Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        }
    }
}
