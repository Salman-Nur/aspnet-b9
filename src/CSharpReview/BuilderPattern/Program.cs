// See https://aka.ms/new-console-template for more information

using BuilderPattern;

string connectionString = 
    new ConnectionStringBuilder("localhost", "AspnetB9")
        .UseMultipleActiveRecordSet()
        .SetCredentials("aspnetb9", "123456")
        .UsePort(2222)
        .GetConnectionString();

Console.WriteLine(connectionString);
