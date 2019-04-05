using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
