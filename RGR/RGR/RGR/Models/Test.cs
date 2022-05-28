using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.Models
{
    public class Test
    {
        public string Text { get; set; }
        public bool Value { get; set; }

        public Test(string text, bool value)
        {
            Text = text;
            Value = value;
        }
    }
}
