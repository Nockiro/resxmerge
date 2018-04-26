using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace ResXMergeTool
{
    public partial class FrmResXDifferences : Form
    {
        String[] FilesToDiff;

        public FrmResXDifferences(String[] filesToDiff = null)
        {
            InitializeComponent();

            chkAutoRemoveBaseOnly.Checked = Properties.Settings.Default.AutoRemoveBaseOnly;

            if (filesToDiff != null)
            {
                FilesToDiff = filesToDiff;
                bw.RunWorkerAsync();
            }
            else btn_addFiles_Click(null, null);
        }

        #region Background Worker
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            EnableControls(false);

            try
            {
                FileParser fileParser = new FileParser(Directory.GetCurrentDirectory());
                fileParser.ParseResXFiles(FilesToDiff);

                foreach (ResXSourceNode n in fileParser.OriginNodes.Values)
                    rows.Add(AddRow(n.Node.Name, n.Node.GetValue((ITypeResolutionService)null), n.Node.Comment, ResXSourceNode.GetStringFromEnum(n.Source), n.Source));

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

        #region Event Handlers
        #region Buttons
        private void btnCancel_Click(object sender, EventArgs e) => Program.StartExternalMergeTool();

        private void btnRestart_Click(object sender, EventArgs e) => reInitDGV();

        private void btn_deleteEntry_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dgv.SelectedRows)
                if (!item.IsNewRow)
                    dgv.Rows.RemoveAt(item.Index);
        }

        private void btn_addFiles_Click(object sender, EventArgs e)
        {
            Dictionary<ResXSourceType, String> chosenFiles = new Dictionary<ResXSourceType, string>();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Title = "Select all compared files";

                ofd.ShowDialog();
                foreach (String filePath in ofd.FileNames)
                {
                    ResXSourceType sourceType = FileParser.GetResXSourceTypeFromFileName(filePath);
                    if (chosenFiles.ContainsKey(sourceType))
                        chosenFiles[sourceType] = filePath;
                    else
                        chosenFiles.Add(sourceType, filePath);
                }
            }
            FilesToDiff = chosenFiles.Values.ToArray();

            if (FilesToDiff?.Length > 0) reInitDGV();
        }

        #region Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                if (FilesToDiff?.Length > 0)
                    sfd.InitialDirectory = Path.GetDirectoryName(FilesToDiff[0]);

                sfd.Filter = "Resource files|*.resx|All files|*.*";
                sfd.ShowDialog();

                if (sfd.FileName != "")
                {
                    String savePath = sfd.FileName;
                    dgv.Cursor = Cursors.WaitCursor;

                    try
                    {
                        if (dgv.IsCurrentCellDirty | dgv.IsCurrentRowDirty)
                            dgv.EndEdit();

                        dgv.Sort(colKey, ListSortDirection.Ascending);

                        ResXResourceWriter resX = new ResXResourceWriter(Path.Combine(Directory.GetCurrentDirectory(), savePath));

                        for (int i = 0; i <= dgv.RowCount - 1; i++)
                        {
                            if (dgv.Rows[i].IsNewRow)
                                continue;
                            if (chkAutoRemoveBaseOnly.Checked && ((ResXSourceType)dgv.Rows[i].Cells[colSourceVal.Index].Value) == ResXSourceType.BASE)
                                continue;

                            ResXDataNode n = new ResXDataNode(Convert.ToString(dgv[colKey.Index, i].Value), dgv[colValue.Index, i].Value)
                            {
                                Comment = Convert.ToString(dgv[colComment.Index, i].Value)
                            };
                            resX.AddResource(n);
                        }
                        resX.Generate();
                        resX.Close();

                        Properties.Settings.Default.AutoRemoveBaseOnly = chkAutoRemoveBaseOnly.Checked;
                        Properties.Settings.Default.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"The ResX file '{savePath}' failed to save properly: " + ex.Message);
                    }
                }
            }
            EnableControls(true);
        }
        #endregion

        private void lbl_caption_Click(object sender, EventArgs e) => Process.Start("https://github.com/Nockiro/resxmerge");
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

        private void dgv_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            btn_deleteEntry.Enabled = e.StateChanged == DataGridViewElementStates.Selected;
            btn_deleteEntry.Text = dgv.SelectedRows.Count > 1 ? "Delete entries" : "Delete entry";
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
                    return (DataGridViewRow)dgv.Invoke(new delAddRow(AddRow), new object[] { key, value, comment, source, sourceV });
                else
                {
                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(dgv, new object[] { key, value, comment, source, sourceV });
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
                    dgv.Invoke(new delAddRows(AddRows), new object[] { rows });
                else
                    dgv.Rows.AddRange(rows.ToArray());

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
                Invoke(new delEnableControls(EnableControls), enable);
            else
                dgv.Enabled = btn_addFiles.Enabled = btn_reread.Enabled = btn_saveOutput.Enabled = btn_reread.Enabled = enable;

        }
        #endregion

        #region Reinitialize datagrid

        private void reInitDGV()
        {
            dgv.Rows.Clear();
            bw.RunWorkerAsync();
        }
        #endregion

        #endregion


    }
}
