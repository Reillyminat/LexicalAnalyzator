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

            //lex.LexemTable.Find(new Lexem(""))

          
        }
    }
}
