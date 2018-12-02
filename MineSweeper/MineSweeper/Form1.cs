using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// セルの大きさ
        /// </summary>
        private const int CellWidth = 32;
        private const int FieldSizeY = 32;

        /// <summary>
        /// 数字のフォントサイズ
        /// </summary>
        private const float FontSize = 20f;

        /// <summary>
        /// ゲームレベル
        /// </summary>
        private const int GameLevel = 1;

        /// <summary>
        /// フェイスマーク(Wingdings)
        /// </summary>
        private const string Face1 = "J";
        private const string Face2 = "K";
        private const string Face3 = "L";

        /// <summary>
        /// フラグマーク(Wingdings)
        /// </summary>
        private const string Space = " ";
        private const string Flag1 = "O";
        private const string Flag2 = "P";


        /// <summary>
        /// ゲーム盤
        /// </summary>
        private ClassBoard classBoard;

        /// <summary>
        /// 機雷の数
        /// </summary>
        private int mineCount;

        /// <summary>
        /// 開いていないセルの数
        /// </summary>
        private int cellCount;

        /// <summary>
        /// タイマー
        /// </summary>
        private int timeCount;

        /// <summary>
        /// 初回クリックフラグ
        /// </summary>
        private bool flagFirstClick;

        /// <summary>
        /// 前面ラベル配列
        /// </summary>
        private Label[] labelFront;

        /// <summary>
        /// 背面ラベル配列
        /// </summary>
        private Label[] labelBack;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form1_Loadイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // インスタンスの生成
            classBoard = new ClassBoard(GameLevel);                 // (ToDo)ゲームレベル選択の機能を追加したらGameLevelを変更すること
            labelFront = new Label[classBoard.MaxX * classBoard.MaxY];  // ラベル配列の生成
            labelBack = new Label[classBoard.MaxX * classBoard.MaxY];   // ラベル配列の生成

            // Labelコントロールのプロパティを設定する
            SuspendLayout();
            for (int no = 0; no < labelFront.Length; no++)
            {
                //インスタンス作成
                labelFront[no] = new Label();
                labelBack[no] = new Label();
                //サイズと位置を設定する
                labelFront[no].Location = new Point(
                    (no % classBoard.MaxX) * CellWidth,
                    (no / classBoard.MaxY) * FieldSizeY);
                labelFront[no].Size = new Size(CellWidth, FieldSizeY);
                labelFront[no].BorderStyle = BorderStyle.FixedSingle;
                labelFront[no].BackColor = Color.LightGray;
                labelFront[no].Font = new Font("Wingdings", FontSize);
                labelFront[no].Tag = no;
                //ラベルを設定
                labelBack[no].Location = labelFront[no].Location;
                labelBack[no].Size = labelFront[no].Size;
                labelBack[no].Tag = labelFront[no].Tag;
                labelBack[no].Font = new Font(labelBack[no].Font.OriginalFontName, FontSize);
                //イベントハンドラに関連付け
                //labelFront[no].Click += new EventHandler(labelField_Click);
                labelFront[no].MouseDown += new MouseEventHandler(labelField_MouseDown);    //右クリックを受けるためにClickではなくMouseDownを使用
            }
            ResumeLayout(false);

            //フォームにコントロールを追加
            panel2.Controls.AddRange(labelFront);
            panel2.Controls.AddRange(labelBack);

            // パネルの幅調整
            panel2.Size = new Size(CellWidth * classBoard.MaxX + 4, FieldSizeY * classBoard.MaxY + 4);
            panel1.Size = new Size(panel2.Size.Width, 49);
            textBoxMine.Location = new Point(3, 3);
            textBoxTime.Location = new Point((panel2.Width - textBoxTime.Width) - 7, 3);
            buttonReset.Location = new Point((panel2.Width - buttonReset.Width) / 2, 3);

            // フォームの幅調整
            Width = panel2.Size.Width + panel2.Location.X * 2 + 16;

            ResetGame();
        }

        /// <summary>
        /// テキストボックスへ3桁の値を表示する
        /// </summary>
        /// <param name="textBox">対象となるテキストボックス</param>
        /// <param name="num">表示する値</param>
        private void DispNumber(TextBox textBox, int num)
        {
            textBox.Text = num.ToString("000");
        }

        /// <summary>
        /// リセットボタンのクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            // ゲームの初期化
            ResetGame();
        }

        /// <summary>
        /// ゲーム状態の初期化
        /// </summary>
        private void ResetGame()
        {
            foreach (var item in labelFront)
            {
                item.Visible = true;
                item.Text = Space;
            }

            // フェイスマーク初期化
            buttonReset.Text = Face2;

            // 機雷の数設定
            mineCount = classBoard.MineCount;
            DispNumber(textBoxMine, mineCount);

            // 開いていないセルの数初期化
            cellCount = classBoard.MaxX * classBoard.MaxY;

            // タイマ値初期化
            timer1.Stop();
            timeCount = 0;
            DispNumber(textBoxTime, timeCount);

            // 初回クリックフラグON
            flagFirstClick = true;
        }

        /// <summary>
        /// 盤上のセルのクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelBoard_Click(object sender, EventArgs e)
        {
            Label item = (Label)sender;
            int no = (int)item.Tag;

            // 最初のクリック
            if (flagFirstClick)
            {
                flagFirstClick = false;
                classBoard.FirstClick(no);

                for (int index = 0; index < labelFront.Length; index++)
                {
                    if (classBoard.CellNum[index] == ClassBoard.NumberOfMine)
                    {
                        labelBack[index].Text = ClassBoard.CharactorOfMine;
                    }
                    else if (classBoard.CellNum[index] == ClassBoard.NumberOfNoMine)
                    {
                        labelBack[index].Text = ClassBoard.CharactorOfNoMine;
                    }
                    else
                    {
                        labelBack[index].Text = classBoard.CellNum[index].ToString();
                    }
                }

                //タイマスタート
                timer1.Interval = 1000;
                timer1.Start();
            }

            Debug.WriteLine("labelField_Click no=" + no.ToString() + " num=" + labelBack[no].Text);
        }

        /// <summary>
        /// フィールドセルのマウスダウンイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelField_MouseDown(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            int index = (int)label.Tag;

            Debug.WriteLine("labelField_MouseDown " + e.Button.ToString());

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (label.Text == Space)
                    {
                        labelBoard_Click(sender, e);
                        if (labelBack[index].Text == ClassBoard.CharactorOfMine)
                        {
                            timer1.Stop();                          //タイマストップ
                            GameOver(false, index);
                            MessageBox.Show("アウト");
                            ResetGame();
                        }
                        else
                        {
                            OpenCell(index % classBoard.MaxX, index / classBoard.MaxX);

                            //クリア確認
                            if (CheckGameClear())
                            {
                                timer1.Stop();                      //タイマストップ
                                GameOver(true, -1);                 //クリア
                                MessageBox.Show("クリア");
                                ResetGame();
                            }
                        }
                    }
                    break;
                case MouseButtons.Right:
                    if (label.Text == Space)
                    {
                        mineCount--;
                        DispNumber(textBoxMine, mineCount);
                        label.Text = Flag1;

                    }
                    else if (label.Text == Flag1)
                    {
                        label.Text = Flag2;

                    }
                    else
                    {
                        mineCount++;
                        DispNumber(textBoxMine, mineCount);
                        label.Text = Space;
                    }
                    Debug.WriteLine("label.Text=" + label.Text);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 空白セルの表示
        /// </summary>
        private void OpenCell(int x, int y)
        {
            // XY座標からindexへ変換
            int index = y * classBoard.MaxX + x;

            // 既に開かれたセルは処理しない
            if (labelFront[index].Visible == false)
            {
                return;
            }

            // セルを開く
            labelFront[index].Visible = false;
            cellCount--;

            // 空白セルの判定
            if (labelBack[index].Text == ClassBoard.CharactorOfNoMine)
            {
                // 上へ
                if (y > 0)
                {
                    OpenCell(x, y - 1);
 
                    // 右上へ
                    if (x < classBoard.MaxX - 1)
                    {
                        OpenCell(x + 1, y - 1);
                    }
                }

                // 右へ
                if (x < classBoard.MaxX - 1)
                {
                    OpenCell(x + 1, y);

                    // 右下へ
                    if (y < classBoard.MaxY - 1)
                    {
                        OpenCell(x + 1, y + 1);
                    }
                }

                // 下へ
                if (y < classBoard.MaxY - 1)
                {
                    OpenCell(x, y + 1);

                    // 左下へ
                    if (x > 0)
                    {
                        OpenCell(x - 1, y + 1);
                    }
                }

                // 左へ
                if (x > 0)
                {
                    OpenCell(x - 1, y);

                    // 左上へ
                    if (y > 0)
                    {
                        OpenCell(x - 1, y - 1);
                    }
                }
            }
        }

        /// <summary>
        /// ゲームのクリア判定
        /// </summary>
        /// <returns>
        /// 判定結果
        /// true :クリア
        /// false:継続
        /// </returns>
        private bool CheckGameClear()
        {
            Debug.WriteLine("残りセル数=" + cellCount);

            if (cellCount == classBoard.MineCount)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ゲームオーバー
        /// </summary>
        private void GameOver(bool smile, int no)
        {
            //フェイスマーク設定
            if (smile)
            {
                buttonReset.Text = Face1;
            }
            else
            {
                buttonReset.Text = Face3;
            }

            //正解表示
            for (int index = 0; index < labelFront.Length; index++)
            {
                if (index == no)
                {
                    labelFront[index].Text = ClassBoard.CharactorOfNg;
                    labelFront[index].Visible = true;
                }
                else if (classBoard.CellNum[index] == ClassBoard.NumberOfMine)
                {
                    labelFront[index].Text = ClassBoard.CharactorOfMine;
                }
            }
        }

        //タイマチックイベント
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeCount++;
            DispNumber(textBoxTime, timeCount);
        }
    }
}
