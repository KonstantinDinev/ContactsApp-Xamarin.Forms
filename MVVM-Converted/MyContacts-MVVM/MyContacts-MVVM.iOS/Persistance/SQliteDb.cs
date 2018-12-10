using System;
using System.IO;
using SQLite;
using Xamarin.Forms;

using MyContactsMVVM.Persistance;
using MyContacts_MVVM.iOS.Persistance;

[assembly: Dependency(typeof(SQLiteDb))]
namespace MyContacts_MVVM.iOS.Persistance
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite15.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}
