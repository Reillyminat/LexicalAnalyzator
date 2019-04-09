using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
namespace LexicalAnalyzerApplication
{
    public partial class MainWindow : Form
    {
        private string fileContent;
        private LexicalAnalyzer lexicalAnalyzer;
        Stack<string> backStack;
        Stack<string> frontStack;

        public MainWindow()
        {
            InitializeComponent();
            backStack = new Stack<string>();
            frontStack = new Stack<string>();
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;

            fileContent = string.Empty;

            lexicalAnalyzer = new LexicalAnalyzer();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void richTextBoxCode_TextChanged(object sender, EventArgs e)
        {
            richTextBoxLineNumbers.Text = "";

            for (int i = 1; i < richTextBoxCode.Lines.Length + 1; i++)
            {
                richTextBoxLineNumbers.Text = richTextBoxLineNumbers.Text + i.ToString() + "\n";
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var fileContent = string.Empty;
            var filePath = string.Empty;
            filePath = @"C:\Users\illym\test.txt";

            fileContent = System.IO.File.ReadAllText(filePath, Encoding.Default);

            richTextBoxCode.Text = fileContent;
            richTextBoxCode_TextChanged(sender, e);

            /* using (OpenFileDialog openFileDialog = new OpenFileDialog())
             {
                 //openFileDialog.InitialDirectory = "c:\\";
                 openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                 //openFileDialog.RestoreDirectory = true;

                 if (openFileDialog.ShowDialog() == DialogResult.OK)
                 {
                     //Get the path of specified file
                     filePath = openFileDialog.FileName;

                     //Read the contents of the file into a stream
                     var fileStream = openFileDialog.OpenFile();

                     using (StreamReader reader = new StreamReader(fileStream))
                     {
                         fileContent = reader.ReadToEnd();
                     }
                 }*/
        }
        private void buttonTranslate_Click(object sender, EventArgs e)
        {
            backStack.Push(richTextBoxCode.Text);
            lexicalAnalyzer.SetCode(richTextBoxCode.Text);
            bool finish=true;
            while (finish)
            {
                switch (lexicalAnalyzer.Tokenizer())
                {
                    case LexemAnalyzerState.SizeError:
                        MessageBox.Show("Ошибка лексического анализа. Слишком длинная лексема.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        finish = false;
                        break;
                    case LexemAnalyzerState.IdentifyError:
                        MessageBox.Show("Ошибка лексического анализа. Невозможно распознать лексему.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        finish = false;
                        break;
                    case LexemAnalyzerState.OK:
                        break;

                }
            }
        }

        private void richTextBox2_VScroll(object sender, EventArgs e)
        {
            IntPtr ptrLparam = new IntPtr(0);
            IntPtr ptrWparam;
            SCROLLINFO si = new SCROLLINFO();
            si.cbSize = (uint)Marshal.SizeOf(si);
            si.fMask = (uint)ScrollInfoMask.SIF_ALL;
            IntPtr rtb1Handle = richTextBoxCode.Handle;
            User32.GetScrollInfo(rtb1Handle,
                (int)ScrollBarDirection.SB_VERT, ref si);

            IntPtr rtb2Handle = richTextBoxLineNumbers.Handle;
            User32.SetScrollInfo(rtb2Handle, (int)ScrollBarDirection.SB_VERT, ref si, true);

            ptrWparam = new IntPtr((int)ScrollBarCommands.SB_THUMBTRACK + 0x10000 * si.nPos);
            User32.SendMessage(rtb2Handle, User32.WM_VSCROLL, ptrWparam, ptrLparam);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frontStack.Push(richTextBoxCode.Text);
            richTextBoxCode.Text = backStack.Pop();
            toolStripButton2.Enabled = true;
            if (backStack.Count == 0)
                toolStripButton1.Enabled = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            backStack.Push(richTextBoxCode.Text);
            richTextBoxCode.Text = frontStack.Pop();
            toolStripButton1.Enabled = true;
            if (frontStack.Count == 0)
                toolStripButton2.Enabled = false;
        }

        private void lexemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lexicalAnalyzer.Save_Lex();
        }

        private void identifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lexicalAnalyzer.Save_ID();
        }

        private void literalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lexicalAnalyzer.Save_Lit();
        }
    }
}
