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
            toolStripStatusLabel1.Text = "";
            fileContent = string.Empty;
            fileContent = System.IO.File.ReadAllText(@"D:\test.txt", Encoding.Default);
            richTextBoxLineNumbers.Text = "";
            richTextBoxCode.Text = fileContent;
            for (int i = 1; i < richTextBoxCode.Lines.Length + 1; i++)
            {
                richTextBoxLineNumbers.Text = richTextBoxLineNumbers.Text + i.ToString() + "\n";
            }
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
            /* using (OpenFileDialog openFileDialog = new OpenFileDialog())
             {
                 //openFileDialog.InitialDirectory = "d:\\";
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
                 }
             }*/
        }
        private void buttonTranslate_Click(object sender, EventArgs e)
        {
            backStack.Push(richTextBoxCode.Text);

            lexicalAnalyzer = new LexicalAnalyzer();
            LexemAnalyzerState state = LexemAnalyzerState.OK; ;
            lexicalAnalyzer.SetCode(richTextBoxCode.Text);
            bool finish=true;
            int line=0;
            int position=1;
            while (finish)
            {
                state = lexicalAnalyzer.Tokenizer(ref line, ref position);
                switch (state)
                {
                    case LexemAnalyzerState.SizeError:
                        MessageBox.Show("Ошибка лексического анализа. Слишком длинная лексема.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        finish = false;
                        break;
                    case LexemAnalyzerState.IdentifyError:
                        MessageBox.Show("Ошибка лексического анализа. Невозможно распознать лексему в строке "+ ++line, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        finish = false;
                        break;
                    case LexemAnalyzerState.OK:
                        break;
                    case LexemAnalyzerState.EOF:
                        finish = false;
                        break;
                }
            }
            if (state == LexemAnalyzerState.EOF)
            {
                SA sA = new SA(lexicalAnalyzer);
                sA.ReadFromFile();
                line = 0;
                position = 0;
                finish = true;
                while (finish)
                {
                    switch (sA.Parse(ref line, ref position))
                    {
                        case ParsingState.IdentifyError:
                            toolStripStatusLabel1.Text = "Ошибка синтаксического анализа. Невозможно распознать лексему " + lexicalAnalyzer.LexemTable.Lexems[position].Name +
                                " (строка " + lexicalAnalyzer.LexemTable.Lexems[position].LineNumber + " , позиция " + lexicalAnalyzer.LexemTable.Lexems[position].CodePosition + " ) в состоянии " + line;
                            finish = false;
                            break;
                        case ParsingState.OK:
                            toolStripStatusLabel1.Text = "Синтаксический анализ прошёл успешно";
                            finish = false;
                            break;
                    }
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
    }
}
