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

        public string Tokenizer(string code)
        {
            int lexemBegin = 0;
            int forward = 0;

            int codeLength = code.Length;

            while (lexemBegin < codeLength)
            {
                string subString = "";

                bool foundDelimiter = false;

                do
                {
                    string chr = code[lexemBegin + forward].ToString();
                    foundDelimiter = _delimiterTable.Find(chr);
                    
                    if (foundDelimiter)
                    {
                        bool foundType = _typeTable.Find(subString);

                        bool foundKeyWord = _keyWordsTable.Find(subString);

                        bool foundIdentifier = _identTable.Find(subString);
                    }
                    else
                    {
                        subString = subString + code[lexemBegin + forward];
                    }

                    forward += 1;
                }
                while (!foundDelimiter);

                lexemBegin += forward;

                forward = 0;
 
            }

            _typeTable.SafeToFile();
            return "";
        }

        private void FoundKeyWord()
        {

        }

        private bool FindLexemType(string subString)
        {
            return false;
        }

    }
}