using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace LexicalAnalyzerApplication
{
    public class LiteralTable
    {
        List<Lexem> _literals;

        public LiteralTable()
        {
            _literals = new List <Lexem>();
        }
        public LiteralType Find(string lexem)
        {
            int state=0;
            if (lexem == "правда")
            {
                return LiteralType.Logic;
            }
            if (lexem == "брехня")
            {
                return LiteralType.Logic;
            }
            if (Regex.IsMatch(lexem[0].ToString(), "[\"]"))
                if (Regex.IsMatch(lexem[lexem.Length - 1].ToString(), "[\"]"))
                {
                    for (int i = 1; i < lexem.Length - 1; i++)
                    {
                        if (Regex.IsMatch(lexem[i].ToString(), "[\"]"))
                            return LiteralType.Error;
                    }
                    return LiteralType.Symbol;
                }
            if (Regex.IsMatch(lexem[0].ToString(), "[^0-9]"))
                return LiteralType.Error;
            bool dotContaining=false;
            foreach (char t in lexem)
            {
                Console.WriteLine(t);
                if (Regex.IsMatch(t.ToString(), "[^0-9]"))
                {
                    if ((t.ToString() == ".") && (dotContaining == false))
                    {
                        dotContaining = true;
                        state = 1;
                    }
                    else
                    {
                        state = 2;
                        break;
                    }
                }
            }
            if (state == 0)
                return LiteralType.Int;
            if (state == 1)
                return LiteralType.Double;
            return LiteralType.Error;
        }
        public void Add(Lexem lit)
        {
            _literals.Add(lit);
        }
        public void SaveToFile()
        {
            using (FileStream fs = new FileStream(@"D:\IdentifierTable.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("{0,10} {1,10} {2,10} {3,10}\n", "Name", "Type", "Position", "Line");
                foreach (Lexem lit in _literals)
                    sw.WriteLine("{0,10} {1,10} {2,10} {3,10}", lit.Name, lit.LexemType, lit.CodePosition, lit.LineNumber);
            }
        }
    }
}
