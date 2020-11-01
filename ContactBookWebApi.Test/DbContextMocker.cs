using ContactBookWebApi.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactBookWebApi.Test
{
    public class DbContextMocker
    {
        public static ContactDbContext GetContactDbContext( string dbName)
        {
            var options = new DbContextOptionsBuilder<ContactDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new ContactDbContext(options);

            dbContext.Seed();

            return dbContext;
        }
    }
}
