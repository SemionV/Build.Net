using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class BuildProject
    {
        public Dictionary<object, Type> Operations { get; private set; }
        public Type Context { get; set; }

        public BuildProject()
        {
            Operations = new Dictionary<object, Type>();
        }
    }
}
