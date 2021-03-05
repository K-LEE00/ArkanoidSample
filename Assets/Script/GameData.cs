using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameData
{
    public enum GameStatus
    {
        Title,
        Wait,
        Play,
        End
    }

    public class StageData
    {
        static public int horizontalMaxConut = 14;
        static public int verticalMaxConut = 10;


        private TextAsset csvFile; // CSVファイル

        public string stageName;
        public string stageTooltip;
        char[,] filedBlockData = new char[verticalMaxConut, horizontalMaxConut];


        private void LineDataSet(string line, int linenum)
        {
            char[] tmp = line.ToCharArray(0, horizontalMaxConut);
            for (int idxx = 0; idxx < horizontalMaxConut; idxx++)
            {
                filedBlockData[linenum, idxx] = tmp[idxx];
            }
        }

        public StageData(string path)
        {
            //CSVからデータ取り出し
            TextAsset csv = Resources.Load(path) as TextAsset;
            StringReader reader = new StringReader(csv.text);

            int datafileline = 2;
            int loadlineindex = 0;
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();

                string tmpstr;
                switch (loadlineindex)
                {
                    case 0:
                        stageName = line.Replace(",", "");
                        break;
                    case 1:
                        stageTooltip = line.Replace(",", "");
                        break;
                    default:
                        tmpstr = line.Replace(",", "");
                        LineDataSet(tmpstr, (loadlineindex - datafileline));
                        break;
                }

                loadlineindex++;
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
            switch (filedBlockData[idxy, idxx])
            {
                case 'I':       //破壊不可
                    return 0;
                case 'N':       //ブロックなし
                    return -1;
                default:        //値=HP
                    return (int)char.GetNumericValue(filedBlockData[idxy, idxx]);
            }
        }

    }
}
