using System.Collections.Generic;
using System.IO;

namespace LexicalAnalyzerApplication
{
    class CG
    {
        Stack<int> OperationsMark;
        Stack<Lexem> UpperStack;
        Stack<Lexem> LowerStack;
        List<string> Fours;
        int Counter;
        public CG()
        {
            OperationsMark = new Stack<int>();
            UpperStack = new Stack<Lexem>();
            LowerStack = new Stack<Lexem>();
            Fours = new List<string>();
            Counter = 0;
        }
        void PushToUS(Lexem token)
        {
            UpperStack.Push(token);
        }
        void PushToLS(Lexem token)
        {
            LowerStack.Push(token);
        }
        void PushToOM(int mark)
        {
            OperationsMark.Push(mark);
        }
        public void Generate(int action)
        {
            string temp = "";
            int local = 0;
            switch (action)
            {
                case 1:
                    //Сравнение нижнего стека с true
                    //LowerStack.Pop();
                    temp = (Fours.Count + 1) + " cmp LS true ";
                    Fours.Add(temp);
                    //Переход к ветке else
                    temp = (Fours.Count + 1) + " jump " + Counter;
                    OperationsMark.Push(Counter);
                    Counter++;
                    Fours.Add(temp);
                    break;
                case 2:
                    //Перейти по метке, пропустив else
                    temp = (Fours.Count + 1) + " jump " + Counter;
                    Fours.Add(temp);
                    //Поставить метку для перехода на else
                    temp = (Fours.Count + 1) + " mark " + OperationsMark.Pop();
                    OperationsMark.Push(Counter);
                    Counter++;
                    Fours.Add(temp);
                    break;
                case 3:
                    //Поставить метку для перехода на else
                    temp = (Fours.Count + 1) + " mark " + OperationsMark.Pop();
                    Fours.Add(temp);
                    break;
                case 4:
                    //Метка после ключевого слова начала цикла
                    //LowerStack.Pop();
                    temp = (Fours.Count + 1) + " mark " + Counter;
                    OperationsMark.Push(Counter);
                    Counter++;
                    Fours.Add(temp);
                    break;
                case 5:
                    //Сравнение НС с true
                    temp = (Fours.Count + 1) + " cmp LS true ";
                    Fours.Add(temp);
                    //Переход к ветке else
                    temp = (Fours.Count + 1) + " jump " + Counter;
                    OperationsMark.Push(Counter);
                    Counter++;
                    Fours.Add(temp);
                    break;
                case 6:
                    //Переход к ветке else
                    local = OperationsMark.Pop();
                    temp = (Fours.Count + 1) + " jump " + OperationsMark.Pop();
                    Fours.Add(temp);
                    //Поставить метку на выход из цикла
                    temp = (Fours.Count + 1) + " mark " + local;
                    Fours.Add(temp);
                    //LowerStack.Pop();
                    break;
            }
        }
        public void SaveToFile()
        {
            string[] arr = new string[4];
            int i = 0, j = 0;
            using (FileStream fs = new FileStream(@"Fours.txt", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("{0,-3} {1,-9} {2,-9} {3,-9}\n", "№", "Операция", "Операнд 1", "Операнд 2");
                foreach (string str in Fours)
                {
                    while (i < str.Length)
                    {
                        if (str[i] == ' ')
                        {
                            j++;
                            i++;
                            continue;
                        }
                        arr[j] += str[i];
                        i++;
                    }
                    i = 0; j = 0;
                    sw.WriteLine("{0,-3} {1,-9} {2,-9} {3,-9}\n", arr[0], arr[1], arr[2], arr[3]);
                    arr[0] = "";
                    arr[1] = "";
                    arr[2] = "";
                    arr[3] = "";
                }
            }
        }
        public void TransferCode(List<string> tokens)
        {
            using (FileStream fs = new FileStream(@"Genereted code.txt", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (string token in tokens)
                {
                    if (token == "\n")
                        sw.WriteLine();
                    else
                    sw.Write(token);
                }
            }
        }
    }
}
