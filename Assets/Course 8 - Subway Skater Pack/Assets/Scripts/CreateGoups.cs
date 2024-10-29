using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro.Examples;
using UnityEngine;

public class CreateGoups : MonoBehaviour
{
    public GameObject[] ramps = new GameObject[2];
    public GameObject[] blocks = new GameObject[2];
    public GameObject coins;
    public GameObject jumps;
    public GameObject slides;
    public GameObject scoreMultiplier;
    public bool isRunning;
    public float j = 1;
    public float k = 1;

    void Start () {
        InvokeRepeating("SpawnAtIntervals", 0, 5);
        InvokeRepeating("SpawnCoinsAtIntervals", 0, Random.Range(5, 15));
        InvokeRepeating("SpawnDiamondAtIntervals", 0, 15f);
    }

    // Create Groups
    public void CreateRampAndBlock(Vector3 location) {
        int rnd1 = Random.Range(0,2);
        int rnd2 = Random.Range(0,2);
        Instantiate(blocks[rnd2], location - Vector3.forward * -2f, transform.rotation);
        Instantiate(ramps[rnd1], location, transform.rotation);
    }
    
    public GameObject ReturnSingle(char type) {
        switch (type)
        {
            case 'j':
                return jumps;
            case 's':
                return slides;
            default:
                int rnd2 = Random.Range(0,2);
                return blocks[rnd2];
        }
    }

    public void SpawnAtIntervals () {
        if (!isRunning)
            return;
        int opt = Random.Range(0, 3);
        switch (opt)
        {
            case 0:
                CreateRampGroups(GameObject.FindGameObjectWithTag("Player").transform.position.z + 100 * j, Random.Range(0, 10));
            break;
            case 1:
                Create2Groups(GameObject.FindGameObjectWithTag("Player").transform.position.z + 150 * j, Random.Range(0, 10));
            break;
            case 2:
                CreateSingles(GameObject.FindGameObjectWithTag("Player").transform.position.z + 150 * j, Random.Range(0, 10));
            break;
        }
        
        j+=0.8f;
    }

    public void SpawnCoinsAtIntervals () {
        if (!isRunning)
            return;
        int opt = Random.Range(0, 3);
        CoinSpawner(GameObject.FindGameObjectWithTag("Player").transform.position.z + Random.Range(20, 30) * k, Random.Range(20, 30));
        k+=1f;
    }

    public void SpawnDiamondAtIntervals()
    {
        if (!isRunning)
            return;
        ScoreMultiplier(GameObject.FindGameObjectWithTag("Player").transform.position.z + Random.Range(20, 30) * k);
    }
    public void CreateRampGroups(float zAxisDistance, int noOfSpawns) {
        Vector3 [] locations = new Vector3 [3] { 
            Vector3.forward * zAxisDistance, 
            Vector3.forward * zAxisDistance - (PlayerController.LANE_DISTANCE * Vector3.left), 
            Vector3.forward * zAxisDistance + (PlayerController.LANE_DISTANCE * Vector3.left)
        };
        List <int> indices = new List<int> {0, 1, 2};
        ShuffleList<int> (indices);
        CreateRampAndBlock(locations[indices[0]]);
        CreateRampAndBlock(locations[indices[1]]);
        Instantiate(ReturnSingle('b'), locations[indices[2]], transform.rotation);
        if (noOfSpawns > 0) {
            noOfSpawns--;
            CreateBlockGroups(zAxisDistance + 7, noOfSpawns, indices[2]);  
        } else {
            return;
        }
    }

    public void Create2Groups(float zAxisDistance, int noOfSpawns) {
        Vector3 [] locations = new Vector3 [3] { 
            Vector3.forward * zAxisDistance, 
            Vector3.forward * zAxisDistance - (PlayerController.LANE_DISTANCE * Vector3.left), 
            Vector3.forward * zAxisDistance + (PlayerController.LANE_DISTANCE * Vector3.left)
        };
        List <int> indices = new List<int> {0, 1, 2};
        ShuffleList<int> (indices);
        int options = Random.Range(0, 4);
        if (options == 0) {
            Instantiate(ReturnSingle('j'), locations[indices[0]], transform.rotation);
            CreateRampAndBlock(locations[indices[1]]);
        } else if (options == 1) {
            Instantiate(ReturnSingle('b'), locations[indices[0]], transform.rotation);
            Instantiate(ReturnSingle('s'), locations[indices[1]], transform.rotation);
        } else if (options == 2) {
            Instantiate(ReturnSingle('j'), locations[indices[0]], transform.rotation);
            Instantiate(ReturnSingle('s'), locations[indices[1]], transform.rotation);
        } else if (options == 3) {
            Instantiate(ReturnSingle('s'), locations[indices[0]], transform.rotation);
            CreateRampAndBlock(locations[indices[1]]);
        } else {
            Instantiate(ReturnSingle('b'), locations[indices[0]], transform.rotation);
            CreateRampAndBlock(locations[indices[1]]);
        }
        if (noOfSpawns > 0) {
            noOfSpawns--;
            Create2Groups(zAxisDistance + 30, noOfSpawns);  
        } else {
            return;
        }
    }

