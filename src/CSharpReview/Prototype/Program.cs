// See https://aka.ms/new-console-template for more information
using Prototype;

Console.WriteLine("Hello, World!");

Product p1 = new Product();
p1.Name = "Cemera";
p1.Price = 30000;

Product p2 = (Product)p1.Clone();

if(p1 != p2)
    Console.WriteLine("Different object");

Console.WriteLine($"{p2.Name}, {p2.Price}");
