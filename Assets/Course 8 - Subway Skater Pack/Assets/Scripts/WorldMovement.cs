using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class WorldMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerController player;
    public GameObject world;
    public bool isRunning;
    public Vector3 spawnPos;
    private float zPos = 50;

    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        InvokeRepeating("SpawnWorld", 0, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOutOfBounds();
    }
    void SpawnWorld()
    {
        if(!isRunning) 
            return;
        Instantiate(world, new Vector3(spawnPos.x, spawnPos.y,spawnPos.z * zPos), transform.rotation);
        zPos += 50;
    }
    void DestroyOutOfBounds()
    {
        if (FindObjectOfType<FloorMove>().transform.position.z < transform.position.z + 50)
        {
        }
    }
}