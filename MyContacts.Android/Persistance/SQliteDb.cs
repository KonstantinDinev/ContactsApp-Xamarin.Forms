using System;
using System.IO;
using SQLite;
using Xamarin.Forms;

using MyContacts.Droid;
using MyContacts.Persistance;

[assembly: Dependency(typeof(SQLiteDb))]
namespace MyContacts.Droid
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
