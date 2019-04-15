using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace LexicalAnalyzerApplication
{
    public enum IdentifierKind { SimpleType, Function, Array, Structure, Error }
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

        public Identifier FindIdent(string name)
        {
            return _idents.Find(x => x.Name == name);
        }

        public bool Existance(string name, ref int subClass)
        {
            Identifier found = _idents.Find(x => x.Name == name);
            if (found == null)
                return false;
            else
            {
                subClass = _idents.FindIndex(x => x.Name.Equals(name));
                return true;
            }
        }
        public bool Find(string name)
        {
            if (Regex.IsMatch(name[0].ToString(), @"[a-zA-ZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯабвгґдеєжзиіїйклмнопрстуфхцчшщьюя_]"))
            {
                for (int i = 1; i < name.Length - 1; i++)
                {
                    if (!Regex.IsMatch(name[i].ToString(), @"[a-zA-ZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯабвгґдеєжзиіїйклмнопрстуфхцчшщьюя0-9_]"))
                        return false;
                }
                return true;
            }
            return false;
        }

        public void SaveToFile()
        {
            using (FileStream fs = new FileStream(@"IdentifierTable.txt", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                int i = 0;
                sw.WriteLine("{0,-3} {1,-10} {2,-10} {3,-10} {4,-10}\n", "№","Name", "Position", "Line", "Repeats");
                foreach (Identifier id in _idents)
                {
                    i++;
                    string[] array = id.LineNumber.Select(n => n.ToString()).ToArray();
                    string res=string.Join(", ", array);
                    sw.WriteLine("{0,-3} {1,-10} {2,-10} {3,-10} {4,-10} ", i, id.Name, id.CodePosition, res, id.NumberOfRepeats);
                }
                sw.WriteLine();
            }
        }
    }
}