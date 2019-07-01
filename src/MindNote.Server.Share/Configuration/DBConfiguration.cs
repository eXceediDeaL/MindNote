using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindNote.Server.Share.Configuration
{
    public enum DBType
    {
        MSSqlServer,
        MySql,
    }

    public class DBConfiguration
    {
        public string ConnectionString { get; set; }

        public DBType Type { get; set; }

        public static DBConfiguration Load(IConfiguration configuration) => configuration.GetSection("db").Get<DBConfiguration>();
    }
}
