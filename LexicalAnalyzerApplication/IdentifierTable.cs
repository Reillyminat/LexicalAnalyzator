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
        public IdentifierKind IdentifyType(string chr)
        {
            if (chr == "[")
            {
                return IdentifierKind.Array;
            }
            if (chr == "(")
            {
                return IdentifierKind.Function;
            }
            return IdentifierKind.SimpleType;
        }

        public bool Existance(string name)
        {
            Identifier found = _idents.Find(x => x.Name == name);
            
            if (found == null)
                return false;
            else return true;
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
            using (FileStream fs = new FileStream(@"IdentifierTable.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10} {7,-10}\n", "Name", "Kind", "Position", "Line", "Descriptor", "Number_of_par", "Parameters", "Param_types");
                foreach (Identifier id in _idents)
                    sw.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10}",id.Name,id.KindNumber,id.CodePosition,id.LineNumber, 0, id.NumberOfParam);
                sw.WriteLine();
            }
        }
    }
}