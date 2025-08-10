namespace FileSample
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
            txtUserId = new TextBox();
            txtDesc = new TextBox();
            button1 = new Button();
            btnLoad = new Button();
            txtLoaded = new TextBox();
            SuspendLayout();
            // 
            // txtUserId
            // 
            txtUserId.Font = new Font("メイリオ", 12F);
            txtUserId.Location = new Point(48, 30);
            txtUserId.Name = "txtUserId";
            txtUserId.Size = new Size(129, 31);
            txtUserId.TabIndex = 0;
            // 
            // txtDesc
            // 
            txtDesc.Font = new Font("メイリオ", 12F);
            txtDesc.Location = new Point(48, 67);
            txtDesc.Multiline = true;
            txtDesc.Name = "txtDesc";
            txtDesc.Size = new Size(378, 75);
            txtDesc.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(48, 148);
            button1.Name = "button1";
            button1.Size = new Size(169, 38);
            button1.TabIndex = 2;
            button1.Text = "save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(448, 15);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(169, 38);
            btnLoad.TabIndex = 3;
            btnLoad.Text = "load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // txtLoaded
            // 
            txtLoaded.Font = new Font("メイリオ", 12F);
            txtLoaded.Location = new Point(448, 59);
            txtLoaded.Multiline = true;
            txtLoaded.Name = "txtLoaded";
            txtLoaded.Size = new Size(327, 328);
            txtLoaded.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 396);
            Controls.Add(txtLoaded);
            Controls.Add(btnLoad);
            Controls.Add(button1);
            Controls.Add(txtDesc);
            Controls.Add(txtUserId);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUserId;
        private TextBox txtDesc;
        private Button button1;
        private Button btnLoad;
        private TextBox txtLoaded;
    }
}
