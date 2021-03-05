using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using GameData;

public class StageController : MonoBehaviour
{
    //private static string stageDataRoot = "Assets\\Resources\\";
    //File System
    private static string stageDataRoot;    //Awake()でビルド環境に合わせてパス指定
    private static string stageDataDirectory = "StageData";
    private static string stageDataFormat = ".csv";
    private string[] stageFileList;

    //CSV
    public List<StageData> StageFiled = new List<StageData>();
    [Disable]public int StageCount;

    //ブロック情報
    public float StageTopMargen;
    public GameObject BlockData;
    public GameObject StageArea;
    public GameObject BlockGroup;
    private float blockWidth;
    private float blockHeight;
    private float StageHeight;
    private float BlockVectorY = 0.7f;

    //ステージ管理
    public List<GameObject> BlockList = new List<GameObject>();

    private void Awake()
    {
#if UNITY_EDITOR    //エディター
        stageDataRoot = "Assets\\Resources\\";
#else               //その他
        stageDataRoot = "Resources\\";
#endif
        blockWidth = BlockData.GetComponent<BlockController>().BlockWidth;
        blockHeight = BlockData.GetComponent<BlockController>().BlockHeight;
        StageHeight = StageArea.transform.localScale.z;

        bool dataref = ReadStageDataList();
        if (dataref && StageCount>0)
        {
            for(int i=0; i< StageCount; i++)
            {
                StageFiled.Add(new StageData(stageFileList[i]));
            }
        }
    }

    private void Start()
    {
    }

    /// <summary>
    /// "Resources\StageData"からステージデータファイルを取得する。
    /// </summary>
    /// <returns>ture:取得成功、false:取得失敗</returns>
    private bool ReadStageDataList()
    {
        try
        {
            string[] read = Directory.GetFiles((stageDataRoot + stageDataDirectory), ("*" + stageDataFormat), SearchOption.TopDirectoryOnly);


            if (read.Length < 1)
            {
                return false;
            }

            stageFileList = new string[read.Length];
            StageCount = stageFileList.Length;
            for (int i = 0; i < StageCount; i++)
            {
                string tmppath;
                tmppath = read[i].Replace(stageDataRoot, "");
                stageFileList[i] = tmppath.Replace(stageDataFormat, "");
            }
        }
        catch
        {
            //ディレクトリ確認失敗
            StageCount = 0;
        }

        return true;
    }

    /// <summary>
    /// ステージ情報に合わせてブロックの配置を行う
    /// </summary>
    /// <param name="stagenum">ステージ番号</param>
    /// <returns>ステージのブロック総数</returns>
    public int CreateStage(int stagenum)
    {
        Vector2 filedsize = StageFiled[stagenum].GetStageBlockMax();
        float xpstart = 0 - ((filedsize.x / 2)*blockWidth) + (blockWidth/2);
        float ypstart = (StageHeight / 2) - StageTopMargen - (blockHeight / 2);

        int blockcount = 0;

        BlockList.Clear();

        for (int idxy = 0; idxy < filedsize.y; idxy++)
        {
            for (int idxx = 0; idxx < filedsize.x; idxx++)
            {
                int tmp = StageFiled[stagenum].GetStageBlock(idxx, idxy);
                if (tmp != -1)
                {
                    //ブロック配置
                    //Vector3 blockpos = new Vector3(xpstart + (idxx * blockWidth), BlockVectorY, ypstart - (idxy * blockHeight));
                    Vector3 blockpos = new Vector3(xpstart + (idxx * blockWidth), BlockVectorY, ypstart - (idxy * blockHeight));
                    GameObject obj = (GameObject)Instantiate(BlockData, Vector3.zero, Quaternion.identity, BlockGroup.transform);
                    obj.transform.position = blockpos;
                    obj.GetComponent<BlockController>().SetBlockHealth(tmp);

                    BlockList.Add(obj);

                    //破壊対象ブロック集計
                    if (tmp >= 1)
                    {
                        blockcount++;
                    }
                }
            }
        }
        return blockcount;
    }

    /// <summary>
    /// すでに破壊したブロックを復活させる。
    /// この処理を使うためにはCreateStage()でブロック配置が必修となる
    /// </summary>
    public void RetryBlock()
    {
        for (int i = 0; i < BlockList.Count; i++)
        {
            BlockList[i].GetComponent<BlockController>().EnableBlock();
            BlockList[i].GetComponent<BlockController>().ResetBlockHp();
        }
    }

    public void ClearBlock()
    {
        for (int i = 0; i < BlockList.Count; i++)
        {
            if(BlockList[i] != null)
            {
                Destroy(BlockList[i]);
            }
        }
        BlockList.Clear();
    }
}

