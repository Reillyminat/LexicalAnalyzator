using System.Collections.Generic;
using System.IO;

namespace LexicalAnalyzerApplication
{
    public enum ParsingState { OK, IdentifyError }
    class SA
    {
        List<State> states;
        List<Lexem> lT;
        List<string> changedCode;
        Stack<int> StateStack;
        CG code;
        public SA(LexicalAnalyzer lexicalAnalyzer)
        {
            StateStack = new Stack<int>();
            lT = lexicalAnalyzer.LexemTable.Lexems;
            changedCode = new List<string>();
            foreach (Lexem token in lT)
            {
                changedCode.Add(" ");
                changedCode.Add(token.Name);
            }
            code = new CG();
        }
        public ParsingState Parse(ref int state, ref int position)
        {
            string buff = "";
            while (true)
            {
                if (position + 1 == lT.Count && state == 3)
                {
                    code.SaveToFile();
                    code.TransferCode(changedCode);
                    return ParsingState.OK;
                }
                if (state == -2 && position < lT.Count)
                    return ParsingState.IdentifyError;
                if (states[state].Action != -1)
                {
                    code.Generate(states[state].Action);
                    changedCode.Insert(position*2, " ");
                    changedCode.Insert(position*2+1, "<A"+ states[state].Action + ">");
                }
                if (lT[position].LexemKind == LexemKind.Literal)
                    buff = "літерал";
                else if (lT[position].LexemKind == LexemKind.Identifier)
                    buff = "ідентифікатор";
                else
                    buff = lT[position].Name;
                if (buff == "\n")
                    buff = "\\n";
                if (states[state].Find(buff))
                {
                    /*действия если принять*/
                    if (states[state].Accept == true)
                    {
                        //принять терминал перейти в следующее состояние
                        //или извлечь состояние из стека и return
                        if (states[state].FromStack == true)
                        {
                            state = StateStack.Pop();
                        }
                        else state = states[state].Jump;
                        position++;
                        continue;
                    }
                    else/*действия если не принять*/
                    {
                        //заслать в стек следующее состояние (если нужно)
                        //и перейти в нужное состояние
                        //и продолжить разбор
                        if (states[state].ToStack != -1)
                        {
                            StateStack.Push(state + 1);
                            state = states[state].Jump;
                            continue;
                        }
                        if (states[state].Jump == -1)
                            state = StateStack.Pop();
                        else state = states[state].Jump;
                        continue;
                    }
                }
                else /*если ошибка то выдать сообщение иначе перейти в следующее состояние*/
                {
                    //если поле ошибки = 1 то проверить стек
                    //если он не пустой то извлечь состояние из стека
                    //иначе выдать сообщение об ошибке
                    //иначе перейти на альтернативное (следующее) правило
                    if (states[state].Suppress != true)
                    {
                        return ParsingState.IdentifyError;
                    }
                    else
                    {
                        state++;
                        continue;
                    }
                }
            }
        }
        public void ReadFromFile()
        {
            using (FileStream fs = new FileStream(@"State machine.txt", FileMode.Open, FileAccess.Read))
            using (StreamReader sw = new StreamReader(fs))
            {
                int i = 0;
                string buff = "";
                string expected = "";
                string temp = "";
                bool terminal;
                int toStack;
                bool fromStack;
                bool supress;
                int jump;
                int action;
                states = new List<State>();
                buff = sw.ReadLine();
                buff = sw.ReadLine();
                while (!sw.EndOfStream)
                {
                    buff = sw.ReadLine();
                    if (buff[i] == ' ')
                    {
                        while (buff[i] == ' ')
                            i++;
                        while (i < buff.Length && buff[i] != ' ')
                        {
                            temp += buff[i];
                            i++;
                        }
                        expected += temp + " ";
                        temp = "";
                    }
                    else
                    {
                        while (buff[i] != ' ')
                            i++;
                        while (buff[i] == ' ')
                            i++;
                        while (buff[i] != ' ')
                        {
                            expected += buff[i];
                            i++;
                        }
                        while (buff[i] == ' ')
                            i++;
                        while (buff[i] != ' ')
                        {
                            temp += buff[i];
                            i++;
                        }
                        jump = int.Parse(temp);
                        temp = "";
                        while (buff[i] == ' ')
                            i++;
                        while (buff[i] != ' ')
                        {
                            temp += buff[i];
                            i++;
                        }
                        if (temp == "True")
                            terminal = true;
                        else terminal = false;
                        temp = "";
                        while (buff[i] == ' ')
                            i++;
                        while (buff[i] != ' ')
                        {
                            temp += buff[i];
                            i++;
                        }
                        toStack = int.Parse(temp);
                        temp = "";
                        while (buff[i] == ' ')
                            i++;
                        while (buff[i] != ' ')
                        {
                            temp += buff[i];
                            i++;
                        }
                        if (temp == "True")
                            fromStack = true;
                        else fromStack = false;
                        temp = "";
                        while (buff[i] == ' ')
                            i++;
                        while (i < buff.Length && buff[i] != ' ')
                        {
                            temp += buff[i];
                            i++;
                        }
                        if (temp == "True")
                            supress = true;
                        else supress = false;
                        temp = "";
                        while (buff[i] == ' ')
                            i++;
                        while (i < buff.Length && buff[i] != ' ')
                        {
                            temp += buff[i];
                            i++;
                        }
                        action = int.Parse(temp);
                        temp = "";
                        states.Add(new State(expected, terminal, toStack, fromStack, supress, jump, action));
                        expected = "";
                    }
                    i = 0;
                }
                //SaveToFile();
            }
        }
        public void SaveToFile()
        {
            using (FileStream fs = new FileStream(@"State machine1.txt", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                int i = 0;
                int beginPos = 0, currentPos = 0;
                string buff = "";
                List<string> temp = new List<string>();
                sw.WriteLine("{0,-3} {1,-30} {2,-3} {3,-3} {4,-3} {5,-3} {6,-3} {7,-3}\n", "№", "Ожидаемый терминал", "Переход", "Принять", "В стек", "Из стека", "Ошибка", "Действия ГК");
                foreach (State state in states)
                {
                    buff = state.Name;
                    while (buff[buff.Length - 1] == ' ')
                        buff = buff.Substring(0, buff.Length - 1);
                    if (state.Jump != -1 && state.Jump > 138)
                        state.Jump += 1;
                    if (state.ToStack != -1 && state.ToStack > 138)
                        state.ToStack += 1;
                    if (i == 139)
                        i = 140;
                    if (!buff.Contains(" "))
                        sw.WriteLine("{0,-3} {1,-30} {2,-7} {3,-7} {4,-6} {5,-8} {6,-7} {7,-3}", i, state.Name, state.Jump, state.Accept, state.ToStack, state.FromStack, state.Suppress, -1);
                    else
                    {
                        do
                        {
                            while (currentPos != buff.Length && buff[currentPos] != ' ')
                                currentPos++;
                            temp.Add(buff.Substring(beginPos, currentPos - beginPos));
                            beginPos = currentPos + 1;
                            currentPos++;
                        } while (currentPos < buff.Length);
                        for (int j = 0; j < temp.Count - 1; j++)
                            sw.WriteLine("{0,-3} {1,0}", "", temp[j]);
                        sw.WriteLine("{0,-3} {1,-30} {2,-7} {3,-7} {4,-6} {5,-8} {6,-7} {7,-3}", i, temp[temp.Count - 1], state.Jump, state.Accept, state.ToStack, state.FromStack, state.Suppress, -1);
                        currentPos = 0;
                        beginPos = 0;
                        temp.RemoveRange(0, temp.Count);
                    }
                    i++;
                }
            }
        }
    }
}
