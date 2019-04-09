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
        IdentifierType _type_number;
        int _code_position;
        int _line_number;
        int _func_descriptor;
        int _struct_descriptor;
        List<string> _func_parameters;
        List<int> _func_types;
        public Identifier()
        {
            _name = "";
            _type_number = IdentifierType.Error;
            _code_position = -1;
            _line_number = -1;
        }

        public Identifier(string name, IdentifierType type_number, int code_position, int line_number)
        {
            _name = name;
            _type_number = type_number;
            _code_position = code_position;
            _line_number = line_number;
        }
        public int LineNumber { get => _line_number;}
        public int CodePosition { get => _code_position;  }
        public IdentifierType TypeNumber { get => _type_number;}
        public string Name { get => _name;  }

    }
}
