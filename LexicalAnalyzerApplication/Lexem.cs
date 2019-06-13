namespace LexicalAnalyzerApplication
{
    public enum LexemKind { Identifier, KeyWord, Delimeter, Operation, Literal, Delimiter }

    public enum LexemType { Int, Double, Logic, Symbol, Error }

    public class Lexem
    {
        LexemKind _lexemKind;
        LexemType _lexemType;
        int _codePosition;
        int _lineNumber;
        string _name;
        int _subClass;

        public int LineNumber { get => _lineNumber; set => _lineNumber = value; }
        public int CodePosition { get => _codePosition; set => _codePosition = value; }
        internal LexemKind LexemKind { get => _lexemKind; set => _lexemKind = value; }
        public int SubClass { get => _subClass; set => _subClass = value; }
        public string Name { get => _name; set => _name = value; }
        public Lexem()
        {
            _codePosition = -1;
            _lineNumber = -1;
            _name = "";
        }

        public Lexem(string name, LexemKind lexemKind, int codePosition, int lineNumber, int subClass)
        {
            _lexemKind = lexemKind;
            _codePosition = codePosition;
            _lineNumber = lineNumber;
            _name = name;
            _subClass = subClass;
        }

        public Lexem(string name, LexemKind lexemKind, int codePosition, int lineNumber, int subClass, LexemType lexemType)
        {
            _lexemKind = lexemKind;
            _codePosition = codePosition;
            _lineNumber = lineNumber;
            _name = name;
            _lexemType = lexemType;
            _subClass = subClass;
        }
    }
}
