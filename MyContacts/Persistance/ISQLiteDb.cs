using System;
using SQLite;

namespace MyContacts.Persistance
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
