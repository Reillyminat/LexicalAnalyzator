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
        LexemType literalType;
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
                if ((_lexemBegin + _forward) == _code.Length-1)
                {
                    foundDelimiter = true;
                    chr = _code[_lexemBegin + _forward].ToString();

                    if (!_delimiterTable.FindSkip(chr) && !_delimiterTable.FindSkip(_subString))
                        _subString += chr;
                    else if (!_delimiterTable.FindSkip(_subString)&&_subString!="")
                    { }
                        else return LexemAnalyzerState.EOF;
                }
                else
                {
                    chr = _code[_lexemBegin + _forward].ToString();
                    while ((_subString == "") &&(_delimiterTable.FindSkip(chr)))
                    {
                        if (_code.Length - 1 == _lexemBegin)
                            return LexemAnalyzerState.EOF;
                        _lexemBegin++;
                        chr = _code[_lexemBegin + _forward].ToString();
                    }
                    foundDelimiter = _delimiterTable.Find(chr);
                    if (chr == "\"")
                    {
                        do
                        {
                            _subString += chr;
                            _forward++;
                            chr = _code[_lexemBegin + _forward].ToString();
                        } while (chr != "\"");
                        literalType = LiteralTable.Find(_subString);
                        lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                        _literalTable.Add(new Lexem(_subString, LexemKind.SimpleType, ++_lexemBegin, lexemLinePositon, literalType));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }
                }
 
                if (foundDelimiter)
                {
                   //Count '\n' before lexem
                    lexemLinePositon = CountLexemLinePosition(_lexemBegin);

                    if (_typeTable.Find(_subString))
                    {
                        _lexemTable.Add(new Lexem(_subString, LexemKind.SimpleType, _lexemBegin, lexemLinePositon));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }
                    
                    if (_keyWordsTable.Find(_subString))
                    {
                        _lexemTable.Add(new Lexem(_subString, LexemKind.KeyWord, _lexemBegin, lexemLinePositon));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }
                    
                    if (IdentTable.Find(_subString))
                    {
                        lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                        chr = _code[_lexemBegin + _forward].ToString();
                        identifierType = IdentTable.IdentifyType(chr);

                        if(identifierType== IdentifierType.SimpleType)
                        {
                            if((_lexemBegin+_forward+7)>_code.Length-1)
                                {}
                            else {
                                string word="";
                            for (int i = 0; i < 7; i++)
                            {
                                chr = _code[_lexemBegin + _forward + 1+i].ToString();
                                word += chr;
                            }

                            if (word == "початок")
                            {
                                identifierType = IdentifierType.Structure;
                                _typeTable.AddUserType(_subString);
                            }
                            }
                        }

                        _identTable.Add(new Identifier(_subString, identifierType, _lexemBegin, lexemLinePositon));
                        _lexemTable.Add(new Lexem(_subString, LexemKind.Identifier, _lexemBegin, lexemLinePositon));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }

                    literalType = LiteralTable.Find(_subString);

                    if (literalType!= LexemType.Error)
                    {
                        //_literalTable.Add(new Lexem(_subString, LexemKind.SimpleType, _lexemBegin, lexemLinePositon, literalType));
                        _lexemTable.Add(new Lexem(_subString, LexemKind.SimpleType, _lexemBegin, lexemLinePositon, literalType));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }

                    if (_operationTable.Find(_subString))
                    {
                        _lexemTable.Add(new Lexem(_subString, LexemKind.Operation, _lexemBegin, lexemLinePositon));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }
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
            int count = 1;

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
    }
}