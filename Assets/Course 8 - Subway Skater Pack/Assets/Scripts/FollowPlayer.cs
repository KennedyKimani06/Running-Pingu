using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 offset;
    private PlayerController player;
    void Start()
    {
        offset = new Vector3 (0f,3f,-4f);
        player = FindObjectOfType<PlayerController>();;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (FindObjectOfType<GameManager>().isGameStarted) {
            transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
        }
    }
}
