﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerApplication
{
    public enum LexemType { Identifier, KeyWord, Delimeter, Operation, SimpleType}

    public class Lexem
    {
        LexemType _lexemType;
        int _codePosition;
        int _lineNumber;

        public int LineNumber { get => _lineNumber; set => _lineNumber = value; }
        public int CodePosition { get => _codePosition; set => _codePosition = value; }
        internal LexemType LexemType { get => _lexemType; set => _lexemType = value; }

        public Lexem()
        {
            _codePosition = -1;
            _lineNumber = -1;
        }

        public Lexem(LexemType lexemType, int codePosition, int lineNumber)
        {
            _lexemType = LexemType;
            _codePosition = codePosition;
            _lineNumber = lineNumber;
        }
    }
}
