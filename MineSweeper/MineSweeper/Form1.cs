using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// マインスイーパー
/// </summary>
namespace MineSweeper
{
    /// <summary>
    /// フォーム
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// セルの大きさ
        /// </summary>
        private const int CellWidth = 32;
        private const int CellHeight = 32;

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
        /// 機雷マーク
        /// </summary>
        private const string CharactorOfMine = "M";
        private const string CharactorOfNoMine = " ";
        private const string CharactorOfNg = "N";

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
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = GameLevel;
            NewGame(GameLevel);
            return;
        }

        /// <summary>
        /// 新規ゲーム設定
        /// </summary>
        /// <param name="level">ゲームレベル</param>
        private void NewGame(int level)
        {
            // インスタンスの生成
            classBoard = new ClassBoard(level);
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
                    (no / classBoard.MaxX) * CellHeight);
                labelFront[no].Size = new Size(CellWidth, CellHeight);
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
                labelFront[no].MouseDown += new MouseEventHandler(LabelFront_MouseDown);    //右クリックを受けるためにClickではなくMouseDownを使用
                labelBack[no].MouseDown += new MouseEventHandler(LabelBackt_MouseDown);
            }

            //フォームにコントロールを追加
            panel2.Controls.AddRange(labelFront);
            panel2.Controls.AddRange(labelBack);

            // パネルの幅調整
            panel2.Size = new Size(CellWidth * classBoard.MaxX + 4, CellHeight * classBoard.MaxY + 4);
            panel1.Size = new Size(panel2.Size.Width, 49);
            textBoxMine.Location = new Point(3, 3);
            textBoxTime.Location = new Point((panel2.Width - textBoxTime.Width) - 7, 3);
            buttonReset.Location = new Point((panel2.Width - buttonReset.Width) / 2, 3);

            // フォームの大きさ調整
            Width = panel2.Size.Width + panel2.Location.X * 2 + 16;
            Height = panel2.Size.Height + panel2.Location.Y + 52;

            ResumeLayout(false);

            // ゲーム状況を初期化
            ResetGame();

            return;
        }

        /// <summary>
        /// テキストボックスへ3桁の値を表示する
        /// </summary>
        /// <param name="textBox">対象となるテキストボックス</param>
        /// <param name="num">表示する値</param>
        private void DispNumber(TextBox textBox, int num)
        {
            textBox.Text = num.ToString("000");
            return;
        }

        /// <summary>
        /// リセットボタンのクリックイベント
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void ButtonReset_Click(object sender, EventArgs e)
        {
            // ゲームの初期化
            ResetGame();
            return;
        }

        /// <summary>
        /// ゲーム状態の初期化
        /// </summary>
        private void ResetGame()
        {
            // すべてのセルの状態を初期化
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

            return;
        }

        /// <summary>
        /// 初回クリック時の処理
        /// </summary>
        /// <param name="no">セルの通し番号</param>
        private void CheckStart(int no)
        {
            // 最初のクリック
            if (flagFirstClick)
            {
                flagFirstClick = false;
                classBoard.LayoutMine(no);

                for (int index = 0; index < labelFront.Length; index++)
                {
                    if (classBoard.CellNum[index] == ClassBoard.NumberOfMine)
                    {
                        labelBack[index].Text = CharactorOfMine;
                    }
                    else if (classBoard.CellNum[index] == ClassBoard.NumberOfNoMine)
                    {
                        labelBack[index].Text = CharactorOfNoMine;
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
            return;
        }

        /// <summary>
        /// フィールドセル(前面)のマウスダウンイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void LabelFront_MouseDown(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            int index = (int)label.Tag;

            Debug.WriteLine("LabelFront_MouseDown " + e.Button.ToString());

            // マウスのボタンに応じて処理を行う
            switch (e.Button)
            {
                case MouseButtons.Left:                             // 左ボタン：セルを開く操作
                    if (label.Text == Space)
                    {
                        CheckStart(index);                          // 初回クリック時の処理

                        if (labelBack[index].Text == CharactorOfMine)
                        {
                            GameOver(false, index);                 // ミス
                        }
                        else
                        {
                            OpenCell(index);                        // セルを開く

                            //クリア確認
                            if (CheckGameClear())
                            {
                                GameOver(true, -1);                 //クリア
                            }
                        }
                    }
                    break;

                case MouseButtons.Right:                            // 右ボタン：旗の操作
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

            return;
        }

        /// <summary>
        /// フィールドセル(背面)のマウスダウンイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void LabelBackt_MouseDown(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            int index = (int)label.Tag;

            Debug.WriteLine("LabelBack_MouseDown " + e.Button.ToString());

            //if (labelFront[index].Visible == true)                  // 開いていないところは無視
            //{
            //    return;
            //}

            // マウスのボタンに応じて処理を行う
            switch (Control.MouseButtons)
            {
                case MouseButtons.Left:                             // 左ボタン
                case MouseButtons.Right:                            // 右ボタン
                    break;                                          // 単独の時は無視する

                case MouseButtons.Left | MouseButtons.Right:        // 左右ボタン：八方向のセルを開く

                    if (label.Text != Space)
                    {
                        // 周囲8方向
                        int[] cellno = {
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.UpperLeft),
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Upper),
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.UpperRight),
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Right),
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.DownRight),
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Down),
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.DownLeft),
                            classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Left),
                        };

                        foreach (int no in cellno)
                        {
                            if (no < 0)
                            {
                                continue;
                            }
                            else if (labelFront[no].Text != Space)
                            {                                       // 旗の立っているセルは開かない
                                continue;
                            }

                            if (labelBack[no].Text == CharactorOfMine)
                            {
                                GameOver(false, no);                // ミス
                                break;
                            }
                            else
                            {
                                OpenCell(no);                       // セルを開く

                                //クリア確認
                                if (CheckGameClear())
                                {
                                    GameOver(true, -1);             //クリア
                                    break;
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return;
        }

        /// <summary>
        /// 空白セルの表示
        /// </summary>
        private void OpenCell(int index)
        {
            // 盤外のセルは対象外とする
            if (index < 0)
            {
                return;
            }

            // 旗のあるセルは処理しない
            if (labelFront[index].Text != Space)
            {
                return;
            }

            // 既に開かれたセルは処理しない
            if (labelFront[index].Visible == false)
            {
                return;
            }

            // セルを開く
            labelFront[index].Visible = false;
            cellCount--;

            // 空白セルの判定
            if (classBoard.CellNum[index] == ClassBoard.NumberOfNoMine)
            {
                // 周囲8方向
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.UpperLeft));
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Upper));
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.UpperRight));
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Right));
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.DownRight));
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Down));
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.DownLeft));
                OpenCell(classBoard.GetIndexEightSide(index, ClassBoard.CellDirection.Left));
            }

            return;
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
        /// <param name="smile">顔の種類</param>
        /// <param name="no">最後にクリックしたセルの通し番号</param>
        private void GameOver(bool smile, int no)
        {
            timer1.Stop();                          //タイマストップ

            //正解表示
            for (int index = 0; index < labelFront.Length; index++)
            {
                if (index == no)
                {
                    labelFront[index].Text = CharactorOfNg;
                    labelFront[index].Visible = true;
                }
                else if (classBoard.CellNum[index] == ClassBoard.NumberOfMine)
                {
                    labelFront[index].Text = CharactorOfMine;
                    labelFront[index].Visible = true;
                }
            }

            //フェイスマーク設定
            if (smile)
            {
                buttonReset.Text = Face1;
                MessageBox.Show("クリア");
            }
            else
            {
                buttonReset.Text = Face3;
                MessageBox.Show("アウト");
            }

            ResetGame();
        }

        /// <summary>
        /// タイマチックイベント
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            timeCount++;
            DispNumber(textBoxTime, timeCount);
        }

        /// <summary>
        /// レベル選択コンボボックス設定イベント
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">送信先オブジェクト</param>
        private void ComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int level = comboBox.SelectedIndex;

            if ((level >= 0) && (level < 3))
            {
                panel2.Controls.Clear();
                NewGame(level);
            }
        }
    }
}
