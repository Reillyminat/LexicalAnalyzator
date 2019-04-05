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
            string code = @"ціле дійсне логіка символ";

            LexicalAnalyzer lex = new LexicalAnalyzer();

            lex.SetCode(code);

            var state =  lex.Tokenizer();

            bool found = false;

            found = lex.LexemTable.Find(new Lexem("ціле", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("дійсне", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("логіка", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("символ", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);


        }

        [TestMethod]
        public void Tokenize_Recongnize_AllKeyWords()
        {
            string code = @"якщо доки інакше початок кінець повернути";

            LexicalAnalyzer lex = new LexicalAnalyzer();

            lex.SetCode(code);

            var state = lex.Tokenizer();

            bool found = false;

            found = lex.LexemTable.Find(new Lexem("якщо", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("доки", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("інакше", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("початок", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("кінець", LexemType.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("повернути", LexemType.SimpleType, 0, 0));
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
