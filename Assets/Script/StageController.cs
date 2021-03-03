using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Stage;

public class StageController : MonoBehaviour
{
    //File System
    private static string stageDataRoot = "Assets\\Resources\\";
    private static string stageDataDirectory = "StageData";
    private static string stageDataFormat = ".csv";
    private string[] stageFileList;

    //CSV
    public List<StageData> StageFiled;

    //ブロック情報
    public float StageTopMargen;
    public GameObject BlockData;
    public GameObject StageArea;
    public GameObject BlockGroup;
    private float blockWidth;
    private float blockHeight;
    private float StageWidth;
    private float StageHeight;
    private float BlockVectorY = 0.7f;

    //ステージ管理
    public List<GameObject> BlockList = new List<GameObject>();

    private void Awake()
    {
        blockWidth = BlockData.transform.localScale.x;
        blockHeight = BlockData.transform.localScale.z;
        StageWidth = StageArea.transform.localScale.x;
        StageHeight = StageArea.transform.localScale.z;

        ReadStageDataList();
        Debug.Log("File Nun : " + stageFileList.Length);

        //StageFiled = new StageData(stageFileList[0]);
        StageFiled = new List<StageData>();
        StageFiled.Add(new StageData(stageFileList[0]));
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
        string[] read = Directory.GetFiles((stageDataRoot + stageDataDirectory), ("*" + stageDataFormat), SearchOption.TopDirectoryOnly);
        
        if (read.Length < 1)
        {
            return false;
        }

        stageFileList = new string[read.Length];
        for (int i = 0; i < read.Length; i++)
        {
            string tmppath;
            tmppath = read[i].Replace(stageDataRoot, "");
            stageFileList[i] = tmppath.Replace(stageDataFormat, "");
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

        float xpstart = 0 - (blockWidth * 10 / 2) + (blockWidth / 2);
        float ypstart = (StageHeight / 2) - StageTopMargen - (blockHeight / 2);
        int blockcount = 0;

        for (int idxy = 0; idxy < filedsize.y; idxy++)
        {
            for (int idxx = 0; idxx < filedsize.x; idxx++)
            {
                int tmp = StageFiled[stagenum].GetStageBlock(idxx, idxy);
                if (tmp != -1)
                {
                    //ブロック配置
                    Vector3 blockpos = new Vector3( xpstart+(idxx*blockWidth), BlockVectorY, ypstart-(idxy*blockHeight));
                    GameObject obj = (GameObject)Instantiate(BlockData, Vector3.zero, Quaternion.identity, BlockGroup.transform);
                    obj.transform.position = blockpos;
                    obj.GetComponent<BlockController>().SetBlockHealth(tmp);

                    BlockList.Add(obj);

                    //破壊対象ブロック集計
                    if(tmp >= 1) 
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
            BlockList[i].gameObject.SetActive(true);
            BlockList[i].GetComponent<BlockController>().ResetBlockHp();
        }
    }
}


namespace Stage
{
    public class StageData
    {
        static public int horizontalMaxConut = 10;
        static public int verticalMaxConut = 5;


        private TextAsset csvFile; // CSVファイル

        public string stageName;
        public string stageTooltip;
        char[,] filedBlockData = new char[verticalMaxConut, horizontalMaxConut];

        public StageData(string path)
        {
            //CSVからデータ取り出し
            TextAsset csv = Resources.Load(path) as TextAsset;
            StringReader reader = new StringReader(csv.text);

            int datafiledstart = 2;
            int loadnum = 0;
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine(); 

                string tmpstr;
                switch (loadnum)
                {
                    case 0:
                        stageName = line.Replace(",", "");
                        break;
                    case 1:
                        stageTooltip = line.Replace(",", "");
                        break;
                    default:
                        tmpstr = line.Replace(",", "");
                        LineDataSet(tmpstr, (loadnum - datafiledstart));
                        break;
                }

                loadnum++;
            }
        }

        /// <summary>
        /// ブロック領域情報を取得する。
        /// </summary>
        /// <returns>vector2.x : Max Horizontal、vector2.y : Max Vertical</returns>
        public Vector2 GetStageBlockMax()
        {
            return new Vector2(horizontalMaxConut, verticalMaxConut);
        }

        /// <summary>
        /// 各位置のブロックの情報を確認する
        /// </summary>
        /// <returns>-1:ブロックなし、0:破壊不可、1~9:ブロックHP</returns>
        public int GetStageBlock(int idxx, int idxy)
        {
            switch (filedBlockData[idxy,idxx])
            {
                case 'I':
                    return 0;
                case 'N':
                    return -1;
                default:
                    return (int)char.GetNumericValue(filedBlockData[idxy,idxx]);
            }
        }

        private void LineDataSet(string line, int linenum)
        {
            char[] tmp = line.ToCharArray(0, horizontalMaxConut);
            for (int idxx = 0; idxx < horizontalMaxConut; idxx++)
            {
                filedBlockData[linenum, idxx] = tmp[idxx];
            }
        }
    }
}