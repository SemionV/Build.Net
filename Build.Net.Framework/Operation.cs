using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class Operation
    {
        public virtual bool Run(RunContext context)
        {
            return true;
        }
    }
}
