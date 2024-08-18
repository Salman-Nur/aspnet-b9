// See https://aka.ms/new-console-template for more information


using FactoryPattern;

CarFactory factory = new BMWFactory();

ICar car = factory.Create("Red", "X2009", 2000);
