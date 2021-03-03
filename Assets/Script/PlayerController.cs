using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject GameFloor;
    public BallController Ball;
    public GameManager gameMan;

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
        if (gameMan.isGameFinish)
        {
            if (Input.GetKey(KeyCode.R))
            {
                gameMan.InitGame();
            }
            return;
        }

        float inputhval = Input.GetAxis("Horizontal");

        if( !gameMan.isGameState && !gameMan.isGameFinish && inputhval != 0)
        {
            gameMan.GameStart();
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