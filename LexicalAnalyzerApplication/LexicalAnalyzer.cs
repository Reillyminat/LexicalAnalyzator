using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    public enum LexemAnalyzerState { OK, SizeError, IdentifyError, EOF}

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
                if (_code.Length == _lexemBegin + _forward)
                    return LexemAnalyzerState.EOF;
                string chr = _code[_lexemBegin + _forward].ToString();

                foundDelimiter = _delimiterTable.Find(chr);

                if (foundDelimiter)
                {
                   //Count '\n' before lexem
                    int lexemLinePositon = CountLexemLinePosition(_lexemBegin);

                    if (_typeTable.Find(_subString))
                    { 
                        _lexemTable.Add(new Lexem(_subString, LexemType.SimpleType, _lexemBegin, lexemLinePositon));
                        return LexemAnalyzerState.OK;
                    }

                    if (_keyWordsTable.Find(_subString))
                    {
                        _lexemTable.Add(new Lexem(_subString, LexemType.KeyWord, _lexemBegin, lexemLinePositon));
                        return LexemAnalyzerState.OK;
                    }

                    if(_operationTable.Find(_subString))
                    {
                        _lexemTable.Add(new Lexem(_subString, LexemType.KeyWord, _lexemBegin, lexemLinePositon));
                        return LexemAnalyzerState.OK;
                    }

                    if (IdentTable.Find(_subString))
                    {
                        _identTable.Add(new Identifier());
                        return LexemAnalyzerState.OK;
                    }

                    _lexemBegin += _forward;
                }
                else
                {
                    _subString = _subString + _code[_lexemBegin + _forward];
                }

                if (_subString.Length > 100)
                    return LexemAnalyzerState.SizeError;

                _forward += 1;
            }
            while (!foundDelimiter);
            return LexemAnalyzerState.IdentifyError;
        }

        //Method count '\n' before lexem
        private int CountLexemLinePosition(int posEnd)
        {
            int count = 0;

            for (int i = 0; i < _code.Length; i++)
            {
                if (_code[i] == '\n')
                    count++;
                if (i == posEnd)
                    break;
            }

            return count;
        }

        public void Save_ID()
        {
            _identTable.SaveToFile();
        }

        public void Save_L()
        {
            _lexemTable.SaveToFile();
        }
    }
}