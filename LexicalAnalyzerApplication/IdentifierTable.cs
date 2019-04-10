using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace LexicalAnalyzerApplication
{
    public enum IdentifierType { SimpleType, Function, Array, Structure, Error }
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
        //Метод для определение типа
        public IdentifierType IdentifyType(string chr)
        {
            //Нужна проверка на структуру, мб проверить, являлась ли предыдущая лексема ключевым словом структура
            if (chr == "[")
            {
                return IdentifierType.Array;
            }
            if (chr == "(")
            {
                return IdentifierType.Function;
            }
            return IdentifierType.SimpleType;
        }
        public bool Find(string name)
        {
           Identifier found = _idents.Find(x => x.Name == name);

            if (found == null)
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
