using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Japanese_Helper
{
    public partial class WaitToFinishWindow : Form
    {
        private Thread thread;
        private volatile string _keyword;
        private static List<string> list = new List<string>();
        public WaitToFinishWindow()
        {
            InitializeComponent();
            ControlBox = false;
        }
        private void SearchThread()
        {
            list = DocXManager.FindSentences(Config.Path, _keyword);
            Invoke((MethodInvoker)delegate
            {
                DialogResult = DialogResult.OK;
            }
            );

        }
        public List<string> OpenWindow(IWin32Window owner, string keyword)
        {
            _keyword = keyword;
            ShowDialog(owner);
            return list;
        }

        private void WaitToFinishWindow_Load(object sender, EventArgs e)
        {
            thread = new Thread(SearchThread)
            {
                IsBackground = true
            };
            thread.Start();
            CenterToParent();
        }
    }
}
