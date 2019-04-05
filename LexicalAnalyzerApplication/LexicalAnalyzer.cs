using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    class LexicalAnalyzer
    {
        TypeTabel _typeTable;
        IdentifierTable _identTable;
        KeyWordsTable _keyWordsTable;
        OperationTable _operationTable;
        DelimiterTable _delimiterTable;
        LiteralTable _literalTable;
        LexemTable _lexemTable;

        string _code;
        int _lexemBegin;
        int _forward;
        int _codeLength;
        string _subString;

        public LexicalAnalyzer()
        {
            _typeTable = new TypeTabel();
            _identTable = new IdentifierTable();
            _keyWordsTable = new KeyWordsTable();
            _operationTable = new OperationTable();
            _delimiterTable = new DelimiterTable();
            _literalTable = new LiteralTable();
            _lexemTable = new LexemTable();
        }
        public void SetCode(string code)
        {
            _code = code;
            _codeLength = _code.Length;
            _lexemBegin = 0;
            _forward = 0;
            _subString = "";
        }
        public int Tokenizer()
        {
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
                        _typeTable.SaveToFile();
                        return 3;
                    }
                    if (_keyWordsTable.Find(_subString))
                    {
                        _lexemBegin += _forward;
                        return 4;
                    }
                    if (_identTable.Find(_subString))
                    {
                        _lexemBegin += _forward;
                        _identTable.SaveToFile();
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