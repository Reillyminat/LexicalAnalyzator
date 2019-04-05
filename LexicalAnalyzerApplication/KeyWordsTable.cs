﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace LexicalAnalyzerApplication
{
    public class KeyWordsTable
    {
        List<string> _keyWordsTable;

        public KeyWordsTable()
        {
            _keyWordsTable = new List<string>();
            
            _keyWordsTable.Add("функція");
            _keyWordsTable.Add("якщо");
            /*_keyWordsTable.Add("інакше");
            _keyWordsTable.Add("доки");
            _keyWordsTable.Add("початок");
            _keyWordsTable.Add("кінець");
            _keyWordsTable.Add("повернути");
            _keyWordsTable.Add("ціле");
            _keyWordsTable.Add("символ");
            _keyWordsTable.Add("дійсне");
            _keyWordsTable.Add("структура");
            _keyWordsTable.Add("символ");
            _keyWordsTable.Add("дійсне");
            _keyWordsTable.Add("логіка");
              */
        }
        public int Count { get => _keyWordsTable.Count; }

        public bool Find(string str)
        {
            string found_name = _keyWordsTable.Find(x => x == str);

            if (found_name == null)
                return false;
            else
            {
                _keyWordsTable.Add(found_name);
                return true;
            }
        }
    }
}
