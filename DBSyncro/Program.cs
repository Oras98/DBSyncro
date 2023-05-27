// See https://aka.ms/new-console-template for more information

using DBSyncro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
string CurrDir = Directory.GetCurrentDirectory();
string jsonPath = @$"{CurrDir}/settings.json";

using StreamReader r = new StreamReader(jsonPath);
string json = r.ReadToEnd();
Settings settings = JsonConvert.DeserializeObject<Settings>(json);

try
{
    IDatabaseConnection? cnn1, cnn2;
    var db_type = settings.Database.Trim().ToLower();

    var DBConnectionFactory = new DBConnectionFactory();
    cnn1 = DBConnectionFactory.GetInstance(db_type, settings.connString1);
    cnn2 = DBConnectionFactory.GetInstance(db_type, settings.connString2);

    if (cnn1 is null || cnn2 is null)
        throw new Exception($"Database type {db_type} is not impemented yet!");

    using var fb_db_syncro = new ComparerController(cnn1, cnn2);
    var res = fb_db_syncro.GetDifferences(true);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"settings.json created at {AppDomain.CurrentDomain.BaseDirectory}");
    Console.ResetColor();
}
catch (Exception err) 
{
    Console.WriteLine(err.Message);
}

Console.WriteLine("Premi INVIO per chiudere l'applicazione...");
Console.ReadLine();

public class Settings
{
    public required string Database { get; set; }
    public required string connString1 { get; set; }
    public required string connString2 { get; set; }    
}