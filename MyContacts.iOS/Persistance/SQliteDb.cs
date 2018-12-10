using System;
using System.IO;
using SQLite;
using Xamarin.Forms;

using MyContacts.iOS;
using MyContacts.Persistance;

[assembly: Dependency(typeof(SQLiteDb))]
namespace MyContacts.iOS
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
