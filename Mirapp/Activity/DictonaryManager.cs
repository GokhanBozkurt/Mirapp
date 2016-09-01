using System.Collections.Generic;

namespace Mirapp
{
    public class DictonaryManager
    {
        public static List<DictonaryWords> PrepareWordList(List<DictonaryWords> listWords, DictonaryWords words)
        {
            var lst = new List<DictonaryWords>();
            foreach (DictonaryWords item in listWords)
            {
                if (item.Language != words.Language)
                {
                    lst.Add(new DictonaryWords()
                    {
                        ID = item.ID, Word = item.TranslatedWord, TranslatedWord = item.Word, Language = item.Language
                    });
                }
                else
                {
                    lst.Add(item);
                }
            }

            return lst;
        }
    }
}