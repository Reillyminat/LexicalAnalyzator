using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    class IdentifierTable
    {
        List<Identifier> _idents;

        public IdentifierTable()
        {
            _idents = new List<Identifier>();

        }

        public int Count { get => _idents.Count; }

        public void Add(Identifier ident)
        {
            _idents.Add(ident);
        }

        public bool Find(string name)
        {
           Identifier found = _idents.Find(x => x.Name == name);

            if (found == null)
                return false;
            else
                return true;
        }
        public void SaveToFile()
        {

        }
    }
}
