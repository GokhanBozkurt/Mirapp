using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Mirapp.Data
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
            return string.Format("Word={0}  TranslatedWord={1} ", Word, TranslatedWord);
        }
    }

    class Repository<T>
    {
        private SQLiteConnection connection;
        public SQLiteException RepositoryException { get; private set; }

        private string path
        {
            get
            {
                var folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var toDatabase = System.IO.Path.Combine(folderPath, "db_sqlnet.db");
                return toDatabase;
            }
        }


        public void Open()
        {
            connection = new SQLiteConnection(path);
        }

        public void Close()
        {
            connection.Close();
        }

        public bool DeleteTable()
        {
            try
            {
                connection.DeleteAll<T>();
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        public bool DropTable()
        {
            try
            {
                connection.DropTable<T>();
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        public string CreateTable()
        {
            try
            {
                connection.CreateTable<T>();
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public bool Insert(T data)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.Insert(data) != 0)
                    db.Update(data);
                return true;
            }
            catch (SQLiteException ex)
            {
                RepositoryException = ex;
                return false;
            }
        }


        public bool Update(T data)
        {
            try
            {
                var db = new SQLiteConnection(path);
                db.Update(data);
                return true;
            }
            catch (SQLiteException ex)
            {
                RepositoryException = ex;
                return false;
            }
        }

        public bool Delete(T data)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.Delete(data) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SQLiteException ex)
            {
                RepositoryException = ex;
                return false;
            }
        }

        private string insertUpdateAllData(IEnumerable<T> data)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "List of data inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public int findNumberRecords()
        {
            try
            {
                var db = new SQLiteConnection(path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM DictonaryWords");

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM DictonaryWords WHERE Word="Amy");

                return count;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }


        public List<DictonaryWords> GetRecords()
        {
            try
            {
                var db = new SQLiteConnection(path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.Table<DictonaryWords>();

               return count.ToList<DictonaryWords>();
                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM DictonaryWords WHERE Word="Amy");

            }
            catch (SQLiteException)
            {
                return null;
            }
        }

        public DictonaryWords GetRecord(DictonaryWords dictonaryWords)
        {
            try
            {
                var db = new SQLiteConnection(path);
                var records = db.Table<DictonaryWords>().Where(a=> a.ID== dictonaryWords.ID);
                return records.FirstOrDefault();

            }
            catch (SQLiteException)
            {
                return null;
            }
        }

    }
}