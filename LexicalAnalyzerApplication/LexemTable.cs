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
            using (FileStream fs = new FileStream(@"LexemTable.txt", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                int i = 0;
                sw.WriteLine("{0,-3} {1,-10} {2,-10} {3,-10} {4,-10}\n", "№","Name", "Line", "Position", "SubClass");
                foreach (Lexem lex in _lexems)
                {
                    i++;
                    sw.WriteLine("{0,-3} {1,-10} {2,-10} {3,-10} {4,-10} {5,10}", i, lex.Name, lex.LineNumber,  lex.CodePosition, lex.LexemType, lex.SubClass);
                }
                sw.WriteLine();
            }
        }
    }
}
