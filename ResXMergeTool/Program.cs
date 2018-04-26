using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ResXMergeTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // if we're running in console mode and don't get the necessary parameters, switch immediately to an external software
            if (CmdLineParametersUsed && (Environment.GetCommandLineArgs().Length != 6 || !Environment.GetCommandLineArgs()[5].ToLower().EndsWith(".resx")))
            {
                Console.WriteLine("Usage: ResXMergeTool.exe File.resx.BASE[.resx] File.resx.LOCAL[.resx] File.resx.REMOTE[.resx] -o File.resx");
                Console.WriteLine("Start external tool if possible..");
                StartExternalMergeTool();
            }
            else
            {
                Console.WriteLine("Starting GUI.. Don't close this window until you're done!");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
            }
        }

        public static bool CmdLineParametersUsed
        {
            get
            {
                try { return Environment.GetCommandLineArgs().Length > 1; }
                catch { return false; }
            }
        }

        public static void StartExternalMergeTool()
        {
            try
            {
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

            Application.Exit();

        }
    }
}
