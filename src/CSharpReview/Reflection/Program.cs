using Reflection;
using System.Reflection;
using System.Collections;

/*
Assembly assembly = Assembly.GetExecutingAssembly();
foreach(var t in assembly.GetTypes())
{
    if(t.Name == "Program")
    {
        var methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        foreach(var m in methods)
        {
            Console.WriteLine(m.Name);
        }

    }
}

void Test(Fruit fruit)
{
    Console.WriteLine((int)fruit);
}

Test(Fruit.Apple | Fruit.Banana);

enum Fruit
{
    Apple = 1,
    Mango = 2,
    Banana = 4,
    Pinapple = 8
}

0001
0010
0100
1000

1111


*/

Course course = new Course("C#", 2000, new List<Topic> 
{ 
    new Topic{ Title = "Tool Installation" , Duration = 2 },
    new Topic{ Title = "Getting Started", Duration = 4 }
});

//Type t = typeof(Course);
Type t = course.GetType();

var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);

foreach (var property in properties)
{
    if (property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
    {
        Type p = property.GetType();
        {
            //Type[] genericArguments = p.GetGenericArguments();
            var items = (IEnumerable)property.GetValue(course);
            foreach (var item in items)
            {
                var it = item.GetType();
                var topicTypes = it.GetProperties(BindingFlags.Instance | 
                    BindingFlags.Public);

                foreach(var topicType in topicTypes)
                {
                    Console.WriteLine(topicType.GetValue(item));
                }
            }
        }
        Console.WriteLine();
    }
    else
        Console.WriteLine(property.GetValue(course));
}




