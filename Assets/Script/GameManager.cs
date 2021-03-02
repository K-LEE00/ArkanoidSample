using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text GameMsg;
    public string startMsg;
    public string endMsg;
    public int blockCount;
    public BlockAreaController BlockArea;
    public BallController Ball;
    public PlayerController player;

    public bool isGameState;
    public bool isGameFinish;
    [SerializeField]private int crashCount;

    private void Start()
    {
        InitGame();
    }

    private void LateUpdate()
    {
        if( crashCount >= blockCount)
        {
            GameEnd();
        }
    }

    public void CrashBlock()
    {
        crashCount++;
    }

    public void InitGame()
    {
        GameMsg.gameObject.SetActive(true);
        GameMsg.text = startMsg;
        crashCount = 0;
        isGameState = false;
        isGameFinish = false;
        BlockArea.ReloadBlock();
        Ball.InitBall();
        player.InitPlayer();
    }

    public void GameStart()
    {
        if (GameMsg.IsActive())
        {
            GameMsg.gameObject.SetActive(false);
        }
        isGameState = true;
        crashCount = 0;
    }

    public void GameEnd()
    {
        if (!GameMsg.IsActive())
        {
            GameMsg.gameObject.SetActive(true);
        }
        GameMsg.text = endMsg;
        isGameFinish = true;
        Ball.StopBall();
    }
}