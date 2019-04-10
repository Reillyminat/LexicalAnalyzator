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
        int _lexemBegin;
        LiteralType literalType;
        IdentifierType identifierType;
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
            _lexemBegin = 0;
        }

        public void SetCode(string code)
        {
            _code = code;
        }
        
        public LexemAnalyzerState Tokenizer()
        {
            int _forward = 0;
            int lexemLinePositon;
            string _subString = "";
            bool foundDelimiter = false;
            string chr="";
            if (_code.Length == _lexemBegin)
                return LexemAnalyzerState.EOF;
            do
            {
                if ((_lexemBegin + _forward + 1) == _code.Length)
                {
                    foundDelimiter = true;
                    chr = _code[_lexemBegin + _forward].ToString();

                    _subString += chr;
                }
                else
                {
                    chr = _code[_lexemBegin + _forward].ToString();
                    if (chr == "\"")
                    {
                        do
                        {
                            _subString += chr;
                            _forward++;
                            chr = _code[_lexemBegin + _forward].ToString();
                        } while (chr != "\"");
                        _lexemBegin += _forward + 2;
                        literalType = LiteralTable.Find(_subString);
                        lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                        _literalTable.Add(new Lexem(_subString, LexemType.SimpleType, _lexemBegin, lexemLinePositon, literalType));
                        return LexemAnalyzerState.OK;
                    }
                    foundDelimiter = _delimiterTable.Find(chr);
                }
 
                if (foundDelimiter)
                {
                   //Count '\n' before lexem
                    lexemLinePositon = CountLexemLinePosition(_lexemBegin);

                    if (_typeTable.Find(_subString))
                    {
                        _lexemBegin += _forward + 1;
                        _lexemTable.Add(new Lexem(_subString, LexemType.SimpleType, _lexemBegin, lexemLinePositon));
                        return LexemAnalyzerState.OK;
                    }
                    
                    if (_keyWordsTable.Find(_subString))
                    {
                        _lexemBegin += _forward + 1;
                        _lexemTable.Add(new Lexem(_subString, LexemType.KeyWord, _lexemBegin, lexemLinePositon));
                        return LexemAnalyzerState.OK;
                    }

                    if(_operationTable.Find(_subString))
                    {
                        _lexemBegin += _forward + 1;
                        _lexemTable.Add(new Lexem(_subString, LexemType.KeyWord, _lexemBegin, lexemLinePositon));
                        return LexemAnalyzerState.OK;
                    }
                    
                    if (IdentTable.Find(_subString))
                    {
                        _lexemBegin += _forward + 1;
                        lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                        chr = _code[_lexemBegin + _forward + 1].ToString();
                        identifierType = IdentTable.IdentifyType(chr);

                        if(identifierType== IdentifierType.SimpleType)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                chr = _code[_lexemBegin + _forward + 1].ToString();
                                _subString += chr;
                            }

                            if (_subString == "початок")
                            {
                                identifierType = IdentifierType.Structure;
                                _typeTable.AddUserType(_subString);
                            }
                        }

                        _identTable.Add(new Identifier(_subString, identifierType, _lexemBegin, lexemLinePositon));
                        return LexemAnalyzerState.OK;
                    }

                    /*literalType = LiteralTable.Find(_subString);
                    if (literalType!= LiteralType.Error)
                    {
                        _lexemBegin += _forward + 1;
                        _literalTable.Add(new Lexem(_subString, LexemType.SimpleType, _lexemBegin, lexemLinePositon, literalType));
                        return LexemAnalyzerState.OK;
                    }*/
                }
                else
                {
                    _subString += chr;
                }

                if (_subString.Length > 100)
                    return LexemAnalyzerState.SizeError;

                _forward++;
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

        public void Save_Lex()
        {
            _lexemTable.SaveToFile();
        }

        public void Save_Lit()
        {
            LiteralTable.SaveToFile();
        }
    }
}