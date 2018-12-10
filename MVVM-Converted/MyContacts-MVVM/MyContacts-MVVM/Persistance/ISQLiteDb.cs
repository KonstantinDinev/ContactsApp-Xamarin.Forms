using System;
using SQLite;

namespace MyContactsMVVM.Persistance
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
