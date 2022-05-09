using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace TestSigningBird
{
    internal class SelectionForm : Form
    {
        public Button btnTestModeOn;
        public Button btnTestModeOff;
        public SelectionForm()
        {
            this.Text = "TestSigning On/Off";

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectionForm));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            this.Width = 300;
            this.Height = 200;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;


            btnTestModeOn = new Button();
            btnTestModeOn.Text = "Windowsをテストモードに設定します。\n(★再起動★を伴います)";
            btnTestModeOn.Top = 20;
            btnTestModeOn.Height = 60;
            btnTestModeOn.Width = 260;
            btnTestModeOn.Left = (this.ClientSize.Width - btnTestModeOn.Width) / 2;
            btnTestModeOn.Click += new EventHandler(this.btnTestModeOn_Click);

            btnTestModeOff = new Button();
            btnTestModeOff.Text = "Windowsを通常モードへ戻します。\n(★再起動★を伴います)";
            btnTestModeOff.Top = 85;
            btnTestModeOff.Height = 60;
            btnTestModeOff.Width = 260;
            btnTestModeOff.Left = (this.ClientSize.Width - this.btnTestModeOn.Width) / 2;
            btnTestModeOff.Click += new EventHandler(this.btnTestModeOff_Click);

            this.Controls.Add(btnTestModeOn);
            this.Controls.Add(btnTestModeOff);
        }

        public void btnTestModeOn_Click(object o, EventArgs e)
        {
            string path;
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                path = "Sysnative\\cmd.exe";
            }
            else
            {
                path = "System32\\cmd.exe";
            }
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), path);
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = "/c bcdedit /set testsigning on";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Verb = "RunAs";
            process.Start();
            string text = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            if (text.Contains("正しく"))
            {
                MessageBox.Show(text + "\nWindowsを再起動します。");
                this.WindowsReboot();
            }
            else
            {
                MessageBox.Show(text);
            }
        }
        public void btnTestModeOff_Click(object o, EventArgs e)
        {
            string path;
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                path = "Sysnative\\cmd.exe";
            }
            else
            {
                path = "System32\\cmd.exe";
            }
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), path);
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = "/c bcdedit /set testsigning off";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Verb = "RunAs";
            process.Start();
            string text = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            if (text.Contains("正しく"))
            {
                MessageBox.Show(text + "\nWindowsを再起動します。");
                this.WindowsReboot();
            }
            else
            {
                MessageBox.Show(text);
            }
        }

        public void WindowsReboot()
        {
            Process process = new Process();
            process.StartInfo.FileName = "shutdown.exe";
            process.StartInfo.Arguments = "-r -t 0";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
        }
    }
}
