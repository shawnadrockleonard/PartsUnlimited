using Microsoft.EntityFrameworkCore;
using PartsUnlimited.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartsUnlimited.UnitTests.Utils
{
    public static class DbHelper
    {
        public static DbContextOptions<PartsUnlimitedContext> GetFakeDbOptions(string databaseName = null, bool enableSensitiveDataLogging = false)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                databaseName = Guid.NewGuid().ToString();
            }

            var options = new DbContextOptionsBuilder<PartsUnlimitedContext>()
                    .UseInMemoryDatabase(databaseName)
                    .EnableSensitiveDataLogging(enableSensitiveDataLogging)
                .Options;
            return options;
        }

        public static PartsUnlimitedContext GetFakeDbContext(DbContextOptions<PartsUnlimitedContext> dbContextOptions = null)
        {
            if (dbContextOptions == null)
            {
                dbContextOptions = GetFakeDbOptions();
            }

            return new PartsUnlimitedContext(dbContextOptions);
        }
    }
}
