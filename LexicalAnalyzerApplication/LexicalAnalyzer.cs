using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    enum LexemAnaluzerError { OK, Error}

    public class LexicalAnalyzer
    {
        TypeTabel _typeTable;
        IdentifierTable _identTable;
        KeyWordsTable _keyWordsTable;
        OperationTable _operationTable;
        DelimiterTable _delimiterTable;
        LiteralTable _literalTable;
        LexemTable _lexemTable;

        string _code;


        public LexemTable LexemTable { get => _lexemTable; set => _lexemTable = value; }
        public IdentifierTable IdentTable { get => _identTable; set => _identTable = value; }
        public LiteralTable LiteralTable { get => _literalTable; set => _literalTable = value; }


        public LexicalAnalyzer()
        {
            _typeTable = new TypeTabel();
            IdentTable = new IdentifierTable();
            _keyWordsTable = new KeyWordsTable();
            _operationTable = new OperationTable();
            _delimiterTable = new DelimiterTable();
            _literalTable = new LiteralTable();
            _lexemTable = new LexemTable();
        }

        public void SetCode(string code)
        {
            _code = code;
        }

        public int Tokenizer()
        {
            int _codeLength = _code.Length;
            int _lexemBegin = 0;
            int _forward = 0;

            string _subString = "";

            if (_lexemBegin > _codeLength)
                return 1;

            _forward = 0;

            bool foundDelimiter = false;
            do
            {
                string chr = _code[_lexemBegin + _forward].ToString();
                foundDelimiter = _delimiterTable.Find(chr);

                if (foundDelimiter)
                {
                    if (_typeTable.Find(_subString))
                    {
                        _lexemBegin += _forward;
                        return 3;
                    }
                    if (_keyWordsTable.Find(_subString))
                    {
                        _lexemBegin += _forward;
                        return 4;
                    }
                    if (IdentTable.Find(_subString))
                    {
                        _lexemBegin += _forward;
                        IdentTable.SaveToFile();
                        return 5;
                    }
                }
                else
                {
                    _subString = _subString + _code[_lexemBegin + _forward];
                }
                if (_subString.Length > 100)
                    return 0;
                _forward += 1;
            }
            while (!foundDelimiter);
            return 2;
        }

        private bool FindLexemType(string subString)
        {
            return false;
        }


        
    }
}