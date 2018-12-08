namespace MineSweeper
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.textBoxMine = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.buttonReset);
            this.panel1.Controls.Add(this.textBoxTime);
            this.panel1.Controls.Add(this.textBoxMine);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(346, 49);
            this.panel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "初級",
            "中級",
            "上級"});
            this.comboBox1.Location = new System.Drawing.Point(76, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(50, 20);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.ComboBox1_SelectionChangeCommitted);
            // 
            // buttonReset
            // 
            this.buttonReset.BackColor = System.Drawing.Color.Yellow;
            this.buttonReset.Font = new System.Drawing.Font("Wingdings", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonReset.ForeColor = System.Drawing.Color.Black;
            this.buttonReset.Location = new System.Drawing.Point(152, 2);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(40, 40);
            this.buttonReset.TabIndex = 0;
            this.buttonReset.Text = "J";
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // textBoxTime
            // 
            this.textBoxTime.BackColor = System.Drawing.Color.Black;
            this.textBoxTime.CausesValidation = false;
            this.textBoxTime.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTime.ForeColor = System.Drawing.Color.Red;
            this.textBoxTime.Location = new System.Drawing.Point(268, 3);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.ReadOnly = true;
            this.textBoxTime.Size = new System.Drawing.Size(70, 39);
            this.textBoxTime.TabIndex = 2;
            this.textBoxTime.Text = "000";
            this.textBoxTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxMine
            // 
            this.textBoxMine.BackColor = System.Drawing.Color.Black;
            this.textBoxMine.CausesValidation = false;
            this.textBoxMine.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMine.ForeColor = System.Drawing.Color.Red;
            this.textBoxMine.Location = new System.Drawing.Point(3, 3);
            this.textBoxMine.Name = "textBoxMine";
            this.textBoxMine.ReadOnly = true;
            this.textBoxMine.Size = new System.Drawing.Size(70, 39);
            this.textBoxMine.TabIndex = 1;
            this.textBoxMine.Text = "000";
            this.textBoxMine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Location = new System.Drawing.Point(12, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(346, 304);
            this.panel2.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 376);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "マインスイーパー";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxMine;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

