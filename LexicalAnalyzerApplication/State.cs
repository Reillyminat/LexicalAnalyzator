namespace LexicalAnalyzerApplication
{
    class State
    {
        public State(string name, bool accept, int toStack, bool fromStack, bool suppress, int jump, int action)
        {
            Name = name;
            Accept = accept;
            ToStack = toStack;
            Suppress = suppress;
            FromStack = fromStack;
            Jump = jump;
            Action = action;
        }
        public string Name { get; set; }
        public int Jump { get; set; }
        public bool Accept { get; set; }
        public int ToStack { get; set; }
        public bool FromStack { get; set; }
        public bool Suppress { get; set; }
        public int Action { get; set; }
        public bool Find(string lexem)
        {
            string temp = "";
            int i = 0;
            while (i<Name.Length)
            {
                if (temp == lexem)
                    return true;
                temp = "";
                while (i < Name.Length) {
                    if (Name[i] == ' ') {
                        i++;
                        break;
                    }
                    temp += Name[i];
                    i++;
                }
            }
            if (temp == lexem)
                return true;
            return false;
        }
    }
}
