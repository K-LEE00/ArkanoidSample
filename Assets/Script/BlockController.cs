using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    GameManager gameMan;
    private int blockHealth = 1;
    private int currentHealth;
    private bool isIndestructible;

    /// <summary>
    /// UnityのInspectorで紐づけする
    /// 0 : 通常ブロック
    /// 1 : HP保持ブロック
    /// 2 : 破壊不可ブロック
    /// </summary>
    public Material[] BlockMaterial;

    private void Awake()
    {
        gameMan = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    /// <summary>
    /// ブロックのHPを設定する。
    /// 0の場合破壊不可ブロックとなる
    /// この関数で設定しない場合ブロックのHPは1となる
    /// </summary>
    /// <param name="hp">ブロックのHP</param>
    public void SetBlockHealth(int hp)
    {
        blockHealth = hp;
        currentHealth = blockHealth;

        switch (blockHealth)
        {
            case 0:
                GetComponent<Renderer>().material = BlockMaterial[2];
                isIndestructible = true;
                break;
            case 1:
                GetComponent<Renderer>().material = BlockMaterial[0];
                break;
            default:
                GetComponent<Renderer>().material = BlockMaterial[1];
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if( !isIndestructible)
        {
            currentHealth--;

            if(currentHealth <= 0)
            {
                //ブロック破壊
                gameMan.CrashBlock();
                this.gameObject.SetActive(false);
            }
        }
    }

    public void ResetBlockHp()
    {
        currentHealth = blockHealth;
    }
}
