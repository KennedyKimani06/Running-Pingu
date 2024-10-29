using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    public GameObject [] Obstacle;
    GameObject[] worldItem;
    public GameObject[] ramp;
    private int DistanceToDestroy = 10;
    private CoinScript [] coin;
    public DiamondScript diamond;

    void Start()
    {
        player = GameObject.Find("Player");
        diamond = FindObjectOfType<DiamondScript>();

    }

    // Update is called once per frame
    void Update()
    {
        worldItem = GameObject.FindGameObjectsWithTag("Respawn");
        coin = GameObject.FindObjectsOfType<CoinScript>();
        ramp = GameObject.FindGameObjectsWithTag("Ramp");
        Obstacle = GameObject.FindGameObjectsWithTag("Obstacle");
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
        for (int i = 0; i < Math.Max(Math.Max(worldItem.Length, coin.Length) , Math.Max(ramp.Length, coin.Length)); i ++)
        {
          try {
            if (worldItem[i].transform.position.z < FindObjectOfType<PlayerController>().transform.position.z - DistanceToDestroy)
            {
                Destroy(worldItem[i]);
            }
            if (Obstacle[i].transform.position.z < FindObjectOfType<PlayerController>().transform.position.z - DistanceToDestroy)
            {
                Destroy(Obstacle[i]);
            }
            if (ramp[i].transform.position.z < FindObjectOfType<PlayerController>().transform.position.z - DistanceToDestroy)
            {
                Destroy(ramp[i]);
            }
            if (coin[i].transform.position.z < FindObjectOfType<PlayerController>().transform.position.z - DistanceToDestroy)
            {
                Destroy(ramp[i]);
            }
             if (diamond.transform.position.z < FindObjectOfType<PlayerController>().transform.position.z - DistanceToDestroy)
            {
                Destroy(diamond);
            }
          } catch (Exception) {
            return;
          }
        }
    }
}
