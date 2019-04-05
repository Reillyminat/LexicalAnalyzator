using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace LexicalAnalyzerApplication
{
    class TypeTabel
    {
        List<string> _types;

        public TypeTabel()
        {
            _types = new List<string>();
            //types
            _types.Add("int");
            _types.Add("char");
            _types.Add("double");
            _types.Add("bool");
            StreamWriter sw = new StreamWriter(@"D:\Работы по программированию\TypesTable.txt", true, Encoding.Default);
            foreach (string i in _types)
            {
                sw.WriteLine(i);
            }
        }

        public int Count { get => _types.Count; }

        public bool Find(string str)
        {
            string found_name = _types.Find(x => x == str);

            if (found_name == null)
                return false;
            else
            {
                _types.Add(found_name);
                return true;
            }
        }

        public void SaveToFile()
        {

        }
    }
}
