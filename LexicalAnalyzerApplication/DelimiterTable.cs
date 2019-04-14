using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    public class DelimiterTable
    {
        List<string> _delimeters;

        public DelimiterTable()
        {
            _delimeters = new List<string>();

            _delimeters.Add("{");
            _delimeters.Add("}");
            _delimeters.Add(" ");
            _delimeters.Add("\n");
            _delimeters.Add("\r");
            _delimeters.Add("\t");
            _delimeters.Add(";");
            _delimeters.Add(",");/*
            _delimeters.Add("[");
            _delimeters.Add("]");*/
        }

        public int Count { get => _delimeters.Count; }

        public bool FindSkip(string str)
        {
            if (str == " " || str == "\n" || str == "\t" || str=="\r")
                return true;
            return false;
        }
        public bool Find(string str)
        {
            string found_name = _delimeters.Find(x => x == str);

            if (found_name == null)
                return false;
            else
                return true;
        }
    }
}
