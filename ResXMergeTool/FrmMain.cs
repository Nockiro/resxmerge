using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResXMergeTool
{
    public partial class FrmMain : Form
    {
        Dictionary<ResXSourceType, String> chosenFiles = new Dictionary<ResXSourceType, string>();
        String savePath;
        
        public FrmMain()
        {
            // if it's no GUI, the basic checks for correct parameters were already done in Program.cs
            if (Program.CmdLineParametersUsed)
                new FrmResXDifferences(new String[] { Environment.GetCommandLineArgs()[1], Environment.GetCommandLineArgs()[2], Environment.GetCommandLineArgs()[3] },
                    Environment.GetCommandLineArgs()[5]).Show();
            else
                InitializeComponent();
        }

        private void btn_file_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
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
            btn_fileOne.Text = String.Join(", ", chosenFiles.Select(file => Path.GetFileName(file.Value)).ToArray());
        }

        private void btn_selectOutput_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.ShowDialog();
                savePath = ofd.FileName;
            }
        }

        private void btn_startDiff_Click(object sender, EventArgs e)
        {
            new FrmResXDifferences(chosenFiles.Values.ToArray(), savePath).ShowDialog();
        }
    }
}
