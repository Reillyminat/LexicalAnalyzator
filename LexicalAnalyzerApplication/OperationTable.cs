using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace LexicalAnalyzerApplication
{
    class OperationTable
    {
        List<string> _operations;

        public OperationTable()
        {
            _operations = new List<string>();

            _operations.Add("+");
            _operations.Add("-");
            _operations.Add("*");
            _operations.Add("/");
            _operations.Add("=");
            _operations.Add("!=");
            _operations.Add("&&");
            _operations.Add("||");
            _operations.Add("<");
            _operations.Add(">");
            _operations.Add("==");
            _operations.Add("<=");
            _operations.Add(">=");
        }

        public int Count { get => _operations.Count; }

        public bool Find(string str, ref int subClass)
        {
            string foud_name = _operations.Find(x => x == str);

            if (foud_name == null)
                return false;
            else
            {
                subClass=_operations.IndexOf(str);
                return true;
            }
        }

    }
}
