using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class Operation<T> where T : RunContext
    {
        public BuildEngine BuildEngine { get; private set; }

        public Operation(BuildEngine buildEngine)
        {
            BuildEngine = buildEngine;
        }

        public virtual bool Run(T context) 
        {
            return true;
        }
    }
}
