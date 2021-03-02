using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5.0f;
    public float speedoffset = 0;
    public float gameOverLine = -6;
    private Rigidbody myRigid;
    private Vector3 StartPos;

    private GameManager gameMan;

    private void Awake()
    {
        myRigid = this.GetComponent<Rigidbody>();
        StartPos = this.transform.position;
        gameMan = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitBall();
        //ForceBallPower();
    }

    // Update is called once per frame
    void Update()
    {
        if( this.transform.position.z <= gameOverLine)
        {
            gameMan.GameEnd();
        }
    }

    public void ForceBallPower()
    {
        myRigid.AddForce((transform.forward + transform.right) * speed, ForceMode.VelocityChange);
    }

    public void InitBall()
    {
        StopBall();
        speed = 5.0f;
        speedoffset = 0;
        this.transform.position = StartPos;
    }

    public void StopBall()
    {
        myRigid.velocity = Vector3.zero;
    }
}
