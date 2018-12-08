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
        /// 方向
        /// </summary>
        public enum CellDirection
        {
            UpperLeft,      // 左上
            Upper,          // 上
            UpperRight,     // 右上
            Right,          // 右
            DownRight,      // 右下
            Down,           // 下
            DownLeft,       // 左下
            Left            // 左
        }

        /// <summary>
        /// 機雷を表す値
        /// </summary>
        public static int NumberOfMine = 9;
        public static int NumberOfNoMine = 0;

        /// <summary>
        /// ゲームレベル
        /// </summary>
        private int gameLevel;
        public int GameLevel { get => gameLevel; set => gameLevel = value; }

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
            gameLevel = 1;
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
        /// 0:初級
        /// 1:中級
        /// 2:上級
        /// 上記以外は1とする
        /// </param>
        public ClassBoard(int level)
        {
            gameLevel = level;

            switch (level)
            {
                case 2:                                             //上級
                    maxX = (int)CellCountXLv.hi;
                    maxY = (int)CellCountYLv.hi;
                    mineCount = (int)MineCountLv.hi;
                    break;
                case 1:                                             //中級
                    maxX = (int)CellCountXLv.mid;
                    maxY = (int)CellCountYLv.mid;
                    mineCount = (int)MineCountLv.mid;
                    break;
                case 0:                                             //初級
                default:
                    maxX = (int)CellCountXLv.low;
                    maxY = (int)CellCountYLv.low;
                    mineCount = (int)MineCountLv.low;
                    break;
            }

            cellNum = new int[maxX * maxY];
        }

        /// <summary>
        /// 機雷の位置設定
        /// </summary>
        public void LayoutMine(int no)
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
            for (int index = 0; index < cellMax; index++)
            {
                //機雷判定
                if (cellNum[index] == NumberOfMine)
                {
                    UpCount(GetIndexEightSide(index, CellDirection.UpperLeft));     // 左上のセルに+1
                    UpCount(GetIndexEightSide(index, CellDirection.Upper));         // 上のセルに+1
                    UpCount(GetIndexEightSide(index, CellDirection.UpperRight));    // 右上のセルに+1
                    UpCount(GetIndexEightSide(index, CellDirection.Right));         // 右のセルに+1
                    UpCount(GetIndexEightSide(index, CellDirection.DownRight));     // 右下のセルに+1
                    UpCount(GetIndexEightSide(index, CellDirection.Down));          // 下のセルに+1
                    UpCount(GetIndexEightSide(index, CellDirection.DownLeft));      // 左下のセルに+1
                    UpCount(GetIndexEightSide(index, CellDirection.Left));          // 左のセルに+1
                }
            }

            return;
        }

        /// <summary>
        /// 周囲の機雷の数をカウントアップする
        /// </summary>
        /// <param name="index">セル番号</param>
        private void UpCount(int index)
        {
            if ((index < 0) || (index >= cellNum.Length))
            {
                return;
            }

            if (cellNum[index] != NumberOfMine)
            {
                cellNum[index]++;
            }
        }

        /// <summary>
        /// 通し番号からボード上のX座標を取得する
        /// </summary>
        /// <param name="index">通し番号</param>
        /// <returns>X座標</returns>
        public int GetPointXFromIndex(int index)
        {
            return index % maxX;
        }

        /// <summary>
        /// 通し番号からボード上のY座標を取得する
        /// </summary>
        /// <param name="index">通し番号</param>
        /// <returns>Y座標</returns>
        public int GetPointYFromIndex(int index)
        {
            return index / maxX;
        }

        /// <summary>
        /// XY座標から通し番号を取得する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>通し番号</returns>
        public int GetIndexFromPointXY(int x, int y)
        {
            return y * maxX + x;
        }

        /// <summary>
        /// 周囲のセルの通し番号を取得する
        /// </summary>
        /// <param name="index">元となるセルの通し番号</param>
        /// <param name="direction">取得対象の方向</param>
        /// <returns>
        /// 取得対象の通し番号
        /// 範囲外の場合は-1を返す
        /// </returns>
        public int GetIndexEightSide(int index, CellDirection direction)
        {
            int x = GetPointXFromIndex(index);
            int y = GetPointYFromIndex(index);

            switch (direction)
            {
                case CellDirection.UpperLeft:                       // 左上
                    if ((x <= 0) || (y <= 0)) break;                // 上端または左端の場合は範囲外
                    return index - maxX - 1;

                case CellDirection.Upper:                           // 上
                    if (y <= 0) break;                              // 上端の場合は範囲外
                    return index - maxX;

                case CellDirection.UpperRight:                      // 右上
                    if ((x >= maxX - 1) || (y <= 0)) break;         // 上端または右端の場合は範囲外
                    return index - maxX + 1;

                case CellDirection.Right:                           // 右
                    if (x >= maxX - 1) break;                       // 右端の場合は範囲外
                    return index + 1;

                case CellDirection.DownRight:                       // 右下
                    if ((x >= maxX - 1) || (y >= maxY - 1)) break;  // 下端または右端の場合は範囲外
                    return index + maxX + 1;

                case CellDirection.Down:                            // 下
                    if (y >= maxY - 1) break;                       // 下端の場合は範囲外
                    return index + maxX;

                case CellDirection.DownLeft:                        // 左下
                    if ((x <= 0) || (y >= maxY - 1)) break;         // 下端または左端の場合は範囲外
                    return index + maxX - 1;

                case CellDirection.Left:                            // 左
                    if (x <= 0) break;                              // 左端の場合は範囲外
                    return index - 1;

                default:
                    break;
            }

            return -1;
        }
    }
}
