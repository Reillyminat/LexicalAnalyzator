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
        IdentifierKind _kind_number;
        LexemType _lexemType;
        int _code_position;
        int _line_number;
        int _func_descriptor;
        int _struct_descriptor;
        List<string> _func_parameters;
        List<int> _func_types;
        public Identifier()
        {
            _name = "";
            _kind_number = IdentifierKind.Error;
            _lexemType = LexemType.Error;
            _code_position = -1;
            _line_number = -1;
        }

        public Identifier(string name, IdentifierKind kind_number, int code_position, int line_number)
        {
            _name = name;
            _kind_number = kind_number;
            _code_position = code_position;
            _line_number = line_number;
            _lexemType = LexemType.Error;
        }
        public int LineNumber { get => _line_number;}
        public int CodePosition { get => _code_position;  }
        public IdentifierKind KindNumber { get => _kind_number; }
        public string Name { get => _name;  }

    }
}
