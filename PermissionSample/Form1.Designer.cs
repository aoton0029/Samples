namespace PermissionSample
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
            btnPerm = new Button();
            btnPK = new Button();
            btnISI = new Button();
            btnKenkyu = new Button();
            SuspendLayout();
            // 
            // btnPerm
            // 
            btnPerm.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnPerm.Location = new Point(12, 12);
            btnPerm.Name = "btnPerm";
            btnPerm.Size = new Size(309, 43);
            btnPerm.TabIndex = 0;
            btnPerm.Text = "権限";
            btnPerm.UseVisualStyleBackColor = true;
            btnPerm.Click += btnPerm_Click;
            // 
            // btnPK
            // 
            btnPK.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnPK.Location = new Point(12, 92);
            btnPK.Name = "btnPK";
            btnPK.Size = new Size(165, 132);
            btnPK.TabIndex = 1;
            btnPK.Text = "PK";
            btnPK.UseVisualStyleBackColor = true;
            btnPK.Click += btnPK_Click;
            // 
            // btnISI
            // 
            btnISI.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnISI.Location = new Point(201, 92);
            btnISI.Name = "btnISI";
            btnISI.Size = new Size(165, 132);
            btnISI.TabIndex = 2;
            btnISI.Text = "ISI";
            btnISI.UseVisualStyleBackColor = true;
            btnISI.Click += btnISI_Click;
            // 
            // btnKenkyu
            // 
            btnKenkyu.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnKenkyu.Location = new Point(397, 92);
            btnKenkyu.Name = "btnKenkyu";
            btnKenkyu.Size = new Size(165, 132);
            btnKenkyu.TabIndex = 3;
            btnKenkyu.Text = "研究所";
            btnKenkyu.UseVisualStyleBackColor = true;
            btnKenkyu.Click += btnKenkyu_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnKenkyu);
            Controls.Add(btnISI);
            Controls.Add(btnPK);
            Controls.Add(btnPerm);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnPerm;
        private Button btnPK;
        private Button btnISI;
        private Button btnKenkyu;
    }
}
