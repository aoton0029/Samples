namespace EmguCVSample
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
            btnTrain = new Button();
            btnAuthorize = new Button();
            SuspendLayout();
            // 
            // btnTrain
            // 
            btnTrain.Font = new Font("メイリオ", 18F);
            btnTrain.Location = new Point(106, 95);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(227, 211);
            btnTrain.TabIndex = 0;
            btnTrain.Text = "学習";
            btnTrain.UseVisualStyleBackColor = true;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnAuthorize
            // 
            btnAuthorize.Font = new Font("メイリオ", 18F);
            btnAuthorize.Location = new Point(382, 95);
            btnAuthorize.Name = "btnAuthorize";
            btnAuthorize.Size = new Size(227, 211);
            btnAuthorize.TabIndex = 1;
            btnAuthorize.Text = "認証";
            btnAuthorize.UseVisualStyleBackColor = true;
            btnAuthorize.Click += btnAuthorize_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnAuthorize);
            Controls.Add(btnTrain);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnTrain;
        private Button btnAuthorize;
    }
}
