using System;

namespace LexicalAnalyzerApplication
{
    public enum LexemAnalyzerState { OK, SizeError, IdentifyError, EOF }

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

        public LexemAnalyzerState Tokenizer(ref int line, ref int position)
        {
            int _forward = 0;
            int lexemLinePositon;
            string _subString = "";
            bool foundDelimiter = false;
            string chr = "";
            bool foundDot = false;
            int subClass = 0;
            int delimeterSubClass=0;
            if (_code.Length == _lexemBegin)
                return LexemAnalyzerState.EOF;

            do
            {
                if (_lexemBegin+_forward+1<_code.Length)
                {
                    chr = _code[_lexemBegin + _forward].ToString();
                    if ((_subString == ""))
                    {
                        while (_delimiterTable.FindSkip(chr))
                        {
                            if (_code.Length - 1 == _lexemBegin)
                                return LexemAnalyzerState.EOF;
                            _lexemBegin++;
                            chr = _code[_lexemBegin + _forward].ToString();
                        }
                    }
                }
                if ((_lexemBegin + _forward) >= _code.Length - 1)
                {
                    foundDelimiter = true;
                    chr = _code[_lexemBegin + _forward].ToString();

                    if (!_delimiterTable.FindSkip(chr) && !_delimiterTable.FindSkip(_subString))
                        _subString += chr;
                    else if (!_delimiterTable.FindSkip(_subString) && _subString != "")
                    { }
                    else return LexemAnalyzerState.EOF;
                }
                else
                {
                    if (chr == ".")
                    {
                        foundDelimiter = true;
                        foundDot = true;
                    }
                    else
                    {
                        if (chr == "\"")
                        {
                            chr = _code[_lexemBegin + ++_forward].ToString();
                            do
                            {
                                if (_code.Length - 1 == _lexemBegin + _forward)
                                    return LexemAnalyzerState.IdentifyError;
                                _subString += chr;
                                _forward++;
                                chr = _code[_lexemBegin + _forward].ToString();
                            } while (chr != "\"");
                            literalType = LiteralTable.Find(_subString);
                            lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                            _lexemTable.Add(new Lexem(_subString, LexemKind.SimpleType, ++_lexemBegin, lexemLinePositon, literalType));
                            _lexemBegin += _forward;
                            return LexemAnalyzerState.OK;
                        }
                        foundDelimiter = _delimiterTable.Find(chr, ref delimeterSubClass);
                    }
                }

                if (foundDelimiter)
                {
                    if(delimeterSubClass==9)
                        position = 0;

                    //Count '\n' before lexem
                    lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                    if (_keyWordsTable.Find(_subString, ref subClass))
                    {
                        line = lexemLinePositon;
                        position++;
                        _lexemTable.Add(new Lexem(_subString, LexemKind.KeyWord, _lexemBegin, lexemLinePositon, subClass));
                        _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, _lexemBegin, lexemLinePositon, delimeterSubClass));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }

                    if (IdentTable.Find(_subString))
                    {
                        line = lexemLinePositon;
                        position++;
                        lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                        if (IdentTable.Existance(_subString, ref subClass))
                        {
                            _lexemTable.Add(new Lexem(_subString, LexemKind.Identifier, position, lexemLinePositon, subClass));
                            _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, _lexemBegin, lexemLinePositon, delimeterSubClass));
                            _lexemBegin += _forward + 1;
                            return LexemAnalyzerState.OK;
                        }
                        /*if (identifierKind == IdentifierKind.Array)
                        {
                            if (Match_Array(chr, ref _forward, ref word, number_of_param, ref foundIdent, ref foundLiteral, ref foundDot) == LexemAnalyzerState.OK)
                            { }
                            else return LexemAnalyzerState.IdentifyError;
                        }
                        if (identifierKind == IdentifierKind.SimpleType)
                        {
                            if ((_lexemBegin + _forward + 7) > _code.Length - 1)
                            { }
                            else
                            {
                                word = "";
                                for (int k = 0; k < 7; k++)
                                {
                                    chr = _code[_lexemBegin + _forward + 1 + k].ToString();
                                    word += chr;
                                }
                                if (word == "початок")
                                {
                                    identifierKind = IdentifierKind.Structure;
                                    _typeTable.AddUserType(_subString);
                                }
                            }
                        }*/

                        _identTable.Add(new Identifier(_subString, _lexemBegin, lexemLinePositon));
                        _lexemTable.Add(new Lexem(_subString, LexemKind.Identifier, _lexemBegin, lexemLinePositon, subClass));
                        _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, _lexemBegin, lexemLinePositon, delimeterSubClass));
                        if (foundDot == true)
                        {
                            lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                            _lexemTable.Add(new Lexem(".", LexemKind.Operation, _lexemBegin + _forward, lexemLinePositon, 12));
                        }
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }

                    literalType = LiteralTable.Find(_subString);

                    if (literalType != LexemType.Error)
                    {
                        line = lexemLinePositon;
                        position++;
                        _lexemTable.Add(new Lexem(_subString, LexemKind.SimpleType, _lexemBegin, lexemLinePositon, literalType));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }

                    if (_operationTable.Find(_subString, ref subClass))
                    {
                        line = lexemLinePositon;
                        position++;
                        _lexemTable.Add(new Lexem(_subString, LexemKind.Operation, _lexemBegin, lexemLinePositon,subClass));
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

       /* public LexemAnalyzerState Match_Array(string chr, ref int _forward, ref string word, int number_of_param, ref bool foundIdent, ref bool foundLiteral, ref bool foundDot)
        {
            int i = 2;
            int j = 0;
            string temp = "";
            chr = _code[_lexemBegin + _forward + i].ToString();
            do
            {
                if (_code.Length - 1 == _lexemBegin + _forward + i)
                    return LexemAnalyzerState.IdentifyError;
                word += chr;
                chr = _code[_lexemBegin + _forward + i + 1].ToString();
                i++;
            } while (chr != "]");
            if (!Int32.TryParse(word, out number_of_param))
            {
                foundIdent = true;
                while (j != word.Length)
                {
                    temp += word[j];
                    j++;
                }
            }
            else foundLiteral = true;
            if ((_lexemBegin + _forward + i < _code.Length - 1) && _code[_lexemBegin + _forward + i + 1].ToString() == ".")
            {
                _forward += i + 1;
                foundDot = true;
            }
            else
            {
                _forward += i;
                j = 0;
            }
            return LexemAnalyzerState.OK;
        }*/
    }
}