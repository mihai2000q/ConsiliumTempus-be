using System.Globalization;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace ConsiliumTempus.SampleData;

/// <summary>
///     Runs the <c>.sql</c> files of the Data folder, that insert the sample data used for both backend and frontend development.
///     The files are embedded in the assembly and are executed by the order of their names (<c>1.user.sql</c>, <c>2.workspace.sql</c>...),
///     so adding sample data for a new table means adding a new file.
///     The sample data is only inserted into an empty database (without users), so it can run multiple times.
/// </summary>
internal static class SampleDataRunner
{
    public static void Run(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        if (HasUsers(connection))
        {
            Console.WriteLine("The database already has users, skipping the sample data.");
            return;
        }

        var assembly = Assembly.GetExecutingAssembly();
        var scripts = assembly
            .GetManifestResourceNames()
            .Where(name => name.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
            .Order(StringComparer.Create(CultureInfo.InvariantCulture, CompareOptions.NumericOrdering))
            .ToList();

        // all or nothing, so the sample data is never partially inserted
        using var transaction = connection.BeginTransaction();
        foreach (var script in scripts)
        {
            Console.WriteLine($"Inserting {script}...");
            Execute(connection, transaction, ReadScript(assembly, script));
        }

        transaction.Commit();
        Console.WriteLine("The sample data has been inserted.");
    }

    private static bool HasUsers(SqlConnection connection)
    {
        using var command = new SqlCommand("SELECT CASE WHEN EXISTS (SELECT 1 FROM [User]) THEN 1 ELSE 0 END", connection);
        return (int)command.ExecuteScalar()! == 1;
    }

    private static string ReadScript(Assembly assembly, string name)
    {
        using var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static void Execute(SqlConnection connection, SqlTransaction transaction, string script)
    {
        using var command = new SqlCommand(script, connection, transaction);
        command.ExecuteNonQuery();
    }
}
