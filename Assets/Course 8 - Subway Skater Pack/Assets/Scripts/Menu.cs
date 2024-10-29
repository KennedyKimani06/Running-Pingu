using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Show", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Show() {
        Canvas.SetActive(true);
    }
    public void StartGameFromMenu() {
        SceneManager.LoadScene(1);
    }
}
