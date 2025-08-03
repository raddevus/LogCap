namespace LogCap.Model;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class WebInfoContext : DbContext
{
    // The variable name must match the name of the table.
    public DbSet<WebInfo> WebInfo { get; set; }
    
    public string DbPath { get; }

    public WebInfoContext()
    {
        var path = Environment.CurrentDirectory;
        DbPath = System.IO.Path.Join(path, "logcap.db");
        Console.WriteLine(DbPath);

        SqliteConnection connection = new SqliteConnection($"Data Source={DbPath}");
        // ########### FYI THE DB is created when it is OPENED ########
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        FileInfo fi = new FileInfo(DbPath);
        // check to see if db file is 0 length, if so, it needs to have table added
        if (fi.Length == 0){
            foreach (String tableCreate in allTableCreation){
                command.CommandText = tableCreate;
                command.ExecuteNonQuery();
            }
        }
    }

    // configures the database for use by EF
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    protected String [] allTableCreation = {
        @"CREATE TABLE WebInfo 
            (
            [ID] INTEGER NOT NULL PRIMARY KEY,
            [SiteDesc] NVARCHAR(130) NOT NULL check(length(SiteDesc) <= 130),
            [IpAddress] NVARCHAR(60) check(length(IpAddress) <= 60),
            [RefUrl] NVARCHAR(160) check(length(RefUrl) <= 160),
            [Info] NVARCHAR(160) check(length(Info) <= 160),
            [Created] NVARCHAR(30) default (datetime('now','localtime')) 
                      check(length(Created) <= 30)
            ); PRAGMA journal_mode=WAL;"
    };

}
