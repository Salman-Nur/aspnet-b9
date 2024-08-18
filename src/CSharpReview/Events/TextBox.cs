using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class TextBox
    {
        public string Text { get; private set; }

        public event Action<string> OnTextChange;

        public void AddText(string text)
        {
            Text = text;

            OnTextChange.Invoke(text);
        }
    }
}
