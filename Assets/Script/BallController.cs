using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class BallController : MonoBehaviour
{
    public float speed;
    public float gameOverLine = -6;
    private Rigidbody myRigid;
    private Vector3 StartPos;

    private GameManager GameManegy;
    private Vector3 lastVelocity;

    private void Awake()
    {
        myRigid = this.GetComponent<Rigidbody>();
        StartPos = this.transform.position;
        GameManegy = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitBall();
    }

    // Update is called once per frame
    void Update()
    {
        if( this.transform.position.z <= gameOverLine && GameManegy.nowStatus == GameStatus.Play)
        {
            GameManegy.GameEnd();
        }
    }

    private void FixedUpdate()
    {
        //反射計算のために座標データを保持する
        lastVelocity = this.myRigid.velocity;
    }

    public void ForceBallPower()
    {
        myRigid.AddForce((transform.forward + transform.right) * speed, ForceMode.VelocityChange);
    }

    public void InitBall()
    {
        StopBall();
        this.transform.position = StartPos;
    }

    public void StopBall()
    {
        myRigid.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GameManegy.nowStatus == GameStatus.Play)
        {
            //反射はVector3 Reflectで制御する
            Vector3 refrectVec = Vector3.Reflect(this.lastVelocity, collision.contacts[0].normal);
            myRigid.velocity = refrectVec;
        }
    }
}
