using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public GameManager GameManager;
    public TMP_Text scoreCounter;
    public TMP_Text livesCounter;
    public TMP_Text gameOver;

    void Start(){
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreCounter.text = GameManager.score.ToString("0000");
        livesCounter.text = "Lives: " + GameManager.lives.ToString();
        if (GameManager.isGameOver()){
            gameOver.text = "GAME OVER";
        }
    }
}