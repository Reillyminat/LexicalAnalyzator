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

            found = lex.LexemTable.Find(new Lexem("ціле", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("дійсне", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("логіка", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("символ", LexemKind.SimpleType, 0, 0));
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

            found = lex.LexemTable.Find(new Lexem("якщо", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("доки", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("інакше", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("початок", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("кінець", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("повернути", LexemKind.SimpleType, 0, 0));
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

            found = lex.LexemTable.Find(new Lexem("*", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("/", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("+", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("-", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem(">", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("<", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("==", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("!=", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem(">=", LexemKind.SimpleType, 0, 0));
            Assert.IsTrue(found);

            found = false;
            found = lex.LexemTable.Find(new Lexem("<=", LexemKind.SimpleType, 0, 0));
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

            code = "країна";
            expected = true;
            actual = id.Find(code);
            Assert.AreEqual(expected, actual);

            IdentifierKind exp;
            IdentifierKind act;

            code = "SomeFunction()  ";
            exp = IdentifierKind.Function;
            act = id.IdentifyType(code);
            Assert.AreEqual(exp, act);

            code = "SomeArray[]";
            exp = IdentifierKind.Array;
            act = id.IdentifyType(code);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Tokenizer_Recognize_Literal()
        {
            LiteralTable lit = new LiteralTable();
            string code;
            LexemType expected;
            LexemType actual;

            code = "\"text&^%$#@/*\"";
            expected = LexemType.Symbol;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "\"123text123\"";
            expected = LexemType.Symbol;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "123";
            expected = LexemType.Int;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "123.1";
            expected = LexemType.Double;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "правда";
            expected = LexemType.Logic;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "брехня";
            expected = LexemType.Logic;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = ".123";
            expected = LexemType.Error;
            actual = lit.Find(code);
            Assert.AreEqual(expected, actual);

            code = "1.2.3";
            expected = LexemType.Error;
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
            //tok.SetCode("школа\nпочаток\nціле номер\nсимвол назва\nкінець\nфункція друк_школи(школа дані)\nпочаток\nписати(дані.номер)\nписати(дані.назва)\nкінець\nфункція головна()\nпочаток\nшкола школи[10]\nціле а = 0\nдоки(а менше 9)\nпочаток\nписати(\"Введіть назву школи\")\nчитати(школи[а].назва)\nписати(\"Введіть номер школи\")\nчитати(школи[а].номер)\nа = а + 1\nкінець\nякщо(школи[0].номер не дорівнює 1)\nпочаток\nписати(\"Є школа №1\")\nкінець\nінакше\nпочаток\nписати(\"Немає школи №1\")\nкінець\nкінець\n");
            tok.SetCode("\"деякий текст для зчитування\" функція");
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
