using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    public enum LexemAnalyzerState { OK, Error}

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

        public LexemAnalyzerState Tokenizer()
        {
            int _codeLength = _code.Length;
            int _lexemBegin = 0;
            int _forward = 0;

            string _subString = "";

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
                        _lexemTable.Add(new Lexem(_subString, LexemType.SimpleType, _lexemBegin, 0));

                        _lexemBegin += _forward;

                    }

                    if (_keyWordsTable.Find(_subString))
                    {
                        _lexemTable.Add(new Lexem(_subString, LexemType.KeyWord, _lexemBegin, 0));

                        _lexemBegin += _forward;
                    }

                    if(_operationTable.Find(_subString))
                    {
                        _lexemTable.Add(new Lexem(_subString, LexemType.KeyWord, _lexemBegin, 0));

                        _lexemBegin += _forward;
                    }

                    if (IdentTable.Find(_subString))
                    {
                        _lexemBegin += _forward;
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

            return LexemAnalyzerState.OK;
        }

        //Method count '\n' before lexem
        private int CountLexemLinePosition(string code, int posEnd)
        {
            int count = 0;

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == '\n')
                    count++;

                if (i == posEnd)
                    break;
            }

            return count;
        }


        
    }
}