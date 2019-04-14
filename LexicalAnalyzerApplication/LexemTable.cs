using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LexicalAnalyzerApplication
{
    public class LexemTable
    {
        List<Lexem> _lexems;

        public LexemTable()
        {
            _lexems = new List<Lexem>();
        }

        public bool Find(Lexem lexem)
        {
            var answer = _lexems.Find(x=> lexem.LexemType == x.LexemType);

            if (answer != null)
                return true;
            else
                return false;

        }

        public void Add(Lexem lexem)
        {
            _lexems.Add(lexem);
        }

        public void SaveToFile()
        {
            using (FileStream fs = new FileStream(@"LexemTable.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}\n", "Name", "Kind", "Type", "Position", "Line");
                foreach (Lexem lex in _lexems)
                    sw.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10}", lex.Name, lex.LexemKind, lex.LexemType, lex.CodePosition, lex.LineNumber);
                sw.WriteLine();
            }
        }
    }
}
