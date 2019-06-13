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
            int subClass = 0;
            bool foundSkip = false;
            int delimeterSubClass = 0;
            if (_code.Length == _lexemBegin)
                return LexemAnalyzerState.EOF;
            do
            {
                if (_lexemBegin + _forward + 1 < _code.Length)
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
                    foundSkip = true;
                    chr = _code[_lexemBegin + _forward].ToString();

                    if (!_delimiterTable.FindSkip(chr) && !_delimiterTable.FindSkip(_subString))
                        _subString += chr;
                    else if (!_delimiterTable.FindSkip(_subString) && _subString != "")
                    { }
                    else return LexemAnalyzerState.EOF;
                }
                else
                {
                    if (chr == "\"")
                    {
                        chr = _code[_lexemBegin + ++_forward].ToString();
                        lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                        position = CalculateLexemPositionInLine(_lexemBegin);
                        _lexemTable.Add(new Lexem("\"", LexemKind.Delimeter, position, lexemLinePositon, 13));
                        do
                        {
                            if (_code.Length - 1 == _lexemBegin + _forward)
                                return LexemAnalyzerState.IdentifyError;
                            _subString += chr;
                            _forward++;
                            chr = _code[_lexemBegin + _forward].ToString();
                        } while (chr != "\"");
                        literalType = LexemType.Symbol;
                        lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                        position = CalculateLexemPositionInLine(++_lexemBegin);
                        _lexemTable.Add(new Lexem(_subString, LexemKind.Literal, position, lexemLinePositon, -1, literalType));
                        _lexemBegin += _forward;
                        position = CalculateLexemPositionInLine(_lexemBegin-1);
                        _lexemTable.Add(new Lexem("\"", LexemKind.Delimeter, position, lexemLinePositon, 13));
                        return LexemAnalyzerState.OK;
                    }
                    foundSkip = _delimiterTable.FindSkip(chr);
                    foundDelimiter = _delimiterTable.Find(chr, ref delimeterSubClass);
                }

                if (foundDelimiter)
                {
                    //Count '\n' before lexem
                    lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                    position = CalculateLexemPositionInLine(_lexemBegin);
                    if (_keyWordsTable.Find(_subString, ref subClass))
                    {
                        line = lexemLinePositon;
                        _lexemTable.Add(new Lexem(_subString, LexemKind.KeyWord, position, lexemLinePositon, subClass));
                        if (!foundSkip)
                            _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, position, lexemLinePositon, delimeterSubClass));
                        _lexemBegin += _forward + 1;
                        return LexemAnalyzerState.OK;
                    }
                    if (_delimiterTable.Find(_subString, ref delimeterSubClass) && !_delimiterTable.FindSkip(_subString))
                    {
                        _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, position, lexemLinePositon, delimeterSubClass));
                        _lexemBegin++;
                        return LexemAnalyzerState.OK;
                    }
                    if (_subString == "")
                    {

                        position = CalculateLexemPositionInLine(_lexemBegin+_forward);
                        if (!_delimiterTable.FindSkip(_subString))
                        {
                            _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, position, lexemLinePositon, delimeterSubClass));
                            _lexemBegin++;
                            foundDelimiter = false;
                        }
                    }
                    else
                    {
                        if (IdentTable.Find(_subString))
                        {
                            line = lexemLinePositon;
                            lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                            if (IdentTable.Existance(_subString, ref subClass))
                            {
                                Identifier ident = _identTable.FindIdent(_subString);
                                ident.NumberOfRepeats += 1;
                                ident.AddLineNumber(line);
                                _lexemTable.Add(new Lexem(_subString, LexemKind.Identifier, position, lexemLinePositon, subClass));
                                if (!foundSkip)
                                    _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, position, lexemLinePositon, delimeterSubClass));
                                _lexemBegin += _forward + 1;
                                return LexemAnalyzerState.OK;
                            }
                            _identTable.Add(new Identifier(_subString, position, lexemLinePositon));
                            _lexemTable.Add(new Lexem(_subString, LexemKind.Identifier, position, lexemLinePositon, subClass));
                            if (!foundSkip)
                                _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, position + _forward, lexemLinePositon, delimeterSubClass));
                            _lexemBegin += _forward + 1;
                            return LexemAnalyzerState.OK;
                        }

                        literalType = LiteralTable.Find(_subString);
                        if (literalType != LexemType.Error)
                        {
                            line = lexemLinePositon;
                            lexemLinePositon = CountLexemLinePosition(_lexemBegin);
                            _lexemTable.Add(new Lexem(_subString, LexemKind.Literal, position, lexemLinePositon, -1, literalType));
                            if (!foundSkip)
                                _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, position+_forward, lexemLinePositon, delimeterSubClass));
                            _lexemBegin += _forward + 1;
                            return LexemAnalyzerState.OK;
                        }

                        if (_operationTable.Find(_subString, ref subClass))
                        {
                            line = lexemLinePositon;
                            _lexemTable.Add(new Lexem(_subString, LexemKind.Operation, position, lexemLinePositon, subClass));
                            if (!foundSkip)
                                _lexemTable.Add(new Lexem(chr, LexemKind.Delimeter, position + _forward, lexemLinePositon, delimeterSubClass));
                            _lexemBegin += _forward + 1;
                            return LexemAnalyzerState.OK;
                        }
                    }
                }
                else
                {
                    _subString += chr;
                    _forward++;
                }
                if (_subString.Length > 100)
                    return LexemAnalyzerState.SizeError;
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

        private int CalculateLexemPositionInLine(int currentLexemPosition)
        {
            int count = 0;

            if (currentLexemPosition >= _code.Length)
                return -1;

            for (int i = currentLexemPosition; i > -1; i--)
            {
                if (_code[i] == '\n')
                    break;

                count++;
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