using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace LexicalAnalyzerApplication
{
    public class IdentifierTable
    {
        List<Identifier> _idents;

        public enum IdentifierType { SimpleType, Structure, Array }
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
            {
                if (Regex.IsMatch(name[0].ToString(), @"[a-zA-Z_]"))
                {
                    for (int i = 1; i < name.Length - 1; i++)
                    {
                        if (!Regex.IsMatch(name[i].ToString(), @"[a-zA-Z0-9_]"))
                            return false;
                    }
                return true;
                }
                return false;
            }
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
