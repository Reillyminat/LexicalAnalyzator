using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace LexicalAnalyzerApplication
{
    public class IdentifierTable
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
            using (FileStream fs = new FileStream(@"D:\IdentifierTable.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("{0,10} {1,10} {2,10} {3,10}\n", "Name", "Type", "Position", "Line");
                foreach (Identifier id in _idents)
                    sw.WriteLine("{0,10} {1,10} {2,10} {3,10}",id.Name,id.TypeNumber,id.CodePosition,id.LineNumber);
            }
        }
    }
}
