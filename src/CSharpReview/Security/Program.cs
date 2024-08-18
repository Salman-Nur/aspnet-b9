﻿
using AdonetExample;

var connectionString = "Server=.\\SQLEXPRESS;Database=AspnetB9;User Id=aspnetb9;Password=123456;Trust Server Certificate=True;";
AdonetUtility adonetUtility = new AdonetUtility(connectionString);
// ' or 1=1;drop table Courses;--
var title = Console.ReadLine();
var sql = "select * from Courses where title = @title";
var data = adonetUtility.GetData(sql, new Dictionary<string, object>
{
    { "title", title }
});

if (data is not null)
{
    foreach (var row in data)
    {
        foreach (var col in row)
        {
            Console.Write(col);
            Console.Write(" ");
        }
        Console.WriteLine();
    }
}