using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace Japanese_Helper
{
    public partial class MainWindow : Form
    {
        private static List<string> foundPhrases;
        public MainWindow()
        {
            InitializeComponent();

        }
        private void SetFont(Font font)
        {
            ParagraphsRichTextBox.Font = font;
            Settings.SaveConfig();
        }
        private void Init()
        {
            MinimumSize = Size;
            CenterToScreen();
            InputTextBox.Text = "";
            Settings.LoadPathFromFile();
            if (Config.font == null)
            {
                Config.font = Font;
            }
            SetFont(Config.font);
            if (Config.Path == "")
            {
                MessageBox.Show("Search path has not been set!", "Warning!");
            }
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (Config.Path == "")
            {
                MessageBox.Show("Search path has not been set!", "Error!");
                return;
            }

            using (var window = new WaitToFinishWindow())
            {
                foundPhrases = window.OpenWindow(this, InputTextBox.Text);
            }
            ParagraphsRichTextBox.Clear();
            foreach (var phrase in foundPhrases)
            {
                ParagraphsRichTextBox.AppendText(phrase + '\n');
            }
        }

        private void SearchButton_TextChanged(object sender, EventArgs e)
        {

        }

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            SearchButton.Enabled = !(textbox.Text == "");
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var font = new FontDialog())
            {
                if (font.ShowDialog(this) == DialogResult.OK)
                {
                    Config.font = font.Font;
                    SetFont(font.Font);
                }
            }
        }

        private void changePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dir = new FolderBrowserDialog())
            {
                dir.SelectedPath = Config.Path;
                if (dir.ShowDialog(this) == DialogResult.OK)
                {
                    Config.Path = dir.SelectedPath;
                    Settings.SaveConfig();
                }
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (foundPhrases == null)
            {
                return;
            }
            using (var save = new SaveFileDialog())
            {
                save.Filter = "txt files(*.txt)| *.txt|DocX files(*.docx)| *.docx";
                if (save.ShowDialog(this) == DialogResult.OK)
                {
                    switch (Path.GetExtension(save.FileName))
                    {
                        case ".txt":
                            using (var txt = new StreamWriter(save.FileName, false))
                            {
                                foreach (var p in foundPhrases)
                                {
                                    txt.WriteLine(p);
                                }
                            }
                            break;
                        case ".docx":
                            DocXManager.SaveToDocx(save.FileName);
                            break;
                    }

                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
