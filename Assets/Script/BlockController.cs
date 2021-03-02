using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    GameManager gameMan;

    private void Awake()
    {
        gameMan = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameMan.CrashBlock();
        this.gameObject.SetActive(false);
    }
}
