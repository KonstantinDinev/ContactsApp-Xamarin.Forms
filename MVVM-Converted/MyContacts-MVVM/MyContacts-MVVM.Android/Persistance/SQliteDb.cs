using System;
using System.IO;
using SQLite;
using Xamarin.Forms;

using MyContacts_MVVM.Droid.Persistance;
using MyContactsMVVM.Persistance;

[assembly: Dependency(typeof(SQLiteDb))]
namespace MyContacts_MVVM.Droid.Persistance
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}
