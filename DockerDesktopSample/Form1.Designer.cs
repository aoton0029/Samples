namespace DockerDesktopSample
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlSide = new Panel();
            btnImages = new Button();
            btnContainer = new Button();
            pnlMain = new Panel();
            button1 = new Button();
            pnlSide.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSide
            // 
            pnlSide.BackColor = Color.FromArgb(20, 19, 25);
            pnlSide.BorderStyle = BorderStyle.FixedSingle;
            pnlSide.Controls.Add(button1);
            pnlSide.Controls.Add(btnImages);
            pnlSide.Controls.Add(btnContainer);
            pnlSide.Dock = DockStyle.Left;
            pnlSide.ForeColor = SystemColors.ButtonFace;
            pnlSide.Location = new Point(0, 0);
            pnlSide.Name = "pnlSide";
            pnlSide.Size = new Size(166, 583);
            pnlSide.TabIndex = 0;
            // 
            // btnImages
            // 
            btnImages.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnImages.FlatAppearance.BorderSize = 0;
            btnImages.FlatStyle = FlatStyle.Flat;
            btnImages.Font = new Font("メイリオ", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnImages.ForeColor = Color.White;
            btnImages.Location = new Point(-1, 44);
            btnImages.Margin = new Padding(0);
            btnImages.Name = "btnImages";
            btnImages.Size = new Size(164, 44);
            btnImages.TabIndex = 1;
            btnImages.Text = "Images";
            btnImages.UseVisualStyleBackColor = true;
            // 
            // btnContainer
            // 
            btnContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnContainer.FlatAppearance.BorderSize = 0;
            btnContainer.FlatStyle = FlatStyle.Flat;
            btnContainer.Font = new Font("メイリオ", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnContainer.ForeColor = Color.White;
            btnContainer.Location = new Point(0, 0);
            btnContainer.Margin = new Padding(0);
            btnContainer.Name = "btnContainer";
            btnContainer.Size = new Size(164, 44);
            btnContainer.TabIndex = 0;
            btnContainer.Text = "Container";
            btnContainer.UseVisualStyleBackColor = true;
            // 
            // pnlMain
            // 
            pnlMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlMain.Location = new Point(168, 1);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(806, 580);
            pnlMain.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(26, 180);
            button1.Name = "button1";
            button1.Size = new Size(81, 23);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(26, 27, 32);
            ClientSize = new Size(976, 583);
            Controls.Add(pnlMain);
            Controls.Add(pnlSide);
            Name = "Form1";
            Text = "Form1";
            pnlSide.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSide;
        private Button btnContainer;
        private Button btnImages;
        private Panel pnlMain;
        private Button button1;
    }
}
