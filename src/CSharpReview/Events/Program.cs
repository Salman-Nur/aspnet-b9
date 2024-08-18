
using Events;

TextBox textbox1 = new TextBox();
textbox1.OnTextChange += PrintText;
textbox1.OnTextChange += PrintText2;

textbox1.AddText("hello");

textbox1.OnTextChange -= PrintText2;

textbox1.AddText("hello");

void PrintText(string text)
{
    Console.WriteLine(text);
}

void PrintText2(string text)
{
    Console.WriteLine(text);
}