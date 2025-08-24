namespace DockerDesktopSample
{
    partial class UcPageContainer
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            btnReload = new Button();
            flp = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReload.Font = new Font("メイリオ", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnReload.Location = new Point(665, 6);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(95, 33);
            btnReload.TabIndex = 3;
            btnReload.Text = "更新";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // flp
            // 
            flp.Location = new Point(3, 45);
            flp.Name = "flp";
            flp.Size = new Size(757, 512);
            flp.TabIndex = 4;
            // 
            // UcPageContainer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flp);
            Controls.Add(btnReload);
            Name = "UcPageContainer";
            Size = new Size(763, 560);
            ResumeLayout(false);
        }

        #endregion

        private Button btnReload;
        private FlowLayoutPanel flp;
    }
}
