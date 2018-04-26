using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace ResXMergeTool
{
    public partial class FrmResXDifferences : Form
    {
        String SavePath = "";
        String[] FilesToDiff;

        #region Event Handlers

        #region Form
        public FrmResXDifferences(String[] diffFiles, String savePath)
        {
            InitializeComponent();
            chkAutoRemoveBaseOnly.Checked = Properties.Settings.Default.AutoRemoveBaseOnly;
            FilesToDiff = diffFiles;
            SavePath = savePath;
        }

        private void frmResXDifferences_Load(object sender, EventArgs e)
        {
                bw.RunWorkerAsync();

        }
        #endregion

        #region Background Worker
        private void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>(10000);

            EnableControls(false);

            //TOOD: Variable instead of current directory
            try
            {
                FileParser fileParser = new FileParser(Directory.GetCurrentDirectory());
                fileParser.ParseResXFiles(FilesToDiff);

                foreach (ResXSourceNode n in fileParser.OriginNodes.Values)
                {
                    rows.Add(AddRow(n.Node.Name, n.Node.GetValue((ITypeResolutionService)null), n.Node.Comment, ResXSourceNode.GetStringFromEnum(n.Source), n.Source));
                }

                foreach (ResXConflictNode n in fileParser.NodeConflicts.Values)
                {
                    if (n.BaseNode != null) rows.Add(AddRow(n.BaseNode.Name, n.BaseNode.GetValue((ITypeResolutionService)null), n.BaseNode.Comment, "XCONFLICT - BASE", ResXSourceType.CONFLICT));
                    if (n.LocalNode != null) rows.Add(AddRow(n.LocalNode.Name, n.LocalNode.GetValue((ITypeResolutionService)null), n.LocalNode.Comment, "XCONFLICT - LOCAL", ResXSourceType.CONFLICT));
                    if (n.RemoteNode != null) rows.Add(AddRow(n.RemoteNode.Name, n.RemoteNode.GetValue((ITypeResolutionService)null), n.RemoteNode.Comment, "XCONFLICT - REMOTE", ResXSourceType.CONFLICT));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("One or more files failed to parse properly: " + ex.Message);
            }
            AddRows(rows);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgv.Sort(colSource, ListSortDirection.Ascending);
            EnableControls(true);
            dgv.Cursor = Cursors.Default;
        }
        #endregion

        #region Buttons
        private void btnCancel_Click(object sender, EventArgs e) => Program.StartExternalMergeTool();
        
        private void btnRestart_Click(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
            bw.RunWorkerAsync();

        }

        #region Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            ResXResourceWriter resX = null;


            EnableControls(false);
            dgv.Cursor = Cursors.WaitCursor;

            try
            {
                if (dgv.IsCurrentCellDirty | dgv.IsCurrentRowDirty)
                    dgv.EndEdit();
                dgv.Sort(colKey, ListSortDirection.Ascending);

                resX = new ResXResourceWriter(Path.Combine(Directory.GetCurrentDirectory(), SavePath));

                for (int i = 0; i <= dgv.RowCount - 1; i++)
                {
                    if (dgv.Rows[i].IsNewRow)
                        continue;
                    if (chkAutoRemoveBaseOnly.Checked && ((ResXSourceType)dgv.Rows[i].Cells[colSourceVal.Index].Value) == ResXSourceType.BASE)
                        continue;

                    ResXDataNode n = new ResXDataNode(Convert.ToString(dgv[colKey.Index, i].Value), dgv[colValue.Index, i].Value);
                    n.Comment = Convert.ToString(dgv[colComment.Index, i].Value);
                    resX.AddResource(n);
                }
                resX.Generate();
                resX.Close();

                Properties.Settings.Default.AutoRemoveBaseOnly = chkAutoRemoveBaseOnly.Checked;
                Properties.Settings.Default.Save();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The ResX file '" + Environment.GetCommandLineArgs()[5] + "' failed to save properly: " + ex.Message);
                EnableControls(true);
            }

        }
        #endregion
        #endregion

        #region Data Grid View
        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (Convert.ToString(dgv[colSource.Index, e.RowIndex].Value))
            {
                case "ALL":
                case "BOTH":
                    e.CellStyle.BackColor = Color.Aquamarine;
                    break;
                case "BASE":
                    e.CellStyle.BackColor = Color.MistyRose;
                    break;
                case "LOCAL":
                case "LOCAL ONLY":
                    e.CellStyle.BackColor = Color.LightGreen;
                    break;
                case "REMOTE":
                case "REMOTE ONLY":
                    e.CellStyle.BackColor = Color.LightSkyBlue;
                    break;
                default:
                    e.CellStyle.BackColor = Color.Firebrick;
                    e.CellStyle.ForeColor = Color.White;
                    break;
            }

        }

        private void dgv_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[colComment.Index].Value = String.Empty;
            e.Row.Cells[colKey.Index].Value = "String1";
            e.Row.Cells[colSource.Index].Value = "MANUAL";
            e.Row.Cells[colSourceVal.Index].Value = ResXSourceType.MANUAL;
            e.Row.Cells[colValue.Index].Value = String.Empty;
        }

        private void dgv_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (object.ReferenceEquals(dgv.SortedColumn, colSource))
            {
                e.SortResult = ((ResXSourceType)dgv[colSourceVal.Index, e.RowIndex1].Value).CompareTo((ResXSourceType)dgv[colSourceVal.Index, e.RowIndex2].Value);
                if (e.SortResult == 0)
                {
                    e.SortResult = Convert.ToString(dgv[colKey.Index, e.RowIndex1].Value).CompareTo(Convert.ToString(dgv[colKey.Index, e.RowIndex2].Value));
                    if (e.SortResult == 0)
                    {
                        e.SortResult = Convert.ToString(dgv[colSource.Index, e.RowIndex1].Value).CompareTo(Convert.ToString(dgv[colSource.Index, e.RowIndex2].Value));
                    }
                }
                e.Handled = true;
            }

        }
        #endregion
             
        #endregion

        #region Functions
        #region Add
        #region Row
        private delegate DataGridViewRow delAddRow(string key, object value, string comment, string source, ResXSourceType sourceV);
        private DataGridViewRow AddRow(string key, object value, string comment, string source, ResXSourceType sourceV)
        {
            try
            {
                if (dgv.InvokeRequired)
                {
                    return (DataGridViewRow)dgv.Invoke(new delAddRow(AddRow), new object[] {
                    key,
                    value,
                    comment,
                    source,
                    sourceV
                });
                }
                else
                {
                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(dgv, new object[] {
                    key,
                    value,
                    comment,
                    source,
                    sourceV
                });
                    return r;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion

        #region Rows
        private delegate void delAddRows(List<DataGridViewRow> rows);
        private void AddRows(List<DataGridViewRow> rows)
        {
            try
            {
                if (dgv.InvokeRequired)
                {
                    dgv.Invoke(new delAddRows(AddRows), new object[] { rows });
                }
                else
                {
                    dgv.Rows.AddRange(rows.ToArray());
                }

            }
            catch (Exception)
            {
            }

        }
        #endregion
        #endregion

        #region Enable Controls
        private delegate void delEnableControls(bool enable);

        private void EnableControls(bool enable)
        {
            if (InvokeRequired)
            {
                Invoke(new delEnableControls(EnableControls), enable);
            }
            else
            {
                dgv.Enabled = enable;
                btnRestart.Enabled = enable;
                btnSave.Enabled = enable;
            }

        }
        #endregion


        #endregion
    }
}
