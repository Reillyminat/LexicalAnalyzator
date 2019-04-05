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


    }
}
