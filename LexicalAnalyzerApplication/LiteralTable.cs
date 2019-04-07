using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LexicalAnalyzerApplication
{
    public class LiteralTable
    {
        List<Lexem> _literals;
        LexemType _lexemType;
        public enum LexemType { Int, Double, Logic, Symbol }

        public LiteralTable()
        {
            _literals = new List <Lexem>();
        }
        public void Find(string lexem)
        {
            bool state=true;
            foreach (char t in lexem)
            {
                if (!Regex.IsMatch(t.ToString(), @"[^0-9]"))
                {
                    state = false;
                    break;
                }
            }
            if (state)
                _lexemType = LexemType.Int;

            if(Regex.IsMatch(lexem[0].ToString(), @"[^a-zA-z]"))

        }
    }
}
