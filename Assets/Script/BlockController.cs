using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    GameManager GameManegy;
    public ParticleSystem CrashParticle;
    public GameObject BlockObject;
    public BoxCollider ColliderArea;
    [Disable] [SerializeField] private int blockHealth = 1;
    [Disable] [SerializeField] private int currentHealth;
    private bool isIndestructible;

    public float BlockWidth
    {
        get
        {
            return BlockObject.transform.localScale.x;
        }
    }

    public float BlockHeight
    {
        get
        {
            return BlockObject.transform.localScale.z;
        }
    }

    /// <summary>
    /// UnityのInspectorで紐づけする
    /// 0 : 通常ブロック
    /// 1 : HP保持ブロック
    /// 2 : 破壊不可ブロック
    /// </summary>
    public Material[] BlockMaterial;

    private void Awake()
    {
        GameManegy = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    /// <summary>
    /// ブロックのHPを設定する。
    /// 0の場合破壊不可ブロックとなる
    /// この関数で設定しない場合ブロックのHPは1となる
    /// </summary>
    /// <param name="hp">ブロックのHP</param>s
    public void SetBlockHealth(int hp)
    {
        blockHealth = hp;
        currentHealth = blockHealth;

        switch (blockHealth)
        {
            case 0:
                BlockObject.GetComponent<Renderer>().material = BlockMaterial[2];
                isIndestructible = true;
                break;
            case 1:
                BlockObject.GetComponent<Renderer>().material = BlockMaterial[0];
                break;
            default:
                BlockObject.GetComponent<Renderer>().material = BlockMaterial[1];
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
                CrashParticle.Play();
                GameManegy.CrashBlock();
                DisableBlock();
            }
        }
    }

    public void ResetBlockHp()
    {
        currentHealth = blockHealth;
    }

    public void DisableBlock()
    {
        BlockObject.SetActive(false);

        //判定処理余裕のため、Colliderの無効は次のフレームで行う。
        StartCoroutine(BlockDisable());
    }

    IEnumerator BlockDisable()
    {
        yield return null;
        ColliderArea.enabled = false;
    }

    public void EnableBlock()
    {
        ColliderArea.enabled = true;
        BlockObject.SetActive(true);
    }
}
