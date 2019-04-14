namespace LexicalAnalyzerApplication
{
    public enum LexemKind { Identifier, KeyWord, Delimeter, Operation, SimpleType }

    public enum LexemType { Int, Double, Logic, Symbol, Error }

    public class Lexem
    {
        LexemKind _lexemKind;
        LexemType _lexemType;
        int _codePosition;
        int _lineNumber;
        string _name;

        public int LineNumber { get => _lineNumber; set => _lineNumber = value; }
        public int CodePosition { get => _codePosition; set => _codePosition = value; }
        internal LexemType LexemType { get => _lexemType; set => _lexemType = value; }
        internal LexemKind LexemKind { get => _lexemKind; set => _lexemKind = value; }
        public string Name { get => _name; set => _name = value; }
        public Lexem()
        {
            _codePosition = -1;
            _lineNumber = -1;
            _name = "";
            _lexemType = LexemType.Error;
        }

        public Lexem(string name, LexemKind lexemKind, int codePosition, int lineNumber)
        {
            _lexemKind = lexemKind;
            _codePosition = codePosition;
            _lineNumber = lineNumber;
            _name = name;
            _lexemType = LexemType.Error;
        }

        public Lexem(string name, LexemKind lexemKind, int codePosition, int lineNumber, LexemType lexemType)
        {
            _lexemKind = lexemKind;
            _codePosition = codePosition;
            _lineNumber = lineNumber;
            _name = name;
            _lexemType = lexemType;
        }
    }
}
