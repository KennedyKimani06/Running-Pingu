using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += Vector3.up * 1.5f;
        transform.Rotate(Vector3.left * 90);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}