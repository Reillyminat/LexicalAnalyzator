using Microsoft.VisualStudio.TestTools.UnitTesting;
using LexicalAnalyzerApplication;

namespace LexicalAnalyzerTests
{
    [TestClass]
    public class LexicalAnalyzerTest
    {
        [TestMethod]
        public void Tokenize_Recongnize_AllSimpleTypes()
        {
            string code = @"���� ����� ����� ������";

            LexicalAnalyzer lex = new LexicalAnalyzer();

            lex.SetCode(code);

            var state =  lex.Tokenizer();

            bool found = false;

            found = lex.LexemTable.Find(new Lexem("����", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("�����", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("�����", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("������", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);


        }

        [TestMethod]
        public void Tokenize_Recongnize_AllKeyWords()
        {
            string code = @"���� ���� ������ ������� ����� ���������";

            LexicalAnalyzer lex = new LexicalAnalyzer();

            lex.SetCode(code);

            var state = lex.Tokenizer();

            bool found = false;

            found = lex.LexemTable.Find(new Lexem("����", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("����", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("������", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("�������", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("�����", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("���������", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);
        }

        [TestMethod]
        public void Tokenize_Recognize_AllOperations()
        {
            string code = @"+ - * / == != > < >= <=";

            LexicalAnalyzer lex = new LexicalAnalyzer();

            lex.SetCode(code);

            var state = lex.Tokenizer();

            bool found = false;

            found = lex.LexemTable.Find(new Lexem("*", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("/", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("+", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("-", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem(">", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("<", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("==", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("!=", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem(">=", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("<=", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);
        }

        [TestMethod]
        public void Tokenizer_Recognize_Identifier()
        {
            IdentifierTable id = new IdentifierTable();
            string code;
            bool expected;
            bool actual;
            code = "t123";
            expected = true;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            id.Add(new Identifier("x1",0,0,0));
            code = "x1";
            expected = true;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            code = "_text";
            expected = true;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            code = "TEXT";
            expected = true;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            code = "\"text\"";
            expected = false;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            code = "&text";
            expected = false;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            code = "1text";
            expected = false;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            code = "�����";
            expected = true;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            IdentifierType exp;
            IdentifierType act;

            code = "SomeFunction()";
            exp = IdentifierType.Function;
            act = id.IdentifyType(code);
            Assert.AreEqual(exp, act);

            code = "SomeArray[]";
            exp = IdentifierType.Array;
            act = id.IdentifyType(code);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Tokenizer_Recognize_Literal()
        {
            LiteralTable lit = new LiteralTable();
            string code;
            LiteralType expected;
            LiteralType actual;

            code = "\"text&^%$#@/*\"";
            expected = LiteralType.Symbol;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "\"123text123\"";
            expected = LiteralType.Symbol;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "123";
            expected = LiteralType.Int;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "123.1";
            expected = LiteralType.Double;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "������";
            expected = LiteralType.Logic;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "������";
            expected = LiteralType.Logic;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = ".123";
            expected = LiteralType.Error;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "1.2.3";
            expected = LiteralType.Error;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tokenizer()
        {
            LexicalAnalyzer tok= new LexicalAnalyzer();
            LexemAnalyzerState expected;
            LexemAnalyzerState actual;
            //Don`t ask me what is this.
            //tok.SetCode("�����\n�������\n���� �����\n������ �����\n�����\n������� ����_�����(����� ���)\n�������\n������(���.�����)\n������(���.�����)\n�����\n������� �������()\n�������\n����� �����[10]\n���� � = 0\n����(� ����� 9)\n�������\n������(\"������ ����� �����\")\n������(�����[�].�����)\n������(\"������ ����� �����\")\n������(�����[�].�����)\n� = � + 1\n�����\n����(�����[0].����� �� ������� 1)\n�������\n������(\"� ����� �1\")\n�����\n������\n�������\n������(\"���� ����� �1\")\n�����\n�����\n");
            tok.SetCode("\"������ ����� ��� ����������\" ������� ");
            expected = LexemAnalyzerState.EOF;
            actual = tok.Tokenizer();
            actual = tok.Tokenizer();
            actual = tok.Tokenizer();
            Assert.AreEqual(expected, actual);

            /*expected = LexemAnalyzerState.EOF;
            actual = tok.Tokenizer();
            Assert.AreEqual(expected, actual);*/
        }
    }
}
