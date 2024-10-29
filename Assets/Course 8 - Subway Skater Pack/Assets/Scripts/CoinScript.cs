using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    #region Define Variables
    private GameManager manager;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter (Collider collider) 
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                manager.coins+=1;
                Destroy(gameObject);
            break;
            default:
                Destroy(gameObject);
            break;
        }
        
    }
}
