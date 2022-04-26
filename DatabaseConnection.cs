using Npgsql;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace SaintSeedCs;

public class DatabaseConnection
{
    private static string Host = "localhost";
    private static string User = "saintminer";
    private static string DBname = DotNetEnv.Env.GetString("DB_NAME");
    private static string Password = DotNetEnv.Env.GetString("DB_PASSWORD");
    private static string Port = "5432";

    public DatabaseConnection()
    {
        DotNetEnv.Env.Load();
        var connString =
            String.Format(
                "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                Host,
                User,
                DBname,
                Port,
                Password);
        
        var connection = new NpgsqlConnection(connString);
        var compiler = new PostgresCompiler();
        var db = new QueryFactory(connection, compiler);
        var vrooms = db.Query("vrooms").Get();
        Console.WriteLine(vrooms);
        foreach (var vroom in vrooms)
        {
            Console.WriteLine(vroom);
            Console.WriteLine(vroom.name);
        }
    }
}