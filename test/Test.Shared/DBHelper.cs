using Microsoft.EntityFrameworkCore;
using System;

namespace Test.Shared
{
    public static class DBHelper
    {
        public static T CreateContext<T>(string dbname) where T : DbContext
        {
            DbContextOptions<T> options = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName: dbname)
                .Options;

            T context = (T)Activator.CreateInstance(typeof(T), options);

            return context;
        }
    }
}
