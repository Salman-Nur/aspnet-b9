// See https://aka.ms/new-console-template for more information


using AbstractFactory;
using AbstractFactory.BMW;

CarFactory factory = new BMWFactory();
IEngine engine = factory.CreateEngine();
ITire tire = factory.CreateTire();
IHeadLight headlight = factory.CreateHeadLight();
