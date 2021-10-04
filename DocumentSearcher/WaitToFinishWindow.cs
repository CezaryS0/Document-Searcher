using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Japanese_Helper
{
    public partial class WaitToFinishWindow : Form
    {
        private Thread thread;
        private static List<string> list = new List<string>();
        public WaitToFinishWindow()
        {
            InitializeComponent();
            ControlBox = false;
        }
        private void SetDialogRes()
        {
            Invoke((MethodInvoker)delegate
            {
                DialogResult = DialogResult.OK;
            }
           );
        }
        private void SearchThread(string _keyword)
        {
            list = FileManager.FindPhrases(Config.Path, _keyword);
            SetDialogRes();
        }
        private void SaveThread(List<string> foundPhrases, string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".txt":
                    TXTManager.SaveToTXT(foundPhrases, filename);
                    break;
                case ".docx":
                    DocXManager.SaveToDocx(foundPhrases, filename);
                    break;
            }
            SetDialogRes();
        }
        public List<string> OpenSearchWindow(IWin32Window owner, string keyword)
        {
            thread = new Thread(() => SearchThread(keyword))
            {
                IsBackground = true
            };
            thread.Start();
            ShowDialog(owner);
            return list;
        }
        public void OpenSaveWindow(IWin32Window owner, List<string> foundPhrases, string filename)
        {
            thread = new Thread(() => SaveThread(foundPhrases, filename))
            {
                IsBackground = true
            };
            thread.Start();
            ShowDialog(owner);
        }
        private void WaitToFinishWindow_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
