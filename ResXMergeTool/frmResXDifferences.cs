using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace ResXMergeTool
{
    public partial class frmResXDifferences : Form
    {

        #region Private Variables
        private bool mblnRestart = false;
        private string mstrPath = string.Empty;
        private SortedList<string, ResXConflictNode> mConflicts = new SortedList<string, ResXConflictNode>();

        private SortedList<string, ResXSourceNode> mOutput = new SortedList<string, ResXSourceNode>();
        #endregion

        #region Event Handlers
        #region Background Worker
        private void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>(10000);

            EnableControls(false);

            if (!mblnRestart)
            {
                if (bw.CancellationPending) return;
                ParseResXFile(Environment.GetCommandLineArgs()[1]);
                if (bw.CancellationPending) return;
                ParseResXFile(Environment.GetCommandLineArgs()[2]);
                if (bw.CancellationPending) return;
                ParseResXFile(Environment.GetCommandLineArgs()[3]);
            }

            foreach (ResXSourceNode n in mOutput.Values)
            {
                if (bw.CancellationPending) return;
                rows.Add(AddRow(n.Node.Name, n.Node.GetValue((ITypeResolutionService)null), n.Node.Comment, GetSourceString(n.Source), n.Source));
            }

            foreach (ResXConflictNode n in mConflicts.Values)
            {
                if (bw.CancellationPending) return;
                if (n.BaseNode != null) rows.Add(AddRow(n.BaseNode.Name, n.BaseNode.GetValue((ITypeResolutionService)null), n.BaseNode.Comment, "XCONFLICT - BASE", ResXSource.CONFLICT));
                if (n.LocalNode != null) rows.Add(AddRow(n.LocalNode.Name, n.LocalNode.GetValue((ITypeResolutionService)null), n.LocalNode.Comment, "XCONFLICT - LOCAL", ResXSource.CONFLICT));
                if (n.RemoteNode != null) rows.Add(AddRow(n.RemoteNode.Name, n.RemoteNode.GetValue((ITypeResolutionService)null), n.RemoteNode.Comment, "XCONFLICT - REMOTE", ResXSource.CONFLICT));
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
        #region Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            StartExternalMergeTool();
        }
        #endregion

        #region Restart
        private void btnRestart_Click(object sender, EventArgs e)
        {
            mblnRestart = true;
            dgv.Rows.Clear();
            bw.RunWorkerAsync();

        }
        #endregion

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

                resX = new ResXResourceWriter(System.IO.Path.Combine(mstrPath, Environment.GetCommandLineArgs()[5]));

                for (int i = 0; i <= dgv.RowCount - 1; i++)
                {
                    if (dgv.Rows[i].IsNewRow)
                        continue;
                    if (chkAutoRemoveBaseOnly.Checked && ((ResXSource)dgv.Rows[i].Cells[colSourceVal.Index].Value) == ResXSource.BASE)
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
            e.Row.Cells[colSourceVal.Index].Value = ResXSource.MANUAL;
            e.Row.Cells[colValue.Index].Value = String.Empty;
        }

        private void dgv_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (object.ReferenceEquals(dgv.SortedColumn, colSource))
            {
                e.SortResult = ((ResXSource)dgv[colSourceVal.Index, e.RowIndex1].Value).CompareTo((ResXSource)dgv[colSourceVal.Index, e.RowIndex2].Value);
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

        #region Form
        public frmResXDifferences()
        {
            InitializeComponent();
            chkAutoRemoveBaseOnly.Checked = Properties.Settings.Default.AutoRemoveBaseOnly;
        }

        private void frmResXDifferences_Load(object sender, EventArgs e)
        {

            if (Environment.GetCommandLineArgs().Length != 6 || !Environment.GetCommandLineArgs()[5].ToLower().EndsWith(".resx"))
            {
                StartExternalMergeTool();
            }
            else
            {
                mstrPath = System.IO.Directory.GetCurrentDirectory();
                bw.RunWorkerAsync();
            }

        }
        #endregion
        #endregion

        #region Functions
        #region Add
        #region Row
        private delegate DataGridViewRow delAddRow(string key, object value, string comment, string source, ResXSource sourceV);
        private DataGridViewRow AddRow(string key, object value, string comment, string source, ResXSource sourceV)
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

        #region Get Source String - String
        private string GetSourceString(ResXSource source)
        {

            switch (source)
            {
                case ResXSource.ALL: return "ALL";
                case ResXSource.BASE: return "BASE";
                case ResXSource.CONFLICT: return "XCONFLICT";
                case ResXSource.BASE_LOCAL: return "LOCAL";
                case ResXSource.BASE_REMOTE: return "REMOTE";
                case ResXSource.LOCAL: return "LOCAL ONLY";
                case ResXSource.LOCAL_REMOTE: return "BOTH";
                case ResXSource.MANUAL: return "MANUAL";
                case ResXSource.REMOTE: return "REMOTE ONLY";
                default: return "????";
            }

        }
        #endregion

        #region Parse ResX File
        private void ParseResXFile(string file)
        {
            ResXSource rSource = ResXSource.CONFLICT;
            string name = null;

            try
            {
                if (file.Contains(".resx"))
                {
                    if (file.Contains("base"))
                        rSource = ResXSource.BASE;
                    else if (file.Contains("local"))
                        rSource = ResXSource.LOCAL;
                    else if(file.Contains("remote"))
                        rSource = ResXSource.REMOTE;
                }
                else return;


                Directory.SetCurrentDirectory(Path.GetDirectoryName(file));
                if (bw.CancellationPending)
                    return;

                ResXResourceReader resx = new ResXResourceReader(file);
                if (bw.CancellationPending)
                    return;

                resx.UseResXDataNodes = true;
                IDictionaryEnumerator dict = resx.GetEnumerator();
                if (bw.CancellationPending)
                    return;

                while (dict.MoveNext())
                {
                    if (bw.CancellationPending)
                        return;
                    ResXDataNode node = (ResXDataNode)dict.Value;
                    if (bw.CancellationPending)
                        return;

                    name = node.Name.ToLower();

                    if (mConflicts.ContainsKey(name))
                    {
                        if (bw.CancellationPending)
                            return;
                        switch (rSource)
                        {
                            case ResXSource.BASE:
                                mConflicts[name].BaseNode = node;
                                break;
                            case ResXSource.LOCAL:
                                mConflicts[name].LocalNode = node;
                                break;
                            case ResXSource.REMOTE:
                                mConflicts[name].RemoteNode = node;
                                break;
                            default:
                                continue;
                        }
                        if (bw.CancellationPending)
                            return;
                    }
                    else
                    {
                        if (bw.CancellationPending)
                            return;
                        if (mOutput.ContainsKey(name))
                        {
                            if (bw.CancellationPending)
                                return;

                            object objA = null;
                            object objB = null;
                            bool objE = false;
                            objA = mOutput[name].Node.GetValue((ITypeResolutionService)null);
                            objB = node.GetValue((ITypeResolutionService)null);

                            objE = string.Equals(objA.GetType().ToString(), objB.GetType().ToString());
                            if (objE && objA is string)
                                objE = string.Equals(objA, objB);

                            if (objE && string.Equals(mOutput[name].Node.Name, node.Name) && string.Equals(mOutput[name].Node.Comment, node.Comment))
                            {
                                if (bw.CancellationPending)
                                    return;
                                mOutput[name].Source = mOutput[name].Source | rSource;
                                if (bw.CancellationPending)
                                    return;
                            }
                            else
                            {
                                if (bw.CancellationPending)
                                    return;
                                switch (mOutput[name].Source)
                                {
                                    case ResXSource.BASE:
                                        mConflicts.Add(name, new ResXConflictNode(mOutput[name].Node, null, null));
                                        break;
                                    case ResXSource.BASE_LOCAL:
                                        mConflicts.Add(name, new ResXConflictNode(mOutput[name].Node, mOutput[name].Node, null));
                                        break;
                                    case ResXSource.BASE_REMOTE:
                                        mConflicts.Add(name, new ResXConflictNode(mOutput[name].Node, null, mOutput[name].Node));
                                        break;
                                    case ResXSource.LOCAL:
                                        mConflicts.Add(name, new ResXConflictNode(null, mOutput[name].Node, null));
                                        break;
                                    case ResXSource.LOCAL_REMOTE:
                                        mConflicts.Add(name, new ResXConflictNode(null, mOutput[name].Node, mOutput[name].Node));
                                        break;
                                    case ResXSource.REMOTE:
                                        mConflicts.Add(name, new ResXConflictNode(null, null, mOutput[name].Node));
                                        break;
                                    default:
                                        continue;
                                }
                                if (bw.CancellationPending)
                                    return;

                                switch (rSource)
                                {
                                    case ResXSource.BASE:
                                        mConflicts[name].BaseNode = node;
                                        break;
                                    case ResXSource.LOCAL:
                                        mConflicts[name].LocalNode = node;
                                        break;
                                    case ResXSource.REMOTE:
                                        mConflicts[name].RemoteNode = node;
                                        break;
                                    default:
                                        continue;
                                }

                                if (bw.CancellationPending)
                                    return;
                                mOutput.Remove(name);
                                if (bw.CancellationPending)
                                    return;
                            }
                        }
                        else
                        {
                            if (bw.CancellationPending)
                                return;
                            mOutput.Add(name, new ResXSourceNode(rSource, node));
                            if (bw.CancellationPending)
                                return;
                        }
                    }
                    if (bw.CancellationPending)
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The ResX file '" + file + "' failed to parse properly: " + ex.Message);
            }

        }
        #endregion

        #region Start KDIFF3
        private void StartExternalMergeTool()
        {
            try
            {
                if (bw.IsBusy)
                    bw.CancelAsync();

            }
            catch (Exception)
            {
            }

            try
            {
                this.Hide();
                // Check for KDIFF
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "KDIFF3", "kdiff3.exe")))
                {
                    ProcessStartInfo pinfo = new ProcessStartInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "KDIFF3", "kdiff3.exe"), Environment.CommandLine.Substring(Application.ExecutablePath.Length + 3));
                    pinfo.RedirectStandardOutput = true;
                    pinfo.UseShellExecute = false;

                    Process p = new Process();
                    p.StartInfo = pinfo;

                    p.Start();
                    StreamWriter s = new StreamWriter(Console.OpenStandardOutput());
                    while (!p.StandardOutput.EndOfStream)
                    {
                        s.WriteLine(p.StandardOutput.ReadLine());
                    }
                   
                }
                else  // check for tortoisegit merge
                {
                    string[] args = Environment.GetCommandLineArgs();
                    string param;
                    if (args.Length == 4)
                        param = $"/mine: { args[2] }/theirs: { args[3] }/base: { args[1] } ";
                    else
                        param = $"/mine: { args[1] }/theirs:{ args[2] } ";

                    ProcessStartInfo pinfo = new ProcessStartInfo("tortoisegitmerge.exe", param);
                    pinfo.RedirectStandardOutput = true;
                    pinfo.UseShellExecute = false;

                    Process p = new Process();
                    p.StartInfo = pinfo;

                    p.Start();
                    StreamWriter s = new StreamWriter(Console.OpenStandardOutput());
                    while (!p.StandardOutput.EndOfStream)
                    {
                        s.WriteLine(p.StandardOutput.ReadLine());
                    }
                }
            }
            catch (Exception)
            {
            }
            try
            {
                Application.Exit();

            }
            catch (Exception)
            {
            }

        }
        #endregion

        #endregion
    }
}
