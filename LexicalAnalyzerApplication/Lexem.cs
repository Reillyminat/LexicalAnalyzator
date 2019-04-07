using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    public enum LexemType { Identifier, KeyWord, Delimeter, Operation, SimpleType }
    public enum LiteralType { Int, Double, Logic, Symbol, Error }
    public class Lexem
    {
        LexemType _lexemType;
        LiteralType _literalType;
        int _codePosition;
        int _lineNumber;
        string _name;

        public int LineNumber { get => _lineNumber; set => _lineNumber = value; }
        public int CodePosition { get => _codePosition; set => _codePosition = value; }
        internal LexemType LexemType { get => _lexemType; set => _lexemType = value; }
        internal LexemType LiteralType { get => _lexemType; set => _lexemType = value; }
        public string Name { get => _name; set => _name = value; }
        public Lexem()
        {
            _codePosition = -1;
            _lineNumber = -1;
            _name = "";
        }

        public Lexem(string name, LexemType lexemType, int codePosition, int lineNumber)
        {
            _lexemType = LexemType;
            _codePosition = codePosition;
            _lineNumber = lineNumber;
            _name = name;
        }

        public Lexem(string name, LexemType lexemType, int codePosition, int lineNumber, LiteralType literalType)
        {
            _lexemType = LexemType;
            _codePosition = codePosition;
            _lineNumber = lineNumber;
            _name = name;
            _literalType = literalType;
        }
    }
}
