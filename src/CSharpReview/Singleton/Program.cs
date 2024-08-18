// See https://aka.ms/new-console-template for more information
using Singleton;

Console.WriteLine("Hello, World!");

Logger logger1 = Logger.CreateLogger();
Logger logger2 = Logger.CreateLogger();

if(logger1 == logger2)
    Console.WriteLine("Same");