    public void CreateSingles(float zAxisDistance, int noOfSpawns) {
        Vector3 [] locations = new Vector3 [3] { 
            Vector3.forward * zAxisDistance, 
            Vector3.forward * zAxisDistance - (PlayerController.LANE_DISTANCE * Vector3.left), 
            Vector3.forward * zAxisDistance + (PlayerController.LANE_DISTANCE * Vector3.left)
        };
        List <int> indices = new List<int> {0, 1, 2};
        ShuffleList<int> (indices);
        int options = Random.Range(0, 3);
        if (options == 0) {
            Instantiate(ReturnSingle('b'), locations[indices[0]], transform.rotation);
        } else if (options == 1) {
            Instantiate(ReturnSingle('s'), locations[indices[1]], transform.rotation);
        } else {
            CreateRampAndBlock(locations[indices[1]]);
        }
        if (noOfSpawns > 0) {
            noOfSpawns--;
            CreateSingles(zAxisDistance + 30, noOfSpawns);  
        } else {
            return;
        }
    }

    public void CreateBlockGroups(float zAxisDistance, int noOfSpawns, int frontLittle) {
        Vector3 [] locations = new Vector3 [3] { 
            Vector3.forward * zAxisDistance, 
            Vector3.forward * zAxisDistance - (PlayerController.LANE_DISTANCE * Vector3.left), 
            Vector3.forward * zAxisDistance + (PlayerController.LANE_DISTANCE * Vector3.left)
        };
        List <int> indices = new List<int> {0, 1, 2};
        ShuffleList<int> (indices);
        Instantiate(ReturnSingle('b'), locations[indices[0]], transform.rotation);
        Instantiate(ReturnSingle('b'), locations[indices[1]], transform.rotation);
        Instantiate(ReturnSingle('b'), locations[indices[2]], transform.rotation);
        if (noOfSpawns > 0) {
            noOfSpawns--;
            CreateBlockGroups(zAxisDistance + 10, noOfSpawns, frontLittle);  
        } else {
            return;
        }
    }

    // Method 3: Unity-specific version
    public void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public void CoinSpawner(float  zAxisDistance, float noOfSpawns)
    {
        Vector3[] location = new Vector3[] { 
            Vector3.forward * zAxisDistance, 
            Vector3.forward * zAxisDistance - (PlayerController.LANE_DISTANCE * Vector3.left), 
            Vector3.forward * zAxisDistance + (PlayerController.LANE_DISTANCE * Vector3.left)
        };
        List <int> indices = new List<int> {0, 1, 2};
        ShuffleList<int> (indices);
        int opt = Random.Range(0, 3);
        switch (opt)
        {
            case 0:
                Instantiate(coins, location[indices[0]], transform.rotation);
                Instantiate(coins, location[indices[1]], transform.rotation);
                Instantiate(coins, location[indices[2]], transform.rotation);
            break;
            case 1: 
                Instantiate(coins, location[indices[0]], transform.rotation);
            break;
            default:
                Instantiate(coins, location[indices[0]], transform.rotation);
                Instantiate(coins, location[indices[1]], transform.rotation);
            break;
        }
        
        if (noOfSpawns > 0) {
            noOfSpawns--;
            CoinSpawner(zAxisDistance+1, noOfSpawns);  
        } else {
            return;
        }
        
        Debug.Log("coin spawned");
    }
    public void ScoreMultiplier(float zAxisDistance)
    {
        Vector3[] location = new Vector3[] { 
        Vector3.forward * zAxisDistance, 
        Vector3.forward * zAxisDistance - (PlayerController.LANE_DISTANCE * Vector3.left), 
        Vector3.forward * zAxisDistance + (PlayerController.LANE_DISTANCE * Vector3.left)
        };
        Vector3 pos = location[Random.Range(0,3)];
        Instantiate(scoreMultiplier, new Vector3(pos.x, pos.y, pos.z), scoreMultiplier.transform.rotation);
        Debug.Log("Diamond Spawned");
    }
    
}
