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
        public LexemType Find(string lexem)
        {
            int state=0;
            if (lexem == "правда")
            {
                return LexemType.Logic;
            }
            if (lexem == "брехня")
            {
                return LexemType.Logic;
            }
            if (Regex.IsMatch(lexem[0].ToString(), "[\"]"))
                if (Regex.IsMatch(lexem[lexem.Length - 1].ToString(), "[\"]"))
                {
                    for (int i = 1; i < lexem.Length - 1; i++)
                    {
                        if (Regex.IsMatch(lexem[i].ToString(), "[\"]"))
                            return LexemType.Error;
                    }
                    return LexemType.Symbol;
                }
            if (Regex.IsMatch(lexem[0].ToString(), "[^0-9]"))
                return LexemType.Error;
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
                return LexemType.Int;
            if (state == 1)
                return LexemType.Double;
            return LexemType.Error;
        }
        public void Add(Lexem lit)
        {
            _literals.Add(lit);
        }
    }
}
