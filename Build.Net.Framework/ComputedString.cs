using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Tasks
{
    public class ComputableString
    {
        public string Template { get; set; }

        public ComputableString()
        {

        }

        public ComputableString(string template)
        {
            Template = template;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
