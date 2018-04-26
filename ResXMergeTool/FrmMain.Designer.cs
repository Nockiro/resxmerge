namespace ResXMergeTool
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btn_fileOne = new System.Windows.Forms.Button();
            this.btn_startDiff = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_selectOutput = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Files:";
            // 
            // btn_fileOne
            // 
            this.btn_fileOne.Location = new System.Drawing.Point(97, 13);
            this.btn_fileOne.Name = "btn_fileOne";
            this.btn_fileOne.Size = new System.Drawing.Size(370, 23);
            this.btn_fileOne.TabIndex = 3;
            this.btn_fileOne.Text = "Choose..";
            this.btn_fileOne.UseVisualStyleBackColor = true;
            this.btn_fileOne.Click += new System.EventHandler(this.btn_file_Click);
            // 
            // btn_startDiff
            // 
            this.btn_startDiff.Location = new System.Drawing.Point(13, 133);
            this.btn_startDiff.Name = "btn_startDiff";
            this.btn_startDiff.Size = new System.Drawing.Size(75, 23);
            this.btn_startDiff.TabIndex = 6;
            this.btn_startDiff.Text = "Diff!";
            this.btn_startDiff.UseVisualStyleBackColor = true;
            this.btn_startDiff.Click += new System.EventHandler(this.btn_startDiff_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(402, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Note: You need exactly one BASE, one LOCAL and one REMOTE file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Output file";
            // 
            // btn_selectOutput
            // 
            this.btn_selectOutput.Location = new System.Drawing.Point(97, 56);
            this.btn_selectOutput.Name = "btn_selectOutput";
            this.btn_selectOutput.Size = new System.Drawing.Size(370, 23);
            this.btn_selectOutput.TabIndex = 9;
            this.btn_selectOutput.Text = "Choose..";
            this.btn_selectOutput.UseVisualStyleBackColor = true;
            this.btn_selectOutput.Click += new System.EventHandler(this.btn_selectOutput_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 208);
            this.Controls.Add(this.btn_selectOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_startDiff);
            this.Controls.Add(this.btn_fileOne);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMain";
            this.Text = "Choose files";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fileOne;
        private System.Windows.Forms.Button btn_startDiff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_selectOutput;
    }
}