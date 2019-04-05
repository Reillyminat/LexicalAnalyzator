using Microsoft.VisualStudio.TestTools.UnitTesting;
using LexicalAnalyzerApplication;

namespace UnitTestProject1
{
    [TestClass]
    public class LexicalAnalyzerTest
    {
        [TestMethod]
        public void Test_Recongnize_JustAllSimpleTypes()
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
    }
}
