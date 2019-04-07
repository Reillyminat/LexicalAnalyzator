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
            _types.Add("ціле");
            _types.Add("дійсне");
            _types.Add("логіка");
            _types.Add("символ");
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
    }
}
