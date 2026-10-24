using ConsiliumTempus.SampleData;

// Inserts the sample data (the .sql files of the Data folder) into an already migrated database.
// It is independent from the migrations and from the Api, and it only runs when SampleData__Enabled is true.
const string enabledVariable = "SampleData__Enabled";
const string connectionFlag = "--connection";

if (!bool.TryParse(Environment.GetEnvironmentVariable(enabledVariable), out var enabled) || !enabled)
{
    Console.WriteLine($"The sample data is disabled, set {enabledVariable} to true to insert it.");
    return 0;
}

var flagIndex = Array.IndexOf(args, connectionFlag);
if (flagIndex < 0 || flagIndex + 1 >= args.Length)
{
    Console.Error.WriteLine($"Missing the connection string, use: {connectionFlag} \"<connection string>\"");
    return 1;
}

SampleDataRunner.Run(args[flagIndex + 1]);
return 0;
