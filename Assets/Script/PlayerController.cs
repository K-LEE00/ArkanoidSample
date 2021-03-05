using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject GameFloor;
    public BallController Ball;
    public GameManager GameManegy;

    private float floorSize;
    private float playerSize;
    private Vector3 playerStartPos;

    private void Awake()
    {
        floorSize = GameFloor.transform.localScale.x / 2;
        playerSize = this.transform.localScale.x / 2;
        playerStartPos = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManegy.nowStatus == GameStatus.Title)
        {
            return;
        }

        if (GameManegy.nowStatus == GameStatus.End)
        {
            if (Input.GetKey(KeyCode.R))
            {
                GameManegy.InitGame();
            }
            if (Input.GetKey(KeyCode.T))
            {
                GameManegy.ReturnTitle();
            }
            return;
        }

        float inputhval = Input.GetAxis("Horizontal");

        if( !GameManegy.isGameState && !GameManegy.isGameFinish && inputhval != 0)
        {
            GameManegy.GameStart();
            Ball.ForceBallPower();
        }

        if((inputhval > 0.1) && (this.transform.position.x < (floorSize- playerSize)))
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }
        
        if((inputhval < -0.1) && (this.transform.position.x > (floorSize - playerSize)*-1))
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    public void InitPlayer()
    {
        this.transform.position = playerStartPos;
    }
}