using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// ゲーム盤クラス
    /// </summary>
    class ClassBoard
    {
        /// <summary>
        /// レベル毎の機雷の数
        /// </summary>
        private enum MineCountLv {
            low = 10,
            mid = 40,
            hi = 99
        }

        /// <summary>
        /// レベル毎の横のセル数
        /// </summary>
        private enum CellCountXLv
        {
            low = 9,
            mid = 16,
            hi = 30
        }

        /// <summary>
        /// レベル毎の縦のセル数
        /// </summary>
        private enum CellCountYLv
        {
            low = 9,
            mid = 16,
            hi = 16
        }

        /// <summary>
        /// 機雷を表す値
        /// </summary>
        public static int NumberOfMine = 9;
        public static int NumberOfNoMine = 0;
        public const string CharactorOfMine = "M";
        public const string CharactorOfNoMine = " ";
        public const string CharactorOfNg = "N";

        /// <summary>
        /// 横のセル数
        /// </summary>
        private int maxX;
        public int MaxX { get => maxX; set => maxX = value; }

        /// <summary>
        /// 縦のセル数
        /// </summary>
        private int maxY;
        public int MaxY { get => maxY; set => maxY = value; }

        /// <summary>
        /// セル毎の数字
        /// </summary>
        private int[] cellNum;
        public int[] CellNum { get => cellNum; set => cellNum = value; }

        /// <summary>
        /// 機雷の数
        /// </summary>
        private int mineCount;
        public int MineCount { get => mineCount; set => mineCount = value; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClassBoard()
        {
            maxX = (int)CellCountXLv.low;
            maxY = (int)CellCountYLv.low;
            mineCount = (int)MineCountLv.low;

            cellNum = new int[maxX * maxY];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="level">
        /// ゲームレベル
        /// 1:初級
        /// 2:中級
        /// 3:上級
        /// 上記以外は1とする
        /// </param>
        public ClassBoard(int level)
        {
            switch (level)
            {
                case 3:                                             //上級
                    maxX = (int)CellCountXLv.hi;
                    maxY = (int)CellCountYLv.hi;
                    mineCount = (int)MineCountLv.hi;
                    break;
                case 2:                                             //中級
                    maxX = (int)CellCountXLv.mid;
                    maxY = (int)CellCountYLv.mid;
                    mineCount = (int)MineCountLv.mid;
                    break;
                case 1:                                             //初級
                default:
                    maxX = (int)CellCountXLv.low;
                    maxY = (int)CellCountYLv.low;
                    mineCount = (int)MineCountLv.low;
                    break;
            }

            cellNum = new int[maxX * maxY];
        }

        /// <summary>
        /// 初回クリック時操作
        /// </summary>
        public void FirstClick(int no)
        {
            int cellMax = maxX * maxY;
            int mineCountMax = mineCount;

            //機雷を初期位置に置く
            for (int index = 0; index < cellMax; index++)
            {
                if (index < mineCountMax)
                {
                    //クリック位置には機雷を置かない
                    if (index == no)
                    {
                        CellNum[index] = 0;
                        mineCountMax++;
                    }
                    else
                    {
                        CellNum[index] = NumberOfMine;
                    }
                }
                else
                {
                    CellNum[index] = 0;
                }
            }

            //機雷の位置をシャッフルする
            System.Random rand = new System.Random();               //乱数初期化
            for (int index = 0; index < cellMax; index++)
            {
                //0～cellMax-1の乱数
                int randIndex = rand.Next(cellMax);

                //クリック位置には機雷を置かない
                if ((index == no) || (randIndex == no))
                {
                    continue;
                }

                //スワップする
                if (index != randIndex)
                {
                    int tmp = CellNum[index];
                    CellNum[index] = CellNum[randIndex];
                    CellNum[randIndex] = tmp;
                }
            }

            //周囲の機雷を数える
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    int index = y * maxX + x;

                    //機雷判定
                    if (cellNum[index] == NumberOfMine)
                    {
                        if (x > 0)
                        {
                            UpCount(y * maxX + (x - 1));            //左のセルに+1

                            if (y > 0)
                            {
                                UpCount((y - 1) * maxX + (x - 1));  //左上のセルに+1
                            }
                        }
                        if (y > 0)
                        {
                            UpCount((y - 1) * maxX + x);            //上のセルに+1

                            if (x < maxX - 1)
                            {
                                UpCount((y - 1) * maxX + (x + 1));  //右上のセルに+1
                            }
                        }
                        if (x < maxX - 1)
                        {
                            UpCount(y * maxX + (x + 1));            //右のセルに+1

                            if (y < maxY - 1)
                            {
                                UpCount((y + 1) * maxX + (x + 1));  //右下のセルに+1
                            }
                        }
                        if (y < maxY - 1)
                        {
                            UpCount((y + 1) * maxX + x);            //下のセルに+1

                            if (x > 0)
                            {
                                UpCount((y + 1) * maxX + (x - 1));  //左下のセルに+1
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 周囲の機雷の数をカウントアップする
        /// </summary>
        /// <param name="index">セル番号</param>
        private void UpCount(int index)
        {
            if (cellNum[index] != NumberOfMine)
            {
                cellNum[index]++;
            }
        }
   }
}
