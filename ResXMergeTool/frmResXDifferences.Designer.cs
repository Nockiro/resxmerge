namespace ResXMergeTool
{
    partial class FrmResXDifferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmResXDifferences));
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSourceVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl = new System.Windows.Forms.Panel();
            this.chkAutoRemoveBaseOnly = new System.Windows.Forms.CheckBox();
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lbl_caption = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_addFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_saveOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_deleteEntry = new System.Windows.Forms.ToolStripButton();
            this.btn_startExternal = new System.Windows.Forms.ToolStripButton();
            this.btn_reread = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnl.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKey,
            this.colValue,
            this.colComment,
            this.colSource,
            this.colSourceVal});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 35);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(1248, 540);
            this.dgv.TabIndex = 1;
            this.dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_CellFormatting);
            this.dgv.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgv_DefaultValuesNeeded);
            this.dgv.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgv_RowStateChanged);
            this.dgv.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgv_SortCompare);
            // 
            // colKey
            // 
            this.colKey.HeaderText = "Key";
            this.colKey.Name = "colKey";
            // 
            // colValue
            // 
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            // 
            // colComment
            // 
            this.colComment.HeaderText = "Comment";
            this.colComment.Name = "colComment";
            // 
            // colSource
            // 
            this.colSource.HeaderText = "Source";
            this.colSource.Name = "colSource";
            this.colSource.ReadOnly = true;
            // 
            // colSourceVal
            // 
            this.colSourceVal.HeaderText = "Source Value";
            this.colSourceVal.Name = "colSourceVal";
            this.colSourceVal.ReadOnly = true;
            this.colSourceVal.Visible = false;
            // 
            // pnl
            // 
            this.pnl.AutoSize = true;
            this.pnl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnl.BackColor = System.Drawing.SystemColors.Control;
            this.pnl.Controls.Add(this.chkAutoRemoveBaseOnly);
            this.pnl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl.Location = new System.Drawing.Point(0, 575);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(1248, 27);
            this.pnl.TabIndex = 2;
            // 
            // chkAutoRemoveBaseOnly
            // 
            this.chkAutoRemoveBaseOnly.AutoSize = true;
            this.chkAutoRemoveBaseOnly.Location = new System.Drawing.Point(12, 7);
            this.chkAutoRemoveBaseOnly.Name = "chkAutoRemoveBaseOnly";
            this.chkAutoRemoveBaseOnly.Size = new System.Drawing.Size(268, 17);
            this.chkAutoRemoveBaseOnly.TabIndex = 2;
            this.chkAutoRemoveBaseOnly.Text = "Automatically remove \'BASE ONLY\' entries on save";
            this.chkAutoRemoveBaseOnly.UseVisualStyleBackColor = true;
            // 
            // bw
            // 
            this.bw.WorkerSupportsCancellation = true;
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_DoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_RunWorkerCompleted);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_addFiles,
            this.btn_saveOutput,
            this.lbl_caption,
            this.toolStripSeparator2,
            this.btn_deleteEntry,
            this.toolStripSeparator1,
            this.btn_startExternal,
            this.btn_reread});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(4, 0, 1, 0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1248, 35);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lbl_caption
            // 
            this.lbl_caption.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lbl_caption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lbl_caption.IsLink = true;
            this.lbl_caption.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lbl_caption.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbl_caption.Name = "lbl_caption";
            this.lbl_caption.Size = new System.Drawing.Size(166, 32);
            this.lbl_caption.Text = "Nockiro - ResXMergeTool v1.1";
            this.lbl_caption.Click += new System.EventHandler(this.lbl_caption_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // btn_addFiles
            // 
            this.btn_addFiles.Image = global::ResXMergeTool.Properties.Resources.addfile;
            this.btn_addFiles.Name = "btn_addFiles";
            this.btn_addFiles.Size = new System.Drawing.Size(105, 35);
            this.btn_addFiles.Text = "Choose files..";
            this.btn_addFiles.Click += new System.EventHandler(this.btn_addFiles_Click);
            // 
            // btn_saveOutput
            // 
            this.btn_saveOutput.Image = global::ResXMergeTool.Properties.Resources.savefile;
            this.btn_saveOutput.Name = "btn_saveOutput";
            this.btn_saveOutput.Size = new System.Drawing.Size(104, 35);
            this.btn_saveOutput.Text = "Save output..";
            this.btn_saveOutput.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btn_deleteEntry
            // 
            this.btn_deleteEntry.Enabled = false;
            this.btn_deleteEntry.Image = global::ResXMergeTool.Properties.Resources.minus_2;
            this.btn_deleteEntry.Name = "btn_deleteEntry";
            this.btn_deleteEntry.Size = new System.Drawing.Size(90, 32);
            this.btn_deleteEntry.Text = "Delete entry";
            this.btn_deleteEntry.Click += new System.EventHandler(this.btn_deleteEntry_Click);
            // 
            // btn_startExternal
            // 
            this.btn_startExternal.Image = global::ResXMergeTool.Properties.Resources.external;
            this.btn_startExternal.Name = "btn_startExternal";
            this.btn_startExternal.Size = new System.Drawing.Size(119, 32);
            this.btn_startExternal.Text = "Start external tool";
            this.btn_startExternal.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btn_reread
            // 
            this.btn_reread.Enabled = false;
            this.btn_reread.Image = global::ResXMergeTool.Properties.Resources.sync;
            this.btn_reread.Name = "btn_reread";
            this.btn_reread.Size = new System.Drawing.Size(87, 32);
            this.btn_reread.Text = "Reread files";
            this.btn_reread.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // FrmResXDifferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 602);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmResXDifferences";
            this.Text = "ResX Differences";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnl.ResumeLayout(false);
            this.pnl.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.DataGridView dgv;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colSource;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colSourceVal;
        internal System.Windows.Forms.Panel pnl;
        internal System.ComponentModel.BackgroundWorker bw;
        private System.Windows.Forms.CheckBox chkAutoRemoveBaseOnly;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem btn_addFiles;
        private System.Windows.Forms.ToolStripMenuItem btn_saveOutput;
        private System.Windows.Forms.ToolStripLabel lbl_caption;
        private System.Windows.Forms.ToolStripButton btn_deleteEntry;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_startExternal;
        private System.Windows.Forms.ToolStripButton btn_reread;
    }
}

