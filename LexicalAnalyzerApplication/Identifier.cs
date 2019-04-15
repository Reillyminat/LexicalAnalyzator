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
        int _line_number;
        int _descriptor;
        int _number_of_param;
        List<string> _parameters;
        List<LexemType> _types;
        public Identifier()
        {
            _name = "";
            _code_position = -1;
            _line_number = -1;
        }

        public Identifier(string name, int code_position, int line_number)
        {
            _name = name;
            _code_position = code_position;
            _line_number = line_number;
        }
        public string get() { return _name; }
        public int LineNumber { get => _line_number;}
        public int CodePosition { get => _code_position;  }
        public string Name { get => _name;  }
        public int NumberOfRepeats { get => _number_of_repeats; }
    }
}
