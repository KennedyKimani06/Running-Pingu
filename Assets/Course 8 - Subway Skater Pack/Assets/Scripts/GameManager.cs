using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    #region 
    public TextMeshProUGUI ScoreTxt, CoinsTxt, ModifierTxt;
    public float score, coins, addscore;
    public bool isGameStarted;
    private PlayerController player;

    #endregion
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        addscore = 1;
        UpdateScores();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted && Input.GetMouseButton(0))
        {
            isGameStarted = true;
            FindObjectOfType<WorldMovement>().isRunning = true;
            FindObjectOfType<CreateGoups>().isRunning = true;
            player.anim.SetTrigger("Run");
        }
        if (isGameStarted) {
            UpdateScores();
            score += player.i*0.1f;
            score += addscore;
        }
    }

    void UpdateScores () {
        ScoreTxt.text = Convert.ToString(Math.Floor(score));
        CoinsTxt.text = Convert.ToString(coins);
        ModifierTxt.text = "X" + Convert.ToString(addscore);
    }
}
