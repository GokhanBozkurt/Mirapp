using SQLite;

namespace Mirapp
{
    public class DictonaryWords
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Word { get; set; }

        public string TranslatedWord { get; set; }

        public string Language { get; set; }

        public override string ToString()
        {
            return string.Format("{0}  --->>> {1} ", Word, TranslatedWord);
        }
    }
}