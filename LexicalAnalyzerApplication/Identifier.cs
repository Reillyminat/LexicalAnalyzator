using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{

    public class Identifier
    {
        string _name;
        int _code_position;
        int _subClass;
        int _number_of_repeats;
        List<int> _line_number;
        int _descriptor;
        int _number_of_param;
        List<string> _parameters;
        List<LexemType> _types;
        public Identifier()
        {
            _name = "";
            _code_position = -1;
            _line_number = null;
            _number_of_repeats=1;
        }

        public Identifier(string name, int code_position, int line_number)
        {
            _name = name;
            _line_number = new List<int>();
            _code_position = code_position;
            _line_number.Add(line_number);
        }
        public string get() { return _name; }
        public List<int> LineNumber { get => _line_number;}
        public int CodePosition { get => _code_position;  }
        public string Name { get => _name;  }
        public int NumberOfRepeats { get => _number_of_repeats; set => _number_of_repeats = value; }
        public void AddLineNumber(int line) { _line_number.Add(line); }
    }
}
